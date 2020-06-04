using GraphX.Common.Enums;
using GraphX.Common.Interfaces;
using GraphX.Common.Models;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TriadCore;
using TriadWpf.ViewModels;

namespace TriadWpf.View.GraphXModels
{
    public class DataVertex : VertexBase, INotifyPropertyChanged
    {
        public CoreName NodeName { get; private set; }
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
