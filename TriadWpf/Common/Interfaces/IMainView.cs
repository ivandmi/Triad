using System;
using System.Collections.Generic;
using TriadWpf.Common;
using TriadWpf.Common.GraphEventArgs;
using TriadWpf.Common.Interfaces;
using TriadWpf.GraphEventArgs;
using TriadWpf.Models;

namespace TriadWpf.Interfaces
{
    public interface IMainView
    {
        /// <summary>
        /// Событие на добавление вершины в представлении и в модели
        /// </summary>
        event EventHandler<VertexEventArgs> AddVertex;

        /// <summary>
        /// Срабатывает, когда требуется удалить вершину
        /// </summary>
        event EventHandler<VertexEventArgs> RemoveVertex;

        /// <summary>
        /// Срабатывает, когда надо добавить дугу в модель и в представление
        /// </summary>
        event EventHandler<EdgeEventArg> AddEdge;

        /// <summary>
        /// Срабатывает, когда надо удалить дугу из модели и представления
        /// </summary>
        event EventHandler<EdgeEventArg> RemoveEdge;

        /// <summary>
        /// Срабатывает, когда надо добавить полюс в модель и представление
        /// </summary>
        event EventHandler<PolusEventArgs> AddPolusToNode;

        event EventHandler<SimulationEventArgs> RunSimulation;

        /// <summary>
        /// Предоставляет управление над вершинами на форме
        /// </summary>
        IGraphViewManager GraphViewManager { get; }

        IProcedureView ProcedureView { get; }

        /// <summary>
        /// Отображает возможные для использования типы вершин, с определенными рутинами
        /// </summary>
        /// <param name="items">Список вершин с определенным типом вершин</param>
        void SetNodeTypes(IEnumerable<RoutineViewItem> items);

        void ShowError(string error);

        void ShowResults(List<ProcedureResult> results);
    }
}
