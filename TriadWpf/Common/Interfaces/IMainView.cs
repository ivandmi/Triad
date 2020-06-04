using System;
using System.Collections.Generic;
using TriadWpf.Common;
using TriadWpf.GraphEventArgs;

namespace TriadWpf.Interfaces
{
    public interface IMainView
    {
        event EventHandler<VertexEventArgs> AddVertex;
        event EventHandler<VertexEventArgs> RemoveVertex;
        event EventHandler<EdgeEventArg> AddEdge;
        event EventHandler<EdgeEventArg> RemoveEdge;
        event EventHandler<PolusEventArgs> AddPolusToNode;
        IGraphViewManager GraphViewManager { get; }
        void SetNodeTypes(IEnumerable<RoutineViewItem> items);
    }
}
