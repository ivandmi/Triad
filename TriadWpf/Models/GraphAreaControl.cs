using GraphX.Controls;
using QuickGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TriadWpf.ViewModels;

namespace TriadWpf.GraphXModels
{
    public class GraphAreaControl : GraphArea<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>>
    {
        public GraphAreaControl() : base()
        {
            this.Unloaded += GraphAreaControl_Unloaded;
        }

        /// <summary>
        /// Source of realisation ItemSource - https://stackoverflow.com/questions/9460034/custom-itemssource-property-for-a-usercontrol
        /// </summary>
        public IEnumerable VertexListSource
        {
            get => (IEnumerable)GetValue(VertexListSourceProperty);
            set => SetValue(VertexListSourceProperty, value);
        }

        public static readonly DependencyProperty VertexListSourceProperty =
            DependencyProperty.Register("VertexListSource", typeof(IEnumerable), typeof(GraphAreaControl), new PropertyMetadata(null, (s, e) =>
            {
                if (s is GraphAreaControl uc)
                {
                    if (e.OldValue is INotifyCollectionChanged oldValueINotifyCollectionChanged)
                    {
                        oldValueINotifyCollectionChanged.CollectionChanged -= uc.VertexListSource_CollectionChanged;
                    }

                    if (e.NewValue is INotifyCollectionChanged newValueINotifyCollectionChanged)
                    {
                        newValueINotifyCollectionChanged.CollectionChanged += uc.VertexListSource_CollectionChanged;
                    }
                }
            }));

        private void VertexListSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var v in e.NewItems)
                    {
                        var vc = new VertexControl(v);
                        this.AddVertexAndData((DataVertex)v, vc, true);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var v in e.OldItems)
                    {
                        var vc = new VertexControl(v);
                        this.RemoveVertexAndEdges((DataVertex)v);
                    }
                    break;
            }
            this.RelayoutGraph();
        }

        public IEnumerable EdgeListSource
        {
            get => (IEnumerable)GetValue(EdgeListSourceProperty);
            set => SetValue(EdgeListSourceProperty, value);
        }

        public static readonly DependencyProperty EdgeListSourceProperty =
            DependencyProperty.Register("EdgeListSource", typeof(IEnumerable), typeof(GraphAreaControl), new PropertyMetadata(null, (s, e) =>
            {
                if (s is GraphAreaControl uc)
                {
                    if (e.OldValue is INotifyCollectionChanged oldValueINotifyCollectionChanged)
                    {
                        oldValueINotifyCollectionChanged.CollectionChanged -= uc.EdgeListSource_CollectionChanged;
                    }

                    if (e.NewValue is INotifyCollectionChanged newValueINotifyCollectionChanged)
                    {
                        newValueINotifyCollectionChanged.CollectionChanged += uc.EdgeListSource_CollectionChanged;
                    }
                }
            }));

        private void EdgeListSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach(var edge in e.NewItems)
                    {
                        var sourceControl = this.VertexList.FirstOrDefault(x => x.Key == ((DataEdge)edge).Source).Value;
                        var targetControl = this.VertexList.FirstOrDefault(x => x.Key == ((DataEdge)edge).Target).Value;
                        var ec = new EdgeControl(sourceControl, targetControl, edge);
                        this.AddEdgeAndData((DataEdge)edge, ec, true);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach(var edge in e.OldItems)
                    {
                        this.RemoveEdge((DataEdge)edge, true);
                    }
                    break;
            }
        }

        /// НЕ ЗАБУДЬ ДОДЕЛАТЬ ЭТО
        /// 

        //to do Do Not Forget To Remove Event On UserControl Unloaded
        private void GraphAreaControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (VertexListSource is INotifyCollectionChanged Vincc)
            {
                Vincc.CollectionChanged -= VertexListSource_CollectionChanged;
            }

            if (EdgeListSource is INotifyCollectionChanged Eincc)
            {
                Eincc.CollectionChanged -= EdgeListSource_CollectionChanged;
            }
        }

    }
}
