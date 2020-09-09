using GraphX.Common.Models;
using System.ComponentModel;
using System.Linq;
using TriadCore;

namespace TriadWpf.View.GraphXModels
{
    //Edge data object
    public class DataEdge : EdgeBase<DataVertex>, INotifyPropertyChanged
    {
        CoreName polusFrom, polusTo;

        public event PropertyChangedEventHandler PropertyChanged;

        public DataEdge(DataVertex source, DataVertex target, CoreName from, CoreName to, double weight = 1)
            : base(source, target, weight)
        {
            polusFrom = from;
            polusTo = to;
        }

        public DataEdge(DataVertex source, DataVertex target) : base(source, target)
        { }

        public DataEdge()
            : base(null, null, 1)
        {
        }

        public string Text { get; set; }

        public CoreName PolusFrom
        {
            get => polusFrom;
        }

        public CoreName PolusTo
        {
            get => polusTo;
        }

        public override string ToString()
        {
            //return polusFrom.ToString()+"->"+polusTo.ToString();
            return "";
        }

        private void OnPropertyChanged(string propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }
    }
}
