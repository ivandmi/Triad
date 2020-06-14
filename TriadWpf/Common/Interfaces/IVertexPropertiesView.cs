using System;
using System.Collections.Generic;
using TriadWpf.Common.GraphEventArgs;
using TriadWpf.Models;

namespace TriadWpf.Common.Interfaces
{
    public interface IVertexPropertiesView
    {
        void SetVertexName(string name);
        void ShowParamsAndVariables(List<RoutineParameter> parameters, List<RoutineParamMetaData> vars);

        event EventHandler<ChangeNameArgs> ChangeName;
        event EventHandler<UpdateParamValueArgs> UpdateRoutineParam;

    }
}
