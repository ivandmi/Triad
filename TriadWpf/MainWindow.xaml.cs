using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Controls.Models;
using GraphX.Logic.Algorithms.LayoutAlgorithms;
using GraphX.Logic.Algorithms.OverlapRemoval;
using ShowcaseApp.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TriadCore;
using TriadWpf.GraphXModels;
using TriadWpf.ViewModels;

namespace TriadWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var logicCore = new LogicCore();
            gArea.LogicCore = logicCore;
            logicCore.Graph = new GraphExample();

            //This property sets layout algorithm that will be used to calculate vertices positions
            //Different algorithms uses different values and some of them uses edge Weight property.
            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;

            //Now we can set optional parameters using AlgorithmFactory
            //NOTE: default parameters can be automatically created each time you change Default algorithms
            logicCore.DefaultLayoutAlgorithmParams =
                               logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            //Unfortunately to change algo parameters you need to specify params type which is different for every algorithm.
            ((KKLayoutParameters)logicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;

            //This property sets vertex overlap removal algorithm.
            //Such algorithms help to arrange vertices in the layout so no one overlaps each other.
            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            //Setup optional params
            logicCore.DefaultOverlapRemovalAlgorithmParams =
                              logicCore.AlgorithmFactory.CreateOverlapRemovalParameters(OverlapRemovalAlgorithmTypeEnum.FSA);
            ((OverlapRemovalParameters)logicCore.DefaultOverlapRemovalAlgorithmParams).HorizontalGap = 50;
            ((OverlapRemovalParameters)logicCore.DefaultOverlapRemovalAlgorithmParams).VerticalGap = 50;

            //This property sets edge routing algorithm that is used to build route paths according to algorithm logic.
            //For ex., SimpleER algorithm will try to set edge paths around vertices so no edge will intersect any vertex.
            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

            //This property sets async algorithms computation so methods like: Area.RelayoutGraph() and Area.GenerateGraph()
            //will run async with the UI thread. Completion of the specified methods can be catched by corresponding events:
            //Area.RelayoutFinished and Area.GenerateGraphFinished.
            logicCore.AsyncAlgorithmCompute = false;

            gArea.SetVerticesDrag(true, true);
            gArea.SetEdgesDrag(false);

            gArea.VertexSelected += graphArea_VertexSelected;
            btnEditMode.Checked += BtnEditMode_Checked;
            btnEditMode.Unchecked += BtnEditMode_Unchecked;
            _editorManager = new EditorObjectManager(gArea, gg_zoomctrl);
        }

        private void BtnEditMode_Unchecked(object sender, RoutedEventArgs e)
        {
            _opMode = EditorOperationMode.Select;
            ClearEditMode();
        }

        private void BtnEditMode_Checked(object sender, RoutedEventArgs e)
        {
            if (btnEditMode.IsChecked == true)
            {
                _opMode = EditorOperationMode.Edit;
            }
        }

        // Код по созданию ребер, если возможно, то лучше вынести в Behavior
        // Пока так, потому что про*бался со сроками
        // Тупо скопировал из примеров библиотеки GraphX

        private EditorOperationMode _opMode = EditorOperationMode.Select;
        private VertexControl _ecFrom;
        private readonly EditorObjectManager _editorManager;

        public VertexControl VertexFromEditMode
        {
            get => _ecFrom;
        }

        public Boolean IsEditMode
        {
            get => _opMode == EditorOperationMode.Edit;
        }

        private void ClearEditMode()
        {
            if (_ecFrom != null) HighlightBehaviour.SetHighlighted(_ecFrom, false);
            _editorManager.DestroyVirtualEdge();
            _ecFrom = null;
        }

        private void CreateEdgeControl(VertexControl vc)
        {
            if (_ecFrom == null)
            {
                _editorManager.CreateVirtualEdge(vc, vc.GetPosition());
                _ecFrom = vc;
                HighlightBehaviour.SetHighlighted(_ecFrom, true);
                return;
            }
            if (_ecFrom == vc) return;

            var data = new DataEdge((DataVertex)_ecFrom.Vertex, (DataVertex)vc.Vertex);
            var ec = new EdgeControl(_ecFrom, vc, data);
            gArea.InsertEdgeAndData(data, ec, 0, true);

            HighlightBehaviour.SetHighlighted(_ecFrom, false);
            _ecFrom = null;
            _editorManager.DestroyVirtualEdge();
        }

        void graphArea_VertexSelected(object sender, VertexSelectedEventArgs args)
        {
            if (args.MouseArgs.LeftButton == MouseButtonState.Pressed)
            {
                switch (_opMode)
                {
                    case EditorOperationMode.Edit:
                        CreateEdgeControl(args.VertexControl);
                        break;
                    case EditorOperationMode.Delete:
                        SafeRemoveVertex(args.VertexControl);
                        break;
                    default:
                        if (_opMode == EditorOperationMode.Select && args.Modifiers == ModifierKeys.Control)
                            SelectVertex(args.VertexControl);
                        break;
                }
            }
        }

        private void SafeRemoveVertex(VertexControl vc)
        {
            //remove vertex and all adjacent edges from layout and data graph
            gArea.RemoveVertexAndEdges(vc.Vertex as DataVertex);
        }

        private static void SelectVertex(DependencyObject vc)
        {
            if (DragBehaviour.GetIsTagged(vc))
            {
                HighlightBehaviour.SetHighlighted(vc, false);
                DragBehaviour.SetIsTagged(vc, false);
            }
            else
            {
                HighlightBehaviour.SetHighlighted(vc, true);
                DragBehaviour.SetIsTagged(vc, true);
            }
        }
    }
}
