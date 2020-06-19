using System.Windows;
using TriadCore;
using TriadWpf.Common.Enums;

namespace TriadWpf.Interfaces
{
    public interface IGraphViewManager
    {
        void AddVertex(CoreName node, Point point, RoutineType type);
        void AddVertex(CoreName node);
        void RemoveVertex(CoreName node);
        void AddEdge(CoreName from, CoreName to);
        void RemoveEdge(CoreName from, CoreName to);
        void AddPolusToVertex(CoreName nodeName, CoreName polusName);

        void ChangeVertexName(CoreName oldName, CoreName newName);
    }
}
