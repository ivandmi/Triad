using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TriadCore
{
    /// <summary>
    /// ���������� �����
    /// </summary>
    public class Graph : ICreatable
    {
        /// <summary>
        /// �����������
        /// </summary>
        public Graph()
            : this(new CoreName("����������� ����"))
        {
        }


        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="coreName">��� �����</param>
        public Graph(CoreName coreName)
        {
            if (coreName == null)
                throw new ArgumentNullException("������ ��� �����");

            this.coreName = coreName;
            this.systemTime = -1;
        }


        /// <summary>
        /// ������� ����� ����  (���������� ��� ������������� ��������)
        /// </summary>
        /// <returns>����� ����</returns>
        public object CreateNew()
        {
            return new Graph();
        }


        /// <summary>
        /// ��� �����
        /// </summary>
        public CoreName Name
        {
            get
            {
                return this.coreName;
            }
        }


        /// <summary>
        /// ���������� ������
        /// </summary>
        /// <value>��� �������</value>
        public Node this[CoreName nodeName]
        {
            get
            {
                if (nodeName == null)
                    throw new ArgumentNullException("������ ��� �������");

                if (!this.nodeList.ContainsKey(nodeName))
                    throw new ArgumentException(String.Format(
                        "���� \"{0}\" �� �������� ������� � ������ \"{1}\"", this, nodeName));

                return this.nodeList[nodeName];
            }
        }


        /// <summary>
        /// ���������� ������
        /// </summary>
        /// <value>������ ������� � �����</value>
        public Node this[int nodeIndex]
        {
            get
            {
                int currIndex = 0;
                foreach (Node node in nodeList.Values)
                {
                    if (currIndex == nodeIndex)
                        return node;
                    currIndex++;
                }

                throw new ArgumentOutOfRangeException("������ ������� ������� �� ���������� �������");
            }
        }


        /// <summary>
        /// ���������� ��� �����
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return coreName.ToString();
        }


        /// <summary>
        /// ������ ������ �����
        /// </summary>
        public IEnumerable<Node> Nodes
        {
            get
            {
                foreach (Node node in nodeList.Values)
                    yield return node;
            }
        }


        /// <summary>
        /// ����� ������ � �����
        /// </summary>
        public int NodeCount
        {
            get
            {
                return this.nodeList.Count;
            }
        }


        /// <summary>
        /// �������� �����
        /// </summary>
        /// <returns></returns>
        public Graph Clone()
        {
            //������� ����� �����
            Graph newGraph = new Graph(this.Name);

            //��������� ������ ��� ������
            foreach (KeyValuePair<CoreName, Node> pair in this.nodeList)
                newGraph.nodeList.Add(pair.Key, pair.Value.Clone());

            return newGraph;
        }

        #region ������������� ��������

        /// <summary>
        /// �������� �������
        /// </summary>
        /// <param name="nodeName">��� �������</param>
        public void DeclareNode(CoreName nodeName)
        {
            if (nodeName == null)
                throw new ArgumentNullException("������ ��� �������");

            //���� � ����� �� ���� ����� ��������� ����� �������
            if (!this.nodeList.ContainsKey(nodeName))
            {
                this.nodeList.Add(nodeName, new Node(nodeName));
            }
            else
            {
                //������ �� �������
            }
        }


        /// <summary>
        /// �������� ������� ������ � �� ��������
        /// </summary>
        /// <param name="nodeName">��� �������</param>
        /// <param name="polusNameList">������ ���� ������� �������</param>
        public void DeclareNode(CoreName nodeName, params CoreName[] polusNameList)
        {
            if (nodeName == null)
                throw new ArgumentNullException("������ ��� �������");

            //��������� ���� �������
            DeclareNode(nodeName);

            //��������� ������ � ����������� �������
            foreach (CoreName polusName in polusNameList)
            {
                this[nodeName].DeclarePolus(polusName);
            }
        }


        /// <summary>
        /// �������� ��������� ������
        /// </summary>
        /// <param name="nodeNameRange">����� ������</param>
        public void DeclareNode(CoreNameRange nodeNameRange)
        {
            if (nodeNameRange == null)
                throw new ArgumentNullException("������ �������� ���� �������");

            //��������� �������� ������
            foreach (CoreName nodeName in nodeNameRange)
            {
                DeclareNode(nodeName);
            }
        }

        public Node RenameNode(CoreName oldName, CoreName newName)
        {
            if(oldName == null)
                throw new ArgumentNullException("������ ������ ��� �������");

            if (newName == null)
                throw new ArgumentNullException("������ ����� ��� �������");

            Node oldNode = this[oldName];
            Node newNode = new Node(newName);

            newNode.Add(oldNode);
            newNode.NodeRoutine = oldNode.NodeRoutine;

            nodeList.Remove(oldName);
            nodeList.Add(newName, newNode);

            return newNode;
        }


        /// <summary>
        /// �������� ����� �� ���� ��������
        /// </summary>
        /// <param name="polusName">��� ������</param>
        public void DeclarePolusInAllNodes(CoreName polusName)
        {
            if (polusName == null)
                throw new ArgumentNullException("������ ��� ������");

            //��������� ����� �� ���� ��������
            foreach (Node node in this.nodeList.Values)
            {
                node.DeclarePolus(polusName);
            }
        }


        /// <summary>
        /// �������� ��������� ������� �� ���� ��������
        /// </summary>
        /// <param name="polusNameRange">����� �������</param>
        public void DeclarePolusInAllNodes(CoreNameRange polusNameRange)
        {
            if (polusNameRange == null)
                throw new ArgumentNullException("������ �������� ���� �������");

            //��������� �� ���� �������� ��� ������ �� ���������
            foreach (CoreName polusName in polusNameRange)
            {
                DeclarePolusInAllNodes(polusName);
            }
        }


        #endregion


        #region ������������ ��������


        /// <summary> 
        /// �������� ����
        /// </summary>
        /// <param name="nodeFromName">��������� �������</param>
        /// <param name="polusFromName">��� ���������� ������</param>
        /// <param name="nodeToName">�������� ������� </param>
        /// <param name="polusToName">��� ��������� ������</param>
        public void AddArc(CoreName nodeFromName, CoreName polusFromName, CoreName nodeToName, CoreName polusToName)
        {
            if (nodeFromName == null)
                throw new ArgumentNullException("������ ��� ������ �������");
            if (polusFromName == null)
                throw new ArgumentNullException("������ ��� ������ ������ �������");
            if (nodeToName == null)
                throw new ArgumentNullException("������ ��� ������ �������");
            if (polusToName == null)
                throw new ArgumentNullException("������ ��� ������ ������ �������");

            if (!this.nodeList.ContainsKey(nodeFromName))
                throw new ArgumentException(String.Format(
                    "���� \"{0}\" �� �������� ������� � ������ \"{1}\"", this, nodeFromName));
            if (!this.nodeList.ContainsKey(nodeToName))
                throw new ArgumentException(String.Format(
                    "���� \"{0}\" �� �������� ������� � ������ \"{1}\"", this, nodeToName));

            Node nodeFrom = this[nodeFromName];
            Node nodeTo = this[nodeToName];
            nodeFrom.AddArc(this[nodeFromName][polusFromName], nodeTo[polusToName]);
        }


        /// <summary>
        /// �������� �����. (����������� ������������ ���� �� polusFromName � polusToName � �� 
        /// polusToName � polusFromName)
        /// </summary>
        /// <param name="nodeFromName">��������� �������</param>
        /// <param name="polusFromName">��� ���������� ������</param>
        /// <param name="nodeToName">�������� ������� </param>
        /// <param name="polusToName">��� ��������� ������</param>
        public void AddEdge(CoreName nodeFromName, CoreName polusFromName, CoreName nodeToName, CoreName polusToName)
        {
            AddArc(nodeFromName, polusFromName, nodeToName, polusToName);
            AddArc(nodeToName, polusToName, nodeFromName, polusFromName);
        }


        /// <summary>
        /// ��������� � ��������
        /// </summary>
        /// <param name="node">�������</param>
        public void Add(Node node)
        {
            if (node == null)
                throw new ArgumentNullException("�������� ������ �������");

            //���� ����� ������� ���� � �����
            if (this.nodeList.ContainsKey(node.Name))
            {
                //������� ���������� ������� � �������� �����
                this.nodeList[node.Name].Add(node);
            }
            //���� ����� ������� ��� � �����
            else
            {
                //������ ��������� ������� � ����
                this.nodeList.Add(node.Name, node);
            }
        }


        /// <summary>
        /// ��������� � ������
        /// </summary>
        /// <param name="graph">����������� ����</param>
        public void Add(Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException("������� ������ ����");

            //������� ������� ���� �� ����� ��������� ����������� �����
            foreach (Node node in graph.nodeList.Values)
            {
                this.Add(node);
            }
        }


        /// <summary>
        /// ������� ������� �� ����� 
        /// </summary>
        /// <param name="node">�������</param>
        /// <remarks>���� ����� ��������� � ������� � ��� �� ������ �� �������� �������, �� ��� ��������� �� �����
        /// ���� � ���������� ������� ��� �������, �� �������� ������� ���� ���������</remarks>
        public void Subtract(Node node)
        {
            if (node == null)
                throw new ArgumentNullException("�������� ������ �������");

            //���� ������� ���� �������� ���������� �������
            if (this.nodeList.ContainsKey(node.Name))
            {
                //���� ���������� ������� �������� ������
                if (node.HasPoluses())
                {
                    //�������� ��������� ������� �� ������� �����
                    this.nodeList[node.Name].Subtract(node);

                    //���� ����� ��������� � ������� ����� �� �������� �������
                    if (!this.nodeList[node.Name].HasPoluses())
                    {
                        //�� ������� ������� �����
                        this.nodeList.Remove(node.Name);
                    }
                }
                //���� ���������� ������� �� �������� �������
                else
                {
                    //�� ������� ��������������� ������� ����� � ����� ������
                    RemoveNode(node.Name);
                }
            }
            //���� ������� ���� �� �������� ���������� �������
            else
            {
                //������ �� ������
            }
        }


        /// <summary>
        /// ������� ������� �� �����
        /// </summary>
        /// <param name="nodeName">��� ��������� �������</param>
        private void RemoveNode(CoreName nodeName)
        {
            if (nodeName == null)
                throw new ArgumentNullException("�������� ������ ��� �������");

            //������� ��� ������ �������
            this.nodeList[nodeName].RemoveAllPoluses();
            //������� ���� �������
            this.nodeList.Remove(nodeName);
        }


        /// <summary>
        /// ������� ����
        /// </summary>
        /// <param name="graph">����</param>
        public void Subtract(Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException("������� ������ ����");

            //�������� �� �������� ����� ��� ������� ����������� �����
            foreach (Node node in graph.nodeList.Values)
            {
                this.Subtract(node);
            }
        }


        /// <summary>
        /// �������� ������� ���� � ������
        /// </summary>
        /// <param name="graph">������ ����</param>
        public void Multiply(Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException("������� ������ ����");

            //������� �������� �����, ������� ����� �������� � ��������� �� ����������� �����
            List<Node> nodesToIntersect = new List<Node>();
            //������� �������� �����, ������� ����� �������
            List<Node> nodesToRemove = new List<Node>();

            //���������� ������� �������� �����
            foreach (Node node in this.nodeList.Values)
            {
                //���� ��������� ���� �������� ������� ������� �������� �����
                if (graph.nodeList.ContainsKey(node.Name))
                {
                    //�� ��� ������� �������� ����� ����� �������� � �������� ���������� �����
                    nodesToIntersect.Add(node);
                }
                //���� ��������� ���� �� �������� ������� ������� �������� �����
                else
                {
                    //�� ��� ������� ����� ������� �� �������� �����
                    nodesToRemove.Add(node);
                }
            }

            //������� ��������� �������
            foreach (Node node in nodesToRemove)
            {
                RemoveNode(node.Name);
            }

            //���������� ��������� �������
            foreach (Node node in nodesToIntersect)
            {
                node.Multiply(graph[node.Name]);
            }
        }


        /// <summary>
        /// ������� ��� ������� �����
        /// </summary>
        public void RemoveAllNodes()
        {
            //���� � ������� ����� ��� ���� �������
            while (this.nodeList.Count != 0)
            {
                RemoveNode(this.nodeList.Keys.GetEnumerator().Current);
            }
        }


        /// <summary>
        /// ��������� ���� ����������������� ������� � ������
        /// </summary>
        public virtual void CompleteGraph()
        {
            //������ �� ������ (��������� ���������������� � ������ ����������)
        }

        public virtual void CompleteGraph(double p) { }
        public virtual void CompleteGraph(int k, int a) { }
        public virtual void CompleteGraph(int k) { }
        public virtual void CompleteGraph(int m, double p) { }

        #endregion


        #region �������� � ��������


        /// <summary>
        /// ������ ������ ��� ������� � �����
        /// </summary>
        /// <param name="nodeName">��� �������</param>
        /// <param name="routine">������</param>
        public void RegisterRoutine(CoreName nodeName, Routine routine)
        {
            if (nodeName == null)
                throw new ArgumentNullException("������ ��� �������");
            if (routine == null)
                throw new ArgumentNullException("�������� ������ ������");

            Node node = this[nodeName];
            node.RegisterRoutine(routine);

            if (systemTime >= 0)
            {
                node.InitializeRoutine();
                node.NodeRoutine.EventCalendar.SystemTime = systemTime;
                node.DoRoutineInitialSection();
            }
        }


        /// <summary>
        /// ������ ������ ��� ��������� ������ � �����
        /// </summary>
        /// <param name="nodeNameRange">�������� ���� ������</param>
        /// <param name="routine">������</param>
        public void RegisterRoutine(CoreNameRange nodeNameRange, Routine routine)
        {
            if (nodeNameRange == null)
                throw new ArgumentNullException("������ �������� ���� ������");
            if (routine == null)
                throw new ArgumentNullException("�������� ������ ������");

            foreach (CoreName nodeName in nodeNameRange)
            {
                RegisterRoutine(nodeName, routine);
            }
        }


        /// <summary>
        /// ������ ������ ��� ���� ������ �����
        /// </summary>
        /// <param name="routine">������������� ������</param>
        public void RegisterRoutineInAllNodes(Routine routine)
        {
            if (routine == null)
                throw new ArgumentNullException("�������� ������ ������");

            foreach (CoreName name in this.nodeList.Keys)
            {
                RegisterRoutine(name, routine);
            }
        }


        /// <summary>
        /// ���������������� ������ ���� ������
        /// </summary>
        private void InitializeRoutineInAllNodes()
        {
            // ������� �����
            Logger.Instance.Clear();

            //�������������� ������ ���� ������
            foreach (Node node in this.nodeList.Values)
            {
                node.InitializeRoutine();
            }
            //��������� ������ Initial � ���� �����
            foreach (Node node in this.nodeList.Values)
            {
                node.DoRoutineInitialSection();
            }
        }


        /// <summary>
        /// �������� ������� ��������� �������������
        /// </summary>
        /// <param name="currSystemTime">������� ��������� �����</param>
        /// <returns>True, ���� ������������� ����� ����������</returns>
        private delegate bool CheckEndOfModelling(double currSystemTime);


        /// <summary>
        /// ������ ������� ��������
        /// </summary>
        /// <param name="checkEndOfModelling">�������� ������� ��������� �������������</param>
        private void DoSimulate(CheckEndOfModelling checkEndOfModelling)
        {
            systemTime = 0;

            //���������� ������ initial � ���� �����
            InitializeRoutineInAllNodes();

            //������, ������� ������� ����������
            Routine routineToExecute;

            do
            {
                routineToExecute = null;
                //����� ���������� ���������������� �������
                double minTime = CommonEvent.MaxEventTime;

                //���������� ������� �����
                foreach (Node node in this.nodeList.Values)
                {
                    if (node.NodeRoutine != null)
                        //���� ��� ��������� �����
                        if (node.NodeRoutine.EventCalendar.NextEventTime < minTime &&
                            //� ���� ������� ��� ������������
                            node.NodeRoutine.EventCalendar.HasEventToExecute)
                        {
                            minTime = node.NodeRoutine.EventCalendar.NextEventTime;
                            routineToExecute = node.NodeRoutine;
                        }
                }

                //���� ������ ��� ���������� ��� � �� �����
                if (routineToExecute == null)
                    break;

                //��������� � ������ ��������� �������
                routineToExecute.EventCalendar.DoNextEvent();
                systemTime = routineToExecute.EventCalendar.SystemTime;
            }
            while (checkEndOfModelling(systemTime));

            systemTime = -1;
        }


        /// <summary>
        /// ������ ������� ��������
        /// </summary>
        /// <param name="iConditions">������� �������������</param>
        public void DoSimulate(params ICondition[] iConditions) //???????????????????
        {
            //���������� ������ initial � ���� �����
            InitializeRoutineInAllNodes();

            foreach (ICondition iCondition in iConditions)
                iCondition.Initialize(this);

            this.DoSimulate(delegate (double currSystemTime)
               {
                   bool bResult = true;
                   foreach (ICondition iCondition in iConditions)
                   {
                       if (!iCondition.DoCheck(currSystemTime))
                           bResult = false;
                   }
                   return bResult;
               });

            foreach (ICondition iCondition in iConditions)
                iCondition.OnTerminate();
        }



        /// <summary>
        /// ������ ������� ��������
        /// </summary>
        /// <param name="endTime">�������� ����� �������������</param>
        public void DoSimulate(double endTime)
        {
            this.DoSimulate(delegate (double currSystemTime)
               {
                   return currSystemTime < endTime;
               });
        }

        #endregion


        #region �������� ��� ����������� �����

        /// <summary>
        /// �������� �������� ����� �����
        /// </summary>
        /// <param name="outPolusName">��� ��������� ������</param>
        /// <param name="internalPolusName">��� ����������� ������</param>
        public void DefineOutPolus(CoreName outPolusName, UniquePolusName internalPolusName)
        {
            if (outPolusName == null)
                throw new ArgumentNullException("������ ��� ������");
            if (internalPolusName == null)
                throw new ArgumentNullException("������ ��� ������");

            //���� �������� ����� � ����� ������ ����� �� ���������������
            if (!outPolusList.ContainsKey(outPolusName))
            {
                outPolusList.Add(outPolusName, internalPolusName);
            }
            //���� �������� ����� � ����� ������ ��� ��� ���������������
            else
            {
                //��������� �������� ����� � ������ ���������� �������
                outPolusList[outPolusName] = internalPolusName;
            }
        }


        /// <summary>
        /// ������������ ������� �������� ����� ������ ������
        /// </summary>
        /// <param name="nodeName">��� ���������������� ������� �������� �����</param>
        /// <param name="graph">����, ������� ������������� ������ ���������������� �������</param>
        /// <param name="polusPairList">������ ������������ ���� ������� ����������������
        /// ������� � ������� ������� ���������� �����</param>
        public void OpenNode(CoreName nodeName, Graph graph, params CoreName[] polusPairList)
        {
            if (nodeName == null)
                throw new ArgumentNullException("������ ��� �������");

            if (graph == null)
                throw new ArgumentNullException("������ ����");

            if (polusPairList == null)
                throw new ArgumentNullException("������ ������ ������������");

            //���� � ����� ��� ������� � ����� ������
            if (!nodeList.ContainsKey(nodeName))
                throw new ArgumentException("� ����� ��� ������� � ������ " + nodeName.ToString());

            //���������������� �������
            Node decodedNode = this[nodeName];

            //������ ���������������� �������
            List<Polus> decodedNodePolusList = new List<Polus>();

            //��������� ������ ������� ���������������� �������
            foreach (Polus polus in decodedNode.Poluses)
            {
                decodedNodePolusList.Add(polus.Clone());
            }

            //������� ���������������� �������
            RemoveNode(decodedNode.Name);

            //��������� 
        }


        #endregion

        /// <summary>
        /// ��������� �����
        /// </summary>
        private double systemTime;
        /// <summary>
        /// ��� �����
        /// </summary>
        private CoreName coreName;
        /// <summary>
        /// ������ ���������� � ���� ������
        /// </summary>
        private Dictionary<CoreName, Node> nodeList = new Dictionary<CoreName, Node>();
        /// <summary>
        /// ������ �������� ������� �����
        /// Key - ��� ��������� ������
        /// Value - ��� ���������������� ����������� ������
        /// </summary>
        private Dictionary<CoreName, UniquePolusName> outPolusList = new Dictionary<CoreName, UniquePolusName>();
    }

}
