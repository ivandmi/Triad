using GraphX.Common.Enums;
using GraphX.Common.Interfaces;
using GraphX.Common.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;
using TriadCore;
using TriadWpf.ViewModels;

namespace TriadWpf.GraphXModels
{
    /// <summary>
    /// DataVertex должен реализовывать INotificationChange, так как вершина складывается в VertexControl.DataContext
    /// В таком случае, она не должна реализовывать Node, а должна содержать его внутри и впредоставлять взаимодействие с ним через команды.
    /// Но это не факт, что правильное решение. Не знаю, решил сделать так. 
    /// </summary>
    public class DataVertex : VertexBase, INotifyPropertyChanged
    {
        public Node Node { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DataVertex(Node node)
        {
            Node = node;
            AddPolusCommand = new RelayCommand(obj => AddPolus(obj));
            (AddPolusCommand as RelayCommand).IsEnabled = true;
        }

        private void AddPolus(object name)
        {
            string n = name as string;
            Node.DeclarePolus(new CoreName(n));
        }

        public ICommand AddPolusCommand { get; private set; }

        private void OnPropertyChanged(string propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }

        public override string ToString()
        {
            return Node.ToString();
        }
    }
}
