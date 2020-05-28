using System;
using System.Collections.Generic;
using System.Text;

namespace TriadCore
{
    public abstract class Tree:Graph
    {
        int index;
        int n;
        int m;

        protected Tree(int N, int M)
            :base(new CoreName("Неизвестное дерево"))
        {
            n = N;
            m = M;
        }

        protected Tree(CoreName name, int N, int M)
            :base(name)
        {
            n = N;
            m = M;
        }

        public override void CompleteGraph()
        {
            index = 0;
            Rec(0, 0);
        }

        private void Rec(int level,int i)
        {
            int k=0;
            if (level >= m)
                return;
            while(k<n)
            {
                index++;
                AddConnection(this[i].Name, this[i][k].Name, this[index].Name, this[index][n].Name);
                k++;
                Rec(level+1,index);
            }
        }

        /// <summary>
        /// Установить "нужное" соединение
        /// </summary>
        /// <param name="nodeName1">Имя первой вершины</param>
        /// <param name="polusName1">Имя первого полюса</param>
        /// <param name="nodeName2">Имя второй вершины</param>
        /// <param name="polusName2">Имя второго полюса</param>
        protected virtual void AddConnection(CoreName nodeName1, CoreName polusName1, CoreName nodeName2, CoreName polusName2)
        {
            return;
        }
    }

    public class UndirectedTree:Tree
    {
        public UndirectedTree(int N,int M)
            :base(N,M)
        { }

        public UndirectedTree(CoreName name, int N, int M)
            :base(name,N,M)
        { }

        protected override void AddConnection(CoreName nodeName1, CoreName polusName1, CoreName nodeName2, CoreName polusName2)
        {
            AddEdge(nodeName1, polusName1, nodeName2, polusName2);
        }
    }
}
