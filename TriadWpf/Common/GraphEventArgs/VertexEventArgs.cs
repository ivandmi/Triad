using System;
using System.Windows;
using System.Windows.Documents.DocumentStructures;
using TriadCore;
using TriadWpf.Common;

namespace TriadWpf.GraphEventArgs
{
    public class VertexEventArgs : EventArgs
    {
        /// <summary>
        /// Имя вершины
        /// </summary>
        public CoreName Name { get; }
        public RoutineViewItem RoutineViewItem { get; }

        public Point Point { get; }
        public VertexEventArgs() { }
        public VertexEventArgs(CoreName name)
        {
            Name = name;
        }

        public VertexEventArgs(Point point)
        {
            Point = point;
        }

        public VertexEventArgs(CoreName name, RoutineViewItem item)
        {
            Name = name;
            RoutineViewItem = item;
        }

        public VertexEventArgs(CoreName name, RoutineViewItem item, Point point)
        {
            Name = name;
            RoutineViewItem = item;
            Point = point;
        }
    }
}
