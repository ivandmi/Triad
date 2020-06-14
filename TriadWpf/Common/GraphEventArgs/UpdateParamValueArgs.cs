using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadWpf.Models;

namespace TriadWpf.Common.GraphEventArgs
{
    public class UpdateParamValueArgs : EventArgs
    {
        public RoutineParamMetaData MetaData { get; }
        public object Value { get; }

        public UpdateParamValueArgs(RoutineParamMetaData data, object value)
        {
            MetaData = data;
            Value = value;
        }
    }
}
