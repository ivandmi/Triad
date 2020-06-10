using System.Collections.Generic;
using TriadWpf.Models;

namespace TriadWpf.Common.Interfaces
{
    interface IProcedureRepository
    {
        IEnumerable<ProcedureMetaData> Procedures { get; }
        ProcedureMetaData GetProcedure(string name);
    }
}
