using System;
using TriadCore;

namespace TriadWpf.GraphEventArgs
{
    public class PolusEventArgs:EventArgs
    {
        public string PolusName { get; private set; }
        public string NodeName { get; private set; }
        public PolusEventArgs(string polusName, string nodeName)
        {
            PolusName = polusName;
            NodeName = nodeName;
        }
    }
}
