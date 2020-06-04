using System;
using TriadCore;

namespace TriadWpf.GraphEventArgs
{
    public class VertexEventArgs : EventArgs
    {
        public CoreName Name { get; }
        public VertexEventArgs(CoreName name)
        {
            Name = name;
        }
    }
}
