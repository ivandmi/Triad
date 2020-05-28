using GraphX.Common.Enums;
using GraphX.Common.Interfaces;
using System;
using System.ComponentModel;
using TriadCore;

namespace TriadWpf.GraphXModels
{
    /// <summary>
    /// DataVertex должен реализовывать INotificationChange, так как вершина складывается в VertexControl.DataContext
    /// В таком случае, она не должна реализовывать Node, а должна содержать его внутри и впредоставлять взаимодействие с ним через команды.
    /// Но это не факт, что правильное решение. Не знаю, решил сделать так. 
    /// </summary>
    public class DataVertex : IGraphXVertex, IEquatable<IGraphXVertex>, IIdentifiableGraphDataObject, INotifyPropertyChanged
    {
        public double Angle { get; set; }
        public int GroupId { get; set; }
        public long ID { get; set; }
        public ProcessingOptionEnum SkipProcessing { get; set; }

        public Node Node { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Equals(IGraphXVertex other)
        {
            return ID == other.ID;
        }

        public DataVertex(CoreName name)
        {
            Node = new Node(name);
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
            return Node.ToString();
        }
    }
}
