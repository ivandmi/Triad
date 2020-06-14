using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadCore;
using TriadWpf.Common.Interfaces;
using TriadWpf.Models;

namespace TriadWpf.Common.GraphEventArgs
{
    public struct NodeParam
    {
        public CoreName NodeName { get; }
        public CoreName ParamName { get; }

        public NodeParam(string nodeName, string paramName)
        {
            NodeName = new CoreName(nodeName);
            ParamName = new CoreName(paramName);
        }
    }
    public class ProcedureEventArgs : EventArgs
    {
        // TODO: передавать не блюпринт, а поля блюпринта
        public ProcedureBlueprint ProcedureBlueprint { get; }

        public ProcedureEventArgs() { }

        public ProcedureEventArgs(ProcedureBlueprint blueprint)
        {
            ProcedureBlueprint = blueprint;
        }
    }
}
