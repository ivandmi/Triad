using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadWpf.Common.Interfaces;
using TriadWpf.Models;

namespace TriadWpf.Presenter
{
    class SimulationResultPresenter
    {
        IResultView resultView;

        public SimulationResultPresenter(IResultView view, List<ProcedureResult> procedureResults)
        {
            resultView = view;

            resultView.ShowModelLog(TriadCore.Logger.Instance.Records);
            
            foreach(var procedureResult in procedureResults)
            {
                if (procedureResult.Result != null)
                {
                    resultView.AddProcedureResult(procedureResult);
                }

                if (procedureResult.OutputResults.Count > 0)
                {
                    foreach(var output in procedureResult.OutputResults)
                    {
                        resultView.AddProcedureOutputResult(procedureResult.ProcedureName, output.Key, output.Value);
                    }
                }
            }
        }
    }
}
