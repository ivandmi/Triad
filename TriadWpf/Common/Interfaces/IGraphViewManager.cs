using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadCore;

namespace TriadWpf.Interfaces
{
    public interface IGraphViewManager
    {
        void AddVertex(CoreName node);
        void RemoveVertex(CoreName node);
        void AddEdge(CoreName from, CoreName to);
        void RemoveEdge(CoreName from, CoreName to);
        void AddPolusToVertex(CoreName nodeName, CoreName polusName);
    }
}
