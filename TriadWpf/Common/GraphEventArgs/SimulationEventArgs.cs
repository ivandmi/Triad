using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriadWpf.Common.GraphEventArgs
{
    // Может убрать стоит
    public class SimulationEventArgs : EventArgs
    {
        public double TermianateTime { get; }
        
        public SimulationEventArgs(double time)
        {
            TermianateTime = time;
        }
    }
}
