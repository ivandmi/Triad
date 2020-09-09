using System;
using System.Collections.Generic;
using System.Text;

namespace TriadCore
{
    public class VarChanging : IProcedure
    {
        private List<Double> time;
        private List<Double> variable;

        private Double Arg
        {
            get
            {
                return Convert.ToDouble(GetSpyVarValue(new CoreName("Arg")));
            }
            set
            {
                SetSpyVarValue(new CoreName("Arg"), value);
            }
        }

        public void RegisterSpyObjects(SpyObject Arg)
        {
            RegisterSpyObject(Arg, new CoreName("Arg"));
            RegisterSpyHandler(Arg, DoHandling);
        }

        public override void DoInitialize()
        {
            time = new List<double>();
            variable = new List<double>(); 
        }

        protected override void DoHandling(SpyObject objectInfo, double systemTime)
        {
            time.Add(systemTime);
            variable.Add(Arg);
        }

        public void GetOutVariables(out double[] time, out double[] arg)
        {
            time = this.time.ToArray();
            arg = this.variable.ToArray();
        }
    }
}
