﻿using TriadCore;
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
using TriadWpf.Models;
using TriadWpf.Common.GraphEventArgs;

namespace TriadWpf.Presenter
{
    public class AppPresenter
    {
        IMainView mainView;
        RoutinesRepository routinesRepository;

        ProcedureRepository procedureRepository;

        /// <summary>
        /// Модель
        /// </summary>
        Graph graph;
        private Dictionary<string ,ICondition> conditions;

        private CommonCondition commonCondition;

        public AppPresenter(IMainView view)
        {
            mainView = view;
            graph = new Graph();
            routinesRepository = new RoutinesRepository();
            procedureRepository = new ProcedureRepository();
            conditions = new Dictionary<string, ICondition>();
            commonCondition = new CommonCondition();

            // События, связанные с отображением графа
            mainView.AddVertex += MainView_AddVertex;
            mainView.RemoveVertex += MainView_RemoveVertex;

            mainView.AddEdge += MainView_AddEdge;
            mainView.RemoveEdge += MainView_RemoveEdge;

            mainView.AddPolusToNode += MainView_AddPolusToNode;

            // Задаем типы рутин и процедур
            mainView.SetNodeTypes(routinesRepository.RoutineMetadata);
            mainView.ProcedureView.SetProceduresTypes(procedureRepository.Procedures);

            // События, связанные с процедурами
            mainView.ProcedureView.CreateProcedureBlueprint += ProcedureView_CreateProcedureBlueprint;
            mainView.ProcedureView.SaveProcedure += ProcedureView_SaveProcedure;

            //Событие запуска симуляции
            mainView.RunSimulation += MainView_RunSimulation;
        }

        private void MainView_RunSimulation(object sender, SimulationEventArgs e)
        {
            SimulationService service = new SimulationService();
            List<ProcedureResult> list = service.Simulate(graph, commonCondition, conditions);

            mainView.ShowResults(list);
        }

        private void ProcedureView_SaveProcedure(object sender, ProcedureEventArgs e)
        {
            IProcedure procedure = e.ProcedureBlueprint.Metadata.CreateProcedure();
            ProcedureBuilder builder = new ProcedureBuilder();
            var dict = CreateProcedureParamDict(e.ProcedureBlueprint.ModelParamByProcParam);
            try
            {
                builder.AddParamsToProcudure(graph, procedure, dict);
            }
            catch (Exception err)
            {
                mainView.ShowError(err.Message);
            }
            commonCondition.AddProcedure(e.ProcedureBlueprint.Name ,procedure);
        }

        private Dictionary<ParamMetadata, NodeParam> CreateProcedureParamDict(Dictionary<ParamMetadata, string> dict)
        {
            Dictionary<ParamMetadata, NodeParam> res = new Dictionary<ParamMetadata, NodeParam>();
            foreach(var pair in dict)
            {
                var splits = pair.Value.Split('.');
                NodeParam nodeParam = new NodeParam(splits[0], splits[1]);
                res.Add(pair.Key, nodeParam);
            }
            return res;
        }

        private void ProcedureView_CreateProcedureBlueprint(object sender, Common.GraphEventArgs.ProcedureEventArgs e)
        {
            ProcedureBlueprint blueprint = new ProcedureBlueprint();
            blueprint.Name = "Новая процедура " + commonCondition.ProceduresCount.ToString();
            blueprint.Metadata = new ProcedureCount();
            mainView.ProcedureView.AddProcedureBlueprint(blueprint);
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
