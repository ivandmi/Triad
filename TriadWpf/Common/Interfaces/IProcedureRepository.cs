using System.Collections.Generic;

namespace TriadWpf.Common.Interfaces
{
    interface IProcedureRepository
    {
        IEnumerable<IProcedureMetadata> Procedures { get; }
    }
}
