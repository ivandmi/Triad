using System;
using System.Collections.Generic;
using System.Linq;
using TriadCore;
using System.Reflection;

namespace TriadWpf.Models
{
    class RoutineDataService
    {
        public List<RoutineParameter> GetRoutineParameters(Routine routine)
        {
            Type type = routine.GetType();

            var constructors = type.GetConstructors();

            foreach(var contructor in constructors)
            {
                if(contructor.GetParameters().Length > 0)
                {
                    List<RoutineParameter> parameters = new List<RoutineParameter>();
                    foreach (var param in contructor.GetParameters())
                    {
                        var meta = new RoutineParamMetaData(param.Name, param.ParameterType);
                        var value = routine.GetValueForVar(new CoreName(param.Name));
                        parameters.Add(new RoutineParameter(meta, value));
                    }

                    return parameters; 
                }
            }

            return null;
        }

        public List<RoutineParamMetaData> GetRoutineVariables(Routine routine)
        {
            Type type = routine.GetType();

            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var fieldsList = fields.ToList();

            var constructorFields = GetRoutineParameters(routine);

            foreach(var field in constructorFields)
            {
                var first = fieldsList.FirstOrDefault(x => x.Name == field.MetaData.Name);
                if (first != null)
                {
                    fieldsList.Remove(first);
                }
            }

            List<RoutineParamMetaData> result = new List<RoutineParamMetaData>();
            foreach(var field in fieldsList)
            {
                var meta = new RoutineParamMetaData(field.Name, field.FieldType);
                result.Add(meta);
            }

            return result;
        }
    }
}
