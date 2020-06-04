using TriadCore;
using TriadWpf.Interfaces;
using TriadWpf.GraphEventArgs;
using TriadWpf.Common;

namespace TriadWpf.Presenter
{
    public class AppPresenter
    {
        IGraphViewManager graphViewManager;
        IMainView mainView;
        RoutinesRepository routinesRepository;
        Graph graph;
        public AppPresenter(IMainView view)
        {
            mainView = view;
            graph = new Graph();
            routinesRepository = new RoutinesRepository();

            mainView.AddVertex += MainView_AddVertex;
            mainView.RemoveVertex += MainView_RemoveVertex;

            mainView.AddEdge += MainView_AddEdge;
            mainView.RemoveEdge += MainView_RemoveEdge;

            mainView.AddPolusToNode += MainView_AddPolusToNode;

            mainView.SetNodeTypes(routinesRepository.RoutineMetadata);
        }

        private void MainView_AddPolusToNode(object sender, PolusEventArgs e)
        {
            graph[e.NodeName].DeclarePolus(e.PolusName);
            mainView.GraphViewManager.AddPolusToVertex(e.NodeName, e.PolusName);
        }

        private void MainView_RemoveEdge(object sender, EdgeEventArg e)
        {
            throw new System.NotImplementedException();
        }

        private void MainView_AddEdge(object sender, EdgeEventArg e)
        {
            graph.AddArc(e.NodeFrom, e.PolusFrom, e.NodeTo, e.PolusTo);
        }

        private void MainView_RemoveVertex(object sender, VertexEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void MainView_AddVertex(object sender, VertexEventArgs e)
        {
            graph.DeclareNode(e.Name);
            mainView.GraphViewManager.AddVertex(e.Name);
        }
    }
}
