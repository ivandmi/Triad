using GraphX.Common.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TriadCore;
using TriadWpf.Common;

namespace TriadWpf.View.GraphXModels
{
    public class DataVertex : VertexBase, INotifyPropertyChanged
    {
        private CoreName nodeName;
        public CoreName NodeName 
        { 
            get => nodeName;
            set
            {
                nodeName = value;
                OnPropertyChanged("NodeName");
            } 
        }
        public ObservableCollection<CoreName> Poluses { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DataVertex(CoreName node)
        {
            NodeName = node;
            Poluses = new ObservableCollection<CoreName>();
        }

        private void AddPolus(object name)
        {
            string n = name as string;
            
        }

        private void OnPropertyChanged(string propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }

        public override string ToString()
        {
            return NodeName.ToString();
        }
    }
}
