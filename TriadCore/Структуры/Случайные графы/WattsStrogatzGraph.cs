using System;
using System.Collections.Generic;
using System.Text;

namespace TriadCore
{
    public class WattsStrogatzGraph:Graph
    {
        int K;
        double p;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="k">Количество связей. Должно быть четным и больше log(N)</param>
        /// <param name="p">Вероятность случайной вершины</param>
        public WattsStrogatzGraph(int k,double p)
            :base(new CoreName("Неизвестный случайный граф"))
        {
            K = k;
            this.p = p;
        }

        public WattsStrogatzGraph(CoreName name, int k,double p)
            :base(name)
        {
            K = k;
            this.p = p;
        }

        public override void CompleteGraph()
        {
            for(int i=0;i<NodeCount;i++)
            {
                for (int j = 1; j <= K/2; j++)
                {
                    if (Rand.RandomReal() < p)
                    {
                        int to;
                        do
                        {
                            to = Rand.RandomIn(0, NodeCount-1);
                        }
                        while (to == i); //в идеале, чтобы еще не было такого ребра у вершины
                        AddEdge(this[i].Name, this[i][to].Name, this[to].Name, this[to][i].Name);
                    }
                    else
                    {
                        int ind = (i + j)% (NodeCount);
                        AddEdge(this[i].Name, this[i][ind].Name, this[ind].Name, this[ind][i].Name);
                    }
                }
            }
        }
    }
}
