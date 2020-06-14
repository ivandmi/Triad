using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadCore;
using TriadWpf.Models;

namespace TriadWpf.Common.Interfaces
{
    interface IResultView
    {
        void ShowModelLog(List<LoggerRecord> records);

        // В идеале все-таки передавать не объект, а поля нужные для отображания
        void AddProcedureResult(ProcedureResult result);
        void AddProcedureOutputResult(string ProcedureName, string paramName, object value);
    }
}
