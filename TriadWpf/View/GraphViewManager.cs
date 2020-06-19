using GraphX.Controls;
using System;
using System.Linq;
using System.Windows;
using TriadCore;
using TriadWpf.Common.Enums;
using TriadWpf.Interfaces;
using TriadWpf.View.GraphXModels;

namespace TriadWpf.View
{
    class GraphViewManager : IGraphViewManager
    {
        GraphAreaControl graphArea;

        ResourceDictionary resources;
        public GraphViewManager(GraphAreaControl control)
        {
            graphArea = control;
            resources = new ResourceDictionary()
            {
                Source = new System.Uri("/TriadWpf;component/View/VertexTemplates.xaml", System.UriKind.Relative)
            };
        }
        public void AddEdge(CoreName from, CoreName to)
        {
            var sourceControl = graphArea.VertexList.FirstOrDefault(x => x.Key.Poluses.Contains(from)).Value;
            var targetControl = graphArea.VertexList.FirstOrDefault(x => x.Key.Poluses.Contains(to)).Value;
            var edge = new DataEdge(
                sourceControl.Vertex as DataVertex, 
                targetControl.Vertex as DataVertex,
                from,
                to);
            var ec = new EdgeControl(sourceControl, targetControl, edge);
            graphArea.AddEdgeAndData(edge, ec, true);
        }

        public void AddPolusToVertex(CoreName nodeName, CoreName polusName)
        {
            graphArea.VertexList.FirstOrDefault(x => x.Key.NodeName.Equals(nodeName)).Key.Poluses.Add(polusName);
        }

        public void AddVertex(CoreName node)
        {
            var vd = new DataVertex(node);
            var vc = new VertexControl(vd);
            graphArea.AddVertexAndData(vd, vc, true);
            graphArea.RelayoutGraph();
        }

        public void AddVertex(CoreName node, Point point, RoutineType type)
        {
            var vd = new DataVertex(node);
            var vc = new VertexControl(vd);
            //Style style = (Style)resources["PetriNetPlace"];
            vc.Style = GetStyle(type);
            vc.SetPosition(point);
            graphArea.AddVertexAndData(vd, vc, true);
        }

        // Говно, которое надо переделать
        private Style GetStyle(RoutineType type)
        {
            switch (type)
            {
                case RoutineType.PetriNetPlace:
                    return (Style)resources["PetriNetPlace"];
                case RoutineType.PetriNetTransition:
                    return (Style)resources["PetriNetTransition"];
                default:
                    return (Style)resources["defaultVertex"];
            }
        }

        public void ChangeVertexName(CoreName oldName, CoreName newName)
        {
            var vertex = graphArea.VertexList.FirstOrDefault(x => x.Key.NodeName.Equals(oldName)).Key;
            vertex.NodeName = newName;
        }

        public void RemoveEdge(CoreName from, CoreName to)
        {
            var dataEdge = graphArea.EdgesList.FirstOrDefault(x => x.Key.PolusFrom == from && x.Key.PolusTo == to).Key;
            graphArea.RemoveEdge(dataEdge);
        }

        public void RemoveVertex(CoreName node)
        {
            var dataVertex = graphArea.VertexList.Keys.First(x => x.NodeName == node);
            graphArea.RemoveVertexAndEdges(dataVertex);
            graphArea.RelayoutGraph();
        }
    }
}
