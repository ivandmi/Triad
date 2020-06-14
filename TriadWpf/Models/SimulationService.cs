using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadCore;

namespace TriadWpf.Models
{
    class SimulationService
    {
        public  List<ProcedureResult> Simulate(Graph graph, CommonCondition commonCondition , Dictionary<string, ICondition> conditions)
        {
            List<ICondition> array = new List<ICondition>();
            array.Add(commonCondition);
            array.AddRange(conditions.Values);

            graph.DoSimulate(array.ToArray());

            SimulationResultsService resultsService = new SimulationResultsService();

            List<ProcedureResult> proceduresResult = resultsService.GetSimulationProceduresResults(commonCondition.GetProcedures());

            List<ProcedureResult> results = resultsService.GetSimulationIConditionResult(conditions);

            proceduresResult.Concat(results);

            return proceduresResult;
        }
    }
}
