using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadCore;

namespace TriadWpf.Common.Interfaces
{
    interface ICommonCondition
    {
        void AddInfProcedure(IProcedure procedure);
        IProcedure this[int index] { get; }

    }
}
