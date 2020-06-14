using System.Collections.Generic;
using TriadWpf.Common.Interfaces;
using TriadWpf.Models;

namespace TriadWpf.Common
{
    class ProcedureRepository : IProcedureRepository
    {
        public IEnumerable<IProcedureMetadata> Procedures { get; }

        public ProcedureRepository()
        {
            Procedures = new List<IProcedureMetadata>()
            {
                new ProcedureCount()
            };
        }
    }
}
