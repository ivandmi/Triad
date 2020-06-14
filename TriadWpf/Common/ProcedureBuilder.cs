using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadCore;
using TriadWpf.Common.GraphEventArgs;
using TriadWpf.Common.Interfaces;
using TriadWpf.Models;

namespace TriadWpf.Common
{
    class ProcedureBuilder
    {
        public void AddParamsToProcudure(Graph graph, IProcedure procedure, IDictionary<IPParamMetadata, NodeParam> procedureParams)
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
                NodeParam node;
                var pair = procedureParams.FirstOrDefault(x => x.Key.Name == param.Name);
                if (pair.Value.NodeName != null)
                {
                    SpyObjectType spyType = pair.Key.Type;
                    node = pair.Value;
                    SpyObject spyObject = graph[node.NodeName].CreateSpyObject(node.ParamName, spyType);
                    parameters.Add(spyObject);
                }
                else
                {
                    // Не для всех параметров нашли соответствие
                }
                
            }
            method.Invoke(procedure, parameters.ToArray());
        }
    }
}
