using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Logic.Algorithms.LayoutAlgorithms;
using GraphX.Logic.Algorithms.OverlapRemoval;
using GraphX.Logic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TriadCore;
using TriadWpf.GraphXModels;

namespace TriadWpf.ViewModels
{
    public class ViewModel : ViewModelBase
    {
        private Graph graph;

        public ObservableCollection<DataVertex> VertexList { get; private set; }

        public Graph Graph
        {
            get => graph;
            set
            {
                graph = value;
            }
        }

        public ViewModel()
        {
            graph = new Graph();
            AddVertexCommand = new RelayCommand(AddVertex);
            ((RelayCommand)AddVertexCommand).IsEnabled = true;
            VertexList = new ObservableCollection<DataVertex>();
            
        }

        public ICommand AddVertexCommand { get; private set; }
        public ICommand RemoveVertexCommand { get; private set; }
        public ICommand AddEdgeCommand { get; private set; }
        public ICommand RemoveEdgeCommand { get; private set; }

        private void AddVertex()
        {
            string name = "Вершина " + (graph.NodeCount + 1).ToString();
            DataVertex dt = new DataVertex(new CoreName(name));
            graph.Add(dt.Node);
            VertexList.Add(dt);
            TextBox tx;
            tx.ToolTip 
        }

        private void AddEdge(DataVertex from, DataVertex to)
        {
            DataEdge de = new DataEdge(from, to);
        }
    }
}
