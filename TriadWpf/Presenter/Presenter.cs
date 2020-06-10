using TriadCore;
using TriadWpf.Interfaces;
using TriadWpf.GraphEventArgs;
using TriadWpf.Common;
using GraphX.Common;
using System.Windows;
using System;
using System.Collections;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Linq;

namespace TriadWpf.Presenter
{
    public class AppPresenter
    {
        IMainView mainView;
        RoutinesRepository routinesRepository;

        /// <summary>
        /// Модель
        /// </summary>
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
            var routine = graph[e.NodeName].NodeRoutine;
            if (routine != null)
            {
                // Пока реализуем связь один к одному
                routine.AddPolusPair(e.PolusName, e.PolusName);
            }
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
            CoreName name;

            if (e.Name != null)
                name = e.Name;
            else
                name = new CoreName("Вершина "+(graph.NodeCount+1).ToString());

            if (e.Point != null)
                mainView.GraphViewManager.AddVertex(name, e.Point);
            else
                mainView.GraphViewManager.AddVertex(name); 
            
            if (e.RoutineViewItem.Type != Common.Enums.RoutineType.Undefined)
            {
                Node node = new Node(name);
                node.RegisterRoutine(routinesRepository.GetRoutine(e.RoutineViewItem.Type));
                //
                //Магия с полюсами
                //

                // Если добавлять с тем же именем, что уже есть в графе, то он сольет полюса просто в одну вершину
                graph.Add(node);
            }
            else
                graph.DeclareNode(name);
        }
    }
}
