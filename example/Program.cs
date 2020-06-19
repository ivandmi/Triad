using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadCore;
using TriadCore.Рутины.Базовые;

namespace example
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] m;
            get(out m);
            Graph g = new Graph();
            CoreName place1 = new CoreName("первое место");
            g.DeclareNode(place1);
            CoreName place2 = new CoreName("конечное место");
            g.DeclareNode(place2);
            CoreName trans = new CoreName("переход");
            g.DeclareNode(trans);
            CoreName input = new CoreName("Input", 0);
            CoreName output = new CoreName("Output", 0);
            g.DeclarePolusInAllNodes(new CoreName("Input", 0));
            g.DeclarePolusInAllNodes(new CoreName("Output", 0));

            g.AddEdge(place1, output, trans, input);
            g.AddEdge(trans, output, place2, input);

            g[place1].NodeRoutine = new PetriNetPlace(1, 1);
            g[place1].NodeRoutine.AddPolusPair(new CoreName("Output", 0), new CoreName("Output", 0));

            g[place2].NodeRoutine = new PetriNetPlace(1, 1);
            g[place2].NodeRoutine.AddPolusPair(new CoreName("Input", 0), new CoreName("Input", 0));

            g[trans].NodeRoutine = new PetriNetTransition(1);
            g[trans].NodeRoutine.AddPolusPair(new CoreName("Input", 0), new CoreName("Input", 0));
            g[trans].NodeRoutine.AddPolusPair(new CoreName("Output", 0), new CoreName("Output", 0));

            g.DoSimulate(10);
        }

        static void get(out double[] arr)
        {
            arr = new double[5];
        }
    }
}
