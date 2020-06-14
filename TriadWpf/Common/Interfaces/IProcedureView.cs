using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadWpf.Common.GraphEventArgs;
using TriadWpf.Models;

namespace TriadWpf.Common.Interfaces
{
    public interface IProcedureView
    {
        void SetProceduresTypes(IEnumerable<IProcedureMetadata> procedures);
        void SetProcedureBluePrints(IEnumerable<ProcedureBlueprint> blueprints);

        void AddProcedureBlueprint(ProcedureBlueprint blueprint);

        event EventHandler<ProcedureEventArgs> CreateProcedureBlueprint;

        event EventHandler<ProcedureEventArgs> RemoveProcedure;

        event EventHandler<ProcedureEventArgs> SaveProcedure;

    }
}
