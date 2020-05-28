using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TriadCore;

namespace TriadNSim
{
    public class NSStructure : GraphBuilder 
    {
        private SimulationInfo simInfo;

        public NSStructure(SimulationInfo simInfo)
        {
            this.simInfo = simInfo;
        }

        public override Graph Build()
        {
            Boolean first = true;
            Graph network = new Graph();
            
            // цикл по всем вершинам
            foreach (NetworkObject node in simInfo.Nodes)
            {
                this.PushEmptyGraph();
                //костыль связанный с именем вершины, оно может быть индексированным
                if (node.Name.Contains("["))
                {
                    string[] m = node.Name.Split('[',']');
                    this.FirstInStackGraph.DeclareNode(new CoreName(m[0], int.Parse(m[1])));
                }
                else
                    this.FirstInStackGraph.DeclareNode(new CoreName(node.Name));
                //
                foreach (Polus pole in node.Routine.Poluses)
                {
                    //???
                    if (pole.Name.Contains("["))
                    {
                        Polus p = new Polus(pole.Name);
                        this.FirstInStackGraph.DeclarePolusInAllNodes(new CoreName(p.Name, p.UpperBounds.ToArray()));
                    }
                    else
                    //
                    this.FirstInStackGraph.DeclarePolusInAllNodes(new CoreName(pole.Name));
                }

                if (!first)
                {
                    this.SecondInStackGraph.Add(this.FirstInStackGraph);
                    this.PopGraph();
                }
                else
                    first = false;
            }

            foreach (Link link in simInfo.Links)
            {
                this.PushEmptyGraph();

                NetworkObject objFrom = link.FromCP.Owner as NetworkObject;
                NetworkObject objTo = link.ToCP.Owner as NetworkObject;

                CoreName from, to;
                if (link.PolusFrom.Contains("["))
                {
                    Polus p = new Polus(link.PolusFrom);
                    from = new CoreName(p.Name, p.UpperBounds.ToArray());
                }
                else
                    from = new CoreName(link.PolusFrom);
                if (link.PolusTo.Contains("["))
                {
                    Polus p = new Polus(link.PolusTo);
                    to = new CoreName(p.Name, p.UpperBounds.ToArray());
                }
                else
                    to = new CoreName(link.PolusFrom);
                //?????????????

                CoreName corefrom = new CoreName(objFrom.Name);
                CoreName coreto= new CoreName(objTo.Name);
                // добавляем полюса вершин
                if (objFrom.Name.Contains("["))
                {
                    string[] m = objFrom.Name.Split('[', ']');
                    corefrom = new CoreName(m[0], int.Parse(m[1]));  
                }
                this.FirstInStackGraph.DeclareNode(corefrom, from);

                if (objTo.Name.Contains("["))
                {
                    string[] m = objTo.Name.Split('[', ']');
                    coreto = new CoreName(m[0], int.Parse(m[1]));  
                }
                this.FirstInStackGraph.DeclareNode(coreto, to);

                this.FirstInStackGraph.AddEdge(corefrom, from, coreto, to);

                this.SecondInStackGraph.Add(this.FirstInStackGraph);
                this.PopGraph();
            }

            network = this.PopGraph();
            return network;
        }
    }
}
