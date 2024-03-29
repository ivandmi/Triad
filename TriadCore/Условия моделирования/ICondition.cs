#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;

namespace TriadCore
{

    /// <summary>
    /// ������� �������������
    /// </summary>
    public class ICondition : IProcedure, IStructExprStack
    {
        /// <summary>
        /// ������ ������������ �������������� ��������
        /// </summary>
        private Dictionary<int, IProcedure> iprocedureList = new Dictionary<int, IProcedure>();
        /// <summary>
        /// ���� ��� ��������� ����������� ���������
        /// </summary>
        private IStructExprStack structExprStack = new StructExprStack();

        public Graph CurrentModel { get; protected set; }

        /// <summary>
        /// �������� �������������� ���������
        /// </summary>
        /// <param name="iprocedure">���. ���������</param>
        /// <param name="ipNumber">����� ��</param>
        protected void AddIProcedure(IProcedure iprocedure, int ipNumber)
        {
            this.iprocedureList.Add(ipNumber, iprocedure);
        }


        /// <summary>
        /// �������� �������������� ��������� �� �� ����������� ������
        /// </summary>
        /// <param name="iprocedureNumber">���������� �����</param>
        /// <returns>���. ���������</returns>
        protected IProcedure GetIProcedure(int iprocedureNumber)
        {
            return this.iprocedureList[iprocedureNumber];
        }

        /// <summary>
        /// �������� ���-�� �������������� ��������
        /// </summary>
        /// <returns>���-�� �������������� ��������</returns>
        protected int GetIProcedureCount()
        {
            return this.iprocedureList.Count;
        }


        /// <summary>
        /// �������� ������� ������������� �� ��� ����������� ������
        /// </summary>
        /// <param name="iconditionNumber">���������� �����</param>
        /// <returns>������� �������������</returns>
        protected ICondition GetICondition(int iconditionNumber)
        {
            return this.GetIProcedure(iconditionNumber) as ICondition;
        }


        /// <summary>
        /// ��������� ������ ������������� � ���� ������������������ ��
        /// </summary>
        protected void InitializeAllIProcedure()
        {
            foreach (IProcedure ip in this.iprocedureList.Values)
                ip.DoInitialize();
        }


        /// <summary>
        /// ��������� ������ ������������� � ������������������ ��
        /// </summary>
        /// <param name="ipNumber">���������� ����� ��</param>
        protected void InitializeIProcedure(int ipNumber)
        {
            this.GetIProcedure(ipNumber).DoInitialize();
        }


        /// <summary>
        /// ���������, ����� �� ���������� �������������
        /// </summary>
        /// <param name="SystemTime">������� ��������� �����</param>
        /// <returns>True, ���� ����� ����������</returns>
        public virtual bool DoCheck(double SystemTime)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Model"></param>
        public void Initialize(Graph Model)
        {
            CurrentModel = Model;
            DoInitialize();
        }

        public virtual void OnTerminate()
        {
            CurrentModel = null;
        }

        //IStructExprStack
        public void PushGraph(Graph graph)
        {
            structExprStack.PushGraph(graph);
        }
        public void PushEmptyGraph()
        {
            structExprStack.PushEmptyGraph();
        }
        public void PushEmptyUndirectPathGraph()
        {
            structExprStack.PushEmptyUndirectPathGraph();
        }
        public void PushEmptyDirectPathGraph()
        {
            structExprStack.PushEmptyDirectPathGraph();
        }
        public void PushEmptyUndirectCicleGraph()
        {
            structExprStack.PushEmptyUndirectCicleGraph();
        }
        public void PushEmptyDirectCicleGraph()
        {
            structExprStack.PushEmptyDirectCicleGraph();
        }
        public void PushEmptyUndirectStarGraph()
        {
            structExprStack.PushEmptyUndirectStarGraph();
        }
        public void PushEmptyDirectStarGraph()
        {
            structExprStack.PushEmptyDirectStarGraph();
        }
        public Graph PopGraph()
        {
            return structExprStack.PopGraph();
        }

        public void PushEmptyErdosRenyiGraph()
        {
            structExprStack.PushEmptyErdosRenyiGraph();
        }

        public void PushEmptyBaklyOktusGraph()
        {
            structExprStack.PushEmptyBaklyOktusGraph();
        }

        public void PushEmptyBollRiordanGraph()
        {
            structExprStack.PushEmptyBollRiordanGraph();
        }

        public void PushEmptyCopyGraph()
        {
            structExprStack.PushEmptyCopyGraph();
        }

        public Graph FirstInStackGraph
        {
            get
            {
                return structExprStack.FirstInStackGraph;
            }
        }
        public Graph SecondInStackGraph
        {
            get
            {
                return structExprStack.SecondInStackGraph;
            }
        }
    }
}
