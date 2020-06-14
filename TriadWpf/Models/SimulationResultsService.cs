using System;
using System.Collections.Generic;
using System.Linq;
using TriadCore;

namespace TriadWpf.Models
{
    class SimulationResultsService
    {
        public List<ProcedureResult> GetSimulationProceduresResults(Dictionary<string, IProcedure> procedures)
        {
            List<ProcedureResult> result = new List<ProcedureResult>();

            foreach(var pair in procedures)
            {
                var procedure = pair.Value;
                var procRes = GetProcedureResult(procedure, pair.Key);

                result.Add(procRes);
            }

            return result;
        }

        public List<ProcedureResult> GetSimulationIConditionResult(Dictionary<string, ICondition> procedures)
        {
            List<ProcedureResult> result = new List<ProcedureResult>();

            foreach (var pair in procedures)
            {
                var procedure = pair.Value;
                var procRes = GetProcedureResult(procedure, pair.Key);

                result.Add(procRes);
            }

            return result;
        }

        public ProcedureResult GetProcedureResult(IProcedure procedure, string name)
        {
            var procRes = new ProcedureResult(name);

            procRes.Result = GetDoProcessingResult(procedure);
            procRes.OutputResults = GetOutVariables(procedure);

            return procRes;
        }

        /// <summary>
        /// Извлекает результат процедуры, который возвращается из метода
        /// DoProcessing
        /// </summary>
        /// <param name="procedure">Процедура или условие моделирования</param>
        /// <returns></returns>
        private object GetDoProcessingResult(IProcedure procedure)
        {
            Type type = procedure.GetType();
            // Не нравится, что извлекаем по имени, хотелось бы проверять что
            // реализован например интерфейс IProcessing
            var method = type.GetMethod("DoProcessing");
            if (method!=null && method.ReturnType == typeof(void))
            {
                return method.Invoke(procedure, null);
            }
            return null;  
        }

        private Dictionary<string, object> GetOutVariables(IProcedure procedure)
        {
            Type type = procedure.GetType();
            var method = type.GetMethod("GetOutVariables");

            Dictionary<string, object> results = new Dictionary<string, object>();

            var parameters = method?.GetParameters();

            foreach(var param in parameters)
            {
                var res = Activator.CreateInstance(param.GetType());
                results.Add(param.Name, res);
            }
            method?.Invoke(procedure, results.Values.ToArray());

            return results;
        }
    }
}
