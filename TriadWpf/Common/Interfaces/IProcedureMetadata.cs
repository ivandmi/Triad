using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadCore;

namespace TriadWpf.Common.Interfaces
{
    public interface IProcedureMetadata
    {
        IProcedure CreateProcedure();
        string Description { get; }
        IEnumerable<IParamMetadata> Params { get; }
    }
}
