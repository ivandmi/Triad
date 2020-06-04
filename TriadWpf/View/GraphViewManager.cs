using GraphX.Controls;
using System.Linq;
using TriadCore;
using TriadWpf.Interfaces;
using TriadWpf.View.GraphXModels;

namespace TriadWpf.View
{
    class GraphViewManager : IGraphViewManager
    {
        GraphAreaControl graphArea;
        public GraphViewManager(GraphAreaControl control)
        {
            graphArea = control;
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
            graphArea.VertexList.FirstOrDefault(x => x.Key.NodeName == nodeName).Key.Poluses.Add(polusName);
        }

        public void AddVertex(CoreName node)
        {
            var vd = new DataVertex(node);
            var vc = new VertexControl(vd);
            graphArea.AddVertexAndData(vd, vc, true);
            graphArea.RelayoutGraph();
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
