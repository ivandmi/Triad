using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadCore;
using TriadWpf.Common.GraphEventArgs;

namespace TriadWpf.Common
{
    class ProcedureBuilder
    {
        void AddParamsToProcudure(Graph graph, IProcedure procedure, IDictionary<string,NodeParam> procedureParams)
        {
            Type type = procedure.GetType();

            // Мне не нравится, что тут тугая связка с названием, которое нигде в интерфейсах не фигурирует
            var method = type.GetMethod("RegisterSpyObjects");
            
            // Подумать как обозначить, что не получилось параметры привязать к процедуре
            if (method == null)
                return;

            List<object> parameters = new List<object>();
            foreach(var param in method.GetParameters())
            {
                NodeParam nodeParam;
                if (procedureParams.TryGetValue(param.Name, out nodeParam))
                {
                    //
                    // Надо придумать откуда я это извлекаю
                    //
                    SpyObjectType spyType = SpyObjectType.Var;
                    SpyObject spyObject = graph[nodeParam.NodeName].CreateSpyObject(nodeParam.ParamName, spyType);
                    parameters.Add(spyObject);
                }
                else
                {
                    // Не для всех параметров нашли параметры
                }
                
            }
            method.Invoke(procedure, parameters.ToArray());
        }
    }
}
