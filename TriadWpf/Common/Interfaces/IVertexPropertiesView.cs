using System;
using System.Collections.Generic;
using TriadCore;
using TriadWpf.Common.GraphEventArgs;
using TriadWpf.GraphEventArgs;
using TriadWpf.Models;

namespace TriadWpf.Common.Interfaces
{
    public interface IVertexPropertiesView
    {
        void SetVertexName(string name);
        void ShowParamsAndVariables(List<RoutineParameter> parameters, List<RoutineParamMetaData> vars);
        void BadName(string errorText);

        void ShowPoluses(params CoreName[] poluses);

        event EventHandler<ChangeNameArgs> ChangeName;
        event EventHandler<UpdateParamValueArgs> UpdateRoutineParam;
        event EventHandler<PolusEventArgs> AddPolus;

    }
}
