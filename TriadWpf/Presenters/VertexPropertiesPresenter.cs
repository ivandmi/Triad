using System;
using TriadCore;
using TriadWpf.Common.Interfaces;
using TriadWpf.Interfaces;
using TriadWpf.Models;
using TriadWpf.View.GraphXModels;

namespace TriadWpf.Presenters
{
    class VertexPropertiesPresenter
    {
        Node node;
        Graph graph;
        RoutineDataService service;

        IGraphViewManager graphView;

        IVertexPropertiesView view;
        public void SetVertex(CoreName name)
        {
            this.node = graph[name];

            // хз хз Долго ли рефлексия выполняется или нет. Хз хз
            view.ShowParamsAndVariables(service.GetRoutineParameters(node.NodeRoutine), service.GetRoutineVariables(node.NodeRoutine));
            view.SetVertexName(name.ToString());
        }

        public VertexPropertiesPresenter(IVertexPropertiesView view, IGraphViewManager graphView, Graph graph)
        {
            this.view = view;
            this.graphView = graphView;
            this.graph = graph;

            service = new RoutineDataService();

            this.view.ChangeName += View_ChangeName;
            this.view.UpdateRoutineParam += View_UpdateRoutineParam;
        }

        private void View_UpdateRoutineParam(object sender, Common.GraphEventArgs.UpdateParamValueArgs e)
        {
            // TODO: try catch ввывод ошибки
            var value = Convert.ChangeType(e.Value, e.MetaData.Type);
            node.NodeRoutine.SetValueForVar(new CoreName(e.MetaData.Name), value);
        }

        private void View_ChangeName(object sender, Common.GraphEventArgs.ChangeNameArgs e)
        {
            graphView.ChangeVertexName(node.Name, new CoreName(e.NewName));
            graph.RenameNode(node.Name, new CoreName(e.NewName));
        }
    }
}
