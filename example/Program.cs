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
            CoreName s = new CoreName("S");
            g.DeclareNode(s);
            
            CoreName i = new CoreName("I");
            g.DeclareNode(i);

            CoreName r = new CoreName("R");
            g.DeclareNode(r);

            CoreName sToi = new CoreName("s->i");
            g.DeclareNode(sToi);

            CoreName sTor = new CoreName("s->r");
            g.DeclareNode(sTor);

            CoreName iTor = new CoreName("i->r");
            g.DeclareNode(iTor);

            CoreName input0 = new CoreName("Input", 0);
            CoreName output0 = new CoreName("Output", 0);
            g.DeclarePolusInAllNodes(input0);
            g.DeclarePolusInAllNodes(output0);

            CoreName output1 = new CoreName("Output", 1);
            g[s].DeclarePolus(output1);

            g.AddEdge(s, output0, sToi, input0);
            g.AddEdge(s, output1, sTor, input0);

            g.AddEdge(sToi, output0, i, input0);

            g.AddEdge(iTor, input0, i, output0);
            g.AddEdge(iTor, output0, r, input0);

            g.AddEdge(sTor, output0, r, input0);

            

            g[s].NodeRoutine = new PetriNetPlace(100, 1);
            g[s].NodeRoutine.AddPolusPair(new CoreName("Output", 0), new CoreName("Output", 0));
            g[s].NodeRoutine.AddPolusPair(new CoreName("Output", 1), new CoreName("Output", 1));

            g[i].NodeRoutine = new PetriNetPlace(0, 1);
            g[i].NodeRoutine.AddPolusPair(new CoreName("Input", 0), new CoreName("Input", 0));
            g[i].NodeRoutine.AddPolusPair(new CoreName("Output", 0), new CoreName("Output", 0));

            g[r].NodeRoutine = new PetriNetPlace(0, 1);
            g[r].NodeRoutine.AddPolusPair(new CoreName("Input", 0), new CoreName("Input", 0));

            g[sToi].NodeRoutine = new PetriNetPropabilityTransition(1, 0.7);
            g[sToi].NodeRoutine.AddPolusPair(new CoreName("Input", 0), new CoreName("Input", 0));
            g[sToi].NodeRoutine.AddPolusPair(new CoreName("Output", 0), new CoreName("Output", 0));

            g[iTor].NodeRoutine = new PetriNetPropabilityTransition(1, 0.2);
            g[iTor].NodeRoutine.AddPolusPair(new CoreName("Input", 0), new CoreName("Input", 0));
            g[iTor].NodeRoutine.AddPolusPair(new CoreName("Output", 0), new CoreName("Output", 0));

            g[sTor].NodeRoutine = new PetriNetPropabilityTransition(1, 0.2);
            g[sTor].NodeRoutine.AddPolusPair(new CoreName("Input", 0), new CoreName("Input", 0));
            g[sTor].NodeRoutine.AddPolusPair(new CoreName("Output", 0), new CoreName("Output", 0));

            g.DoSimulate(300);
        }

        static void get(out double[] arr)
        {
            arr = new double[5];
        }
    }
}
