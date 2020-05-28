using GraphX.Common.Models;
using System.ComponentModel;
using System.Linq;
using TriadCore;

namespace TriadWpf.GraphXModels
{
    //Edge data object
    public class DataEdge : EdgeBase<DataVertex>, INotifyPropertyChanged
    {
        Polus polusFrom, polusTo;

        public event PropertyChangedEventHandler PropertyChanged;

        public DataEdge(DataVertex source, DataVertex target, double weight = 1)
            : base(source, target, weight)
        {
        }

        public DataEdge()
            : base(null, null, 1)
        {
        }

        public string Text { get; set; }

        public Polus PolusFrom
        {
            get => polusFrom;
            set
            {
                var oldValue = polusFrom;
                polusFrom = value;
                if (polusTo != null)
                {
                    if (polusTo.TargetInputPoluses.Contains(oldValue))
                    {
                        polusTo.RemoveInputArc(oldValue.Name, oldValue.BaseNode.Name);
                    }
                    polusTo.AddInputArc(polusFrom);
                    polusFrom.AddOutputArc(polusTo);
                }
                OnPropertyChanged("PolusFrom");
            }
        }

        public Polus PolusTo
        {
            get => polusTo;
            set
            {
                var oldValue = polusTo;
                polusTo = value;
                if (polusFrom != null)
                {
                    if (polusFrom.TargetOutputPoluses.Contains(oldValue))
                    {
                        polusFrom.RemoveOutputArc(oldValue.Name, oldValue.BaseNode.Name);
                    }
                    polusFrom.AddOutputArc(polusTo);
                    polusTo.AddInputArc(polusFrom);
                }
                OnPropertyChanged("PolusTo");
            }
        }

        public override string ToString()
        {
            return Text;
            Source.
        }

        private void OnPropertyChanged(string propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
                PropertyChanged()
            }
        }
    }
}
