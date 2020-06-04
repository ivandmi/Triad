using System;
using TriadCore;

namespace TriadWpf.GraphEventArgs
{
    public class PolusEventArgs:EventArgs
    {
        public CoreName PolusName { get; private set; }
        public CoreName NodeName { get; private set; }
        public PolusEventArgs(CoreName polusName, CoreName nodeName)
        {
            PolusName = polusName;
            NodeName = nodeName;
        }
    }
}
