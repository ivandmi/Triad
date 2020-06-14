using System.Collections.Generic;
using TriadCore;
using TriadWpf.Common.Interfaces;

namespace TriadWpf.Models
{
    class ProcedureCount : IProcedureMetadata
    {
        public string Name => "Счетчик";

        public string Description => "Подсчет количества срабатываний полюса";

        public IEnumerable<IPParamMetadata> Params { get; }

        public IProcedure CreateProcedure()
        {
            return new IPCount();
        }

        public ProcedureCount()
        {
            Params = new List<IPParamMetadata>() {
                new IPParamMetadata("Полюс", "Полюс", "Arg", SpyObjectType.Polus)
            };

        }
    }
}
