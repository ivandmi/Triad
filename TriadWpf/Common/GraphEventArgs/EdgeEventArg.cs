using System;
using TriadCore;

namespace TriadWpf.GraphEventArgs
{
    public class EdgeEventArg : EventArgs
    {
        public CoreName NodeFrom { get; }
        public CoreName NodeTo { get; }
        public CoreName PolusTo { get; }
        public CoreName PolusFrom { get; }

        public EdgeEventArg(CoreName nodeFrom, CoreName polusFrom, CoreName nodeTo, CoreName polusTo)
        {
            PolusFrom = polusFrom;
            NodeFrom = nodeFrom;
            PolusTo = polusTo;
            NodeTo = nodeTo;
        }
    }
}
