using System;
using System.Collections.Generic;
using System.Text;

namespace TriadCore
{
    public class ComplGraph:Graph
    {
        public ComplGraph()
            :base(new CoreName("Неизвестный полный граф"))
        { }

        public ComplGraph(CoreName name)
            :base(name)
        { }

        public override void CompleteGraph()
        {
            for(int i=0;i<NodeCount-1;i++)
                for(int j=i+1;j<NodeCount;j++)
                    AddEdge(this[i].Name, this[i][j].Name, this[j].Name, this[j][i].Name);
        }
    }
}
