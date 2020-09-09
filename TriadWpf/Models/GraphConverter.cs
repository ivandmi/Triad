using System.Linq;
using TriadCore;
using TriadWpf.View.GraphXModels;

namespace TriadWpf.Models
{
    public class GraphConverter
    {
        public GraphExample GetGraph(Graph graph)
        {
            GraphExample res = new GraphExample();
            foreach (var node in graph.Nodes)
            {
                DataVertex data = new DataVertex(node.Name);
                res.AddVertex(data);
            }

            foreach (var node in graph.Nodes)
            {
                var vertex = res.Vertices.First(x => x.NodeName.Equals(node.Name));
                var nodesIn = StandartFunctions.GetAdjacentNodesIn(node);
                foreach (var edge in nodesIn)
                {
                    var nodeIn = edge as Node;
                    var vertexIn = res.Vertices.First(x => x.NodeName.Equals(nodeIn.Name));
                    var e = new DataEdge(vertexIn, vertex);
                    res.AddEdge(e);
                }

                var nodesOut = StandartFunctions.GetAdjacentNodesOut(node);
                foreach (var edge in nodesOut)
                {
                    var nodeOut = edge as Node;
                    var vertexOut = res.Vertices.First(x => x.NodeName.Equals(nodeOut.Name));
                    var e = new DataEdge(vertex, vertexOut);
                    res.AddEdge(e);
                }
            }

            return res;
        }
    }
}
