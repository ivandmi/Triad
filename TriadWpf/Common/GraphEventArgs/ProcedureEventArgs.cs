using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadCore;
using TriadWpf.Common.Interfaces;

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
        public IProcedureMetadata ProcedureMetadata { get; }

        /// <summary>
        /// Key - параметр процедуры, 
        /// </summary>
        public Dictionary<string, NodeParam> Params { get; }
        public string ProcedureName { get; }

        public ProcedureEventArgs(IProcedureMetadata procData, Dictionary<string, NodeParam> procParams, string name)
        {
            ProcedureMetadata = procData;
            Params = procParams;
            ProcedureName = name;
        }


    }
}
