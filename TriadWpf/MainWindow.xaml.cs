using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Controls.Models;
using GraphX.Logic.Algorithms.LayoutAlgorithms;
using GraphX.Logic.Algorithms.OverlapRemoval;
using ShowcaseApp.WPF.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TriadCore;
using TriadWpf.Interfaces;
using TriadWpf.GraphEventArgs;
using TriadWpf.View;
using TriadWpf.View.GraphXModels;
using TriadWpf.Common;
using System.Collections.Generic;
using TriadWpf.Common.GraphEventArgs;
using TriadWpf.Common.Interfaces;
using TriadWpf.Models;
using TriadWpf.Presenter;
using System.Linq;

namespace TriadWpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainView
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
                               logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.Circular);
            //Unfortunately to change algo parameters you need to specify params type which is different for every algorithm.
            //((KKLayoutParameters)logicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;

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
            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.None;
            logicCore.EdgeCurvingEnabled = true;

            //This property sets async algorithms computation so methods like: Area.RelayoutGraph() and Area.GenerateGraph()
            //will run async with the UI thread. Completion of the specified methods can be catched by corresponding events:
            //Area.RelayoutFinished and Area.GenerateGraphFinished.
            //logicCore.AsyncAlgorithmCompute = false;

            gArea.SetVerticesDrag(true, true);
            gArea.SetEdgesDrag(false);

            gArea.VertexLabelFactory = new DefaultVertexlabelFactory();

            //_editorManager = new EditorObjectManager(gArea, gg_zoomctrl);
            
            gArea.VertexSelected += graphArea_VertexSelected;
            gArea.VertexClicked += GArea_VertexClicked;
            GraphViewManager = new GraphViewManager(gArea);
            ProcedureView = procedureView;
            VertexPropertiesView = vertexPropertiesView;
            btnEditMode.Checked += BtnEditMode_Checked;
            btnEditMode.Unchecked += BtnEditMode_Unchecked;
            _editorManager = new EditorObjectManager(gArea, zoomctrl);

            vertexView.PreviewMouseLeftButtonDown += VertexView_PreviewMouseLeftButtonDown;
            zoomctrl.PreviewDrop += dg_Area_Drop;
            GraphExample graph = new GraphExample();
            
            
            //ZoomControl.SetViewFinderVisibility(zoomctrl, Visibility.Visible);
            //gArea.ShowAllVerticesLabels();
        }

        private void VertexView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        void dg_Area_Drop(object sender, DragEventArgs e)
        {
            //how to get dragged data by its type
            var pos = zoomctrl.TranslatePoint(e.GetPosition(zoomctrl), gArea);
            OnAddVertex(new VertexEventArgs((RoutineViewItem)e.Data.GetData(typeof(RoutineViewItem)), pos));
        }

        private void GArea_VertexClicked(object sender, VertexClickedEventArgs args)
        {

            if (args.MouseArgs.LeftButton == MouseButtonState.Released)
            {
                switch (_opMode)
                {
                    case EditorOperationMode.Edit:
                        CreateEdgeControl(args.Control);
                        break;
                    case EditorOperationMode.Delete:
                        SafeRemoveVertex(args.Control);
                        break;
                    default:
                        if (_opMode == EditorOperationMode.Select && args.Modifiers == ModifierKeys.Control)
                            SelectVertex(args.Control);
                        break;
                }
            }
        }

        public event EventHandler<VertexEventArgs> AddVertex;
        public event EventHandler<VertexEventArgs> RemoveVertex;
        public event EventHandler<EdgeEventArg> AddEdge;
        public event EventHandler<EdgeEventArg> RemoveEdge;
        public event EventHandler<PolusEventArgs> AddPolusToNode;
        public event EventHandler<ProcedureEventArgs> AddProcedure;
        public event EventHandler<SimulationEventArgs> RunSimulation;
        public event EventHandler<VertexEventArgs> VertexSeleacted;
        public event EventHandler GenerateGraph;

        public IGraphViewManager GraphViewManager { get; private set; }

        public IProcedureView ProcedureView { get; }

        public IVertexPropertiesView VertexPropertiesView { get; }

        private void OnGenerateGraph()
        {
            GenerateGraph?.Invoke(this, null);
        }

        private void OnVertexSeleacted(VertexEventArgs e)
        {
            VertexSeleacted?.Invoke(this, e);
        }

        private void OnAddVertex(VertexEventArgs e)
        {
            AddVertex?.Invoke(this, e);
        }

        private void OnRemoveVertex(VertexEventArgs e)
        {
            RemoveVertex?.Invoke(this, e);
        }

        private void OnAddEdge(EdgeEventArg e)
        {
            AddEdge?.Invoke(this, e);
        }

        private void OnRemoveEdge(EdgeEventArg e)
        {
            RemoveEdge?.Invoke(this, e);
        }

        private void OnAddPolusToNode(PolusEventArgs e)
        {
            AddPolusToNode?.Invoke(this, e);
        }

        private void OnAddProcedure(ProcedureEventArgs e)
        {
            AddProcedure?.Invoke(this, e);
        }

        private void OnRunSimulate(SimulationEventArgs e)
        {
            RunSimulation?.Invoke(this, e);
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

        //Код по созданию ребер, если возможно, то лучше вынести в Behavior
        //Пока так, потому что про* бался со сроками
        // Тупо скопировал из примеров библиотеки GraphX

        private EditorOperationMode _opMode = EditorOperationMode.Select;
        private VertexControl _ecFrom;
        private Polus _polusFrom;
        private readonly EditorObjectManager _editorManager;

        private void ClearEditMode()
        {
            if (_ecFrom != null) HighlightBehaviour.SetHighlighted(_ecFrom, false);
            _editorManager.DestroyVirtualEdge();
            _ecFrom = null;
        }

        private void CreateEdgeControl(VertexControl vc)
        {
            if ((vc.Vertex as DataVertex).Poluses.Count == 0)
            {
                MessageBox.Show("В данной вершине нет полюса. Сперва добавьте полюс в вершину.");
                return;
            }
            if (_ecFrom == null)
            {
                _editorManager.CreateVirtualEdge(vc, vc.GetPosition());
                _ecFrom = vc;
                HighlightBehaviour.SetHighlighted(_ecFrom, true);
                return;
            }
            if (_ecFrom == vc) return;


            AddArcForm addArcForm = new AddArcForm((_ecFrom.Vertex as DataVertex).Poluses, (vc.Vertex as DataVertex).Poluses);
            if (addArcForm.ShowDialog() == true)
            {
                // Это событие отлиается от других тем, что не добавляет изменения во view стоит переименовать
                // или добавлять на форме дугу тоже через presenter
                OnAddEdge(new EdgeEventArg((_ecFrom.Vertex as DataVertex).NodeName, addArcForm.PolusFrom, (vc.Vertex as DataVertex).NodeName, addArcForm.PolusTo));
                var data = new DataEdge((DataVertex)_ecFrom.Vertex, (DataVertex)vc.Vertex, addArcForm.PolusFrom, addArcForm.PolusTo);
                var ec = new EdgeControl(_ecFrom, vc, data);
                gArea.InsertEdgeAndData(data, ec, 0, true);

                HighlightBehaviour.SetHighlighted(_ecFrom, false);
                _ecFrom = null;
                _editorManager.DestroyVirtualEdge();
            }
        }

        void graphArea_VertexSelected(object sender, VertexSelectedEventArgs args)
        {

            if (args.MouseArgs.RightButton == MouseButtonState.Pressed)
            {
                ContextMenu contextMenu = new ContextMenu();
                args.VertexControl.ContextMenu = contextMenu;
                var mi = new MenuItem();
                mi.Header = "Добавить полюс";
                mi.Click += (s, e) =>
                {
                    // Этот кал бы вынести в xml и использовать здесь, задавая только итемы
                    Popup popup = new Popup();
                    StackPanel panel = new StackPanel();
                    TextBlock text = new TextBlock();
                    text.Text = "Имя полюса: ";
                    text.Background = new SolidColorBrush(Colors.White);
                    TextBox textBox = new TextBox();
                    textBox.Width = 150;
                    Button btn = new Button();
                    btn.Content = "Ok";
                    btn.Click += (ss, ee) =>
                    {
                        popup.IsOpen = false;
                        CoreName name = (args.VertexControl.Vertex as DataVertex).NodeName;
                        //OnAddPolusToNode(new PolusEventArgs(new CoreName(textBox.Text), name));
                    };
                    panel.Orientation = Orientation.Horizontal;
                    panel.Margin = new Thickness(5);
                    panel.Background = new SolidColorBrush(Colors.BlueViolet);

                    panel.Children.Add(text);
                    panel.Children.Add(textBox);
                    panel.Children.Add(btn);
                    popup.Child = panel;
                    popup.PlacementTarget = args.VertexControl;
                    popup.StaysOpen = false;
                    popup.IsOpen = true;
                };
                contextMenu.Items.Add(mi);
                contextMenu.IsOpen = true;
            }

            if (args.MouseArgs.LeftButton == MouseButtonState.Pressed)
            {
                OnVertexSeleacted(new VertexEventArgs((args.VertexControl.Vertex as DataVertex).NodeName));
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

        private void btnAddVertex_Click(object sender, RoutedEventArgs e)
        {
            OnAddVertex(new VertexEventArgs());
        }

        public void SetNodeTypes(IEnumerable<RoutineViewItem> items)
        {
            vertexView.ItemsSource = items;
            vertexView.DisplayMemberPath = "Name";
        }

        public void ShowError(string error)
        {
            txtError.Text += error + "\n";
        }

        public void ShowResults(List<ProcedureResult> results)
        {
            SimilationResultView view = new SimilationResultView();
            SimulationResultPresenter presenter = new SimulationResultPresenter(view, results);
            view.Show();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            OnRunSimulate(new SimulationEventArgs(double.Parse(txtTime.Text)));
        }

        private void checkEdgeLabels_Checked(object sender, RoutedEventArgs e)
        {
            if(checkEdgeLabels.IsChecked == true)
            {
                gArea.ShowAllEdgesLabels();
            }
        }

        private void checkEdgeLabels_Unchecked(object sender, RoutedEventArgs e)
        {
            // TODO: Завести в презентере переменную отвечающую за это
            // и при добавлении рубер не генерить лейбл
            gArea.ShowAllEdgesLabels(false);
        }

        private void checkVertexLabels_Checked(object sender, RoutedEventArgs e)
        {
            gArea.ShowAllVerticesLabels();
        }

        private void checkVertexLabels_Unchecked(object sender, RoutedEventArgs e)
        {
            gArea.ShowAllVerticesLabels(false);
        }

        private void btnCreateModel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveModel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnGraphConst_Click(object sender, RoutedEventArgs e)
        {
            OnGenerateGraph();
        }

        private void btnRandomGraph_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
