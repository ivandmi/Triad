using System.Collections.Generic;
using TriadCore;
using TriadWpf.Common.Interfaces;

namespace TriadWpf.Models
{
    class ProcedureCount : IProcedureMetadata
    {
        public string Name => "Счетчик";

        public string Description => "Подсчет количества срабатываний полюса";

        public IEnumerable<ParamMetadata> Params { get; }

        public IProcedure CreateProcedure()
        {
            return new IPCount();
        }

        public ProcedureCount()
        {
            Params = new List<ParamMetadata>() {
                new ParamMetadata("Полюс", "Полюс", "Arg", SpyObjectType.Polus)
            };

        }
    }
}
