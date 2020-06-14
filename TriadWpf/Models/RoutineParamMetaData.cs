using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriadWpf.Models
{
    public class RoutineParamMetaData
    {
        public string Name { get; }

        public Type Type { get; }

        public RoutineParamMetaData(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }

    public class RoutineParameter
    {
        public RoutineParamMetaData MetaData { get; }

        public object Value { get; set; }

        public RoutineParameter(RoutineParamMetaData data, object value)
        {
            MetaData = data;
            Value = value;
        }
    }
}
