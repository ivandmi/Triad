using System.Windows.Input;
using TriadWpf.GraphXModels;

namespace TriadWpf.ViewModels
{
    /// <summary>
    /// Не совсем уверен, что для вершины нужна отдельная VM,
    /// решил упороться и сделать
    /// </summary>
    public class ViewModelVertex : ViewModelBase
    {
        private DataVertex vertex;
        public ICommand SetRoutine { get; private set; }

        public ViewModelVertex(DataVertex v)
        {
            vertex = v;
        }

        override public string ToString()
        {
            return vertex.ToString();
        }
    }
}
