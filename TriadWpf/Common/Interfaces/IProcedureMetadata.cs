using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadCore;
using TriadWpf.Models;

namespace TriadWpf.Common.Interfaces
{
    /// <summary>
    /// Информация о информационной процедуре
    /// </summary>
    public interface IProcedureMetadata
    {
        IProcedure CreateProcedure();
        string Name { get; }
        string Description { get; }
        IEnumerable<IPParamMetadata> Params { get; }
    }
}
