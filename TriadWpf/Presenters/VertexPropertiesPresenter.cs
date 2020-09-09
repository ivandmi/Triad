using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Documents;
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
            view.ShowPoluses(node.Poluses.Select(x => x.Name).ToArray());
        }

        public VertexPropertiesPresenter(IVertexPropertiesView view, IGraphViewManager graphView, Graph graph)
        {
            this.view = view;
            this.graphView = graphView;
            this.graph = graph;

            service = new RoutineDataService();

            this.view.ChangeNodeName += View_ChangeName;
            this.view.UpdateRoutineParam += View_UpdateRoutineParam;
            this.view.AddPolus += View_AddPolus;

            this.view.ChangePolusName += View_ChangePolusName;
            this.view.RemovePolus += View_RemovePolus;
        }

        private void View_RemovePolus(object sender, GraphEventArgs.PolusEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void View_ChangePolusName(object sender, Common.GraphEventArgs.ChangeNameArgs e)
        {
            throw new NotImplementedException();
        }

        private void View_AddPolus(object sender, GraphEventArgs.PolusEventArgs e)
        {
            CoreName nodeName = ParseCoreName(e.NodeName);
            CoreName polusName = ParseCoreName(e.PolusName);

            if (polusName != null)
            {
                node.DeclarePolus(polusName);
                if (node.NodeRoutine != null)
                {
                    node.NodeRoutine.AddPolusPair(new CoreName(polusName.BaseName, polusName.IndexArray), polusName);
                }

                // Потом может сделать, чтоб со строками работали вьюшки, а не с CoreName
                view.ShowPoluses(node.Poluses.Select(x => x.Name).ToArray());
                graphView.AddPolusToVertex(node.Name, polusName);
            }
        }

        private void View_UpdateRoutineParam(object sender, Common.GraphEventArgs.UpdateParamValueArgs e)
        {
            // TODO: try catch ввывод ошибки
            string value = e.Value.ToString();
            if (value.Contains('.'))
            {
                value = value.Replace('.', ',');
            }
            var convertValue = Convert.ChangeType(value, e.MetaData.Type);
            node.NodeRoutine.SetValueForVar(new CoreName(e.MetaData.Name), convertValue);
        }

        private void View_ChangeName(object sender, Common.GraphEventArgs.ChangeNameArgs e)
        {
            CoreName name = ParseCoreName(e.NewName);
            if(name!= null)
            {
                graphView.ChangeVertexName(node.Name, name);
                node = graph.RenameNode(node.Name, name);
            }
        }

        private CoreName ParseCoreName(string name)
        {
            //[\d\w]+\[\d+(\,\d+)*\]
            Regex indexedName = new Regex("[А-Яа-яA-Za-z0-9]+\\[\\d+(\\,\\d+)*\\]");
            Regex simpleName = new Regex("[А-Яа-яA-Za-z0-9 ]+");
            if (indexedName.IsMatch(name) || simpleName.IsMatch(name))
            {
                var splits = name.Split('[', ']', ',');
                var baseName = splits[0];
                List<int> indexes = new List<int>();
                for(int i = 1; i<splits.Length;i++)
                {
                    if (splits[i] != "")
                    {
                        var part = int.Parse(splits[i]);
                        indexes.Add(part);
                    } 
                }
                return new CoreName(baseName, indexes.ToArray());
            }
            else
            {
                view.BadName(String.Format("Недопустимое имя \"{0}\".", name));
                return null;
            }
        }
    }
}
