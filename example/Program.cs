using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadCore;

namespace example
{
    //public class MyRout : Routine
    //{

    //    private Boolean Defence;

    //    private Boolean bad;

    //    private Double beta;

    //    private Double gama;

    //    private Int32 i;

    //    private Double Seed = 0;

    //    public MyRout(Boolean Defence, Boolean bad, Double beta, Double gama)
    //    {
    //        this.Defence = Defence;
    //        this.bad = bad;
    //        this.beta = beta;
    //        this.gama = gama;
    //    }

    //    public override void DoInitialize()
    //    {
    //        if ((bad))
    //        {
    //            Schedule(1, this.SendMessage);
    //        }
    //    }

    //    private void SendMessage()
    //    {
    //        SendMessageViaAllPoluses(" ");
    //        if ((Rand.RandomReal() < gama))
    //        {
    //            Defence = true;
    //            DoVarChanging(new CoreName("Defence"));
    //            bad = false;
    //            DoVarChanging(new CoreName("bad"));
    //        }
    //        if ((Defence == false))
    //        {
    //            Schedule(1, this.SendMessage);
    //        }
    //    }

    //    protected override void ReceiveMessageVia(CoreName polusName, String message)
    //    {
    //        Int32 polusIndex = GetPolusIndex(polusName);
    //        if ((Defence == false) && (Seed == 0) && (Rand.RandomReal() < beta))
    //        {
    //            Schedule(1, this.SendMessage);
    //            bad = true;
    //            DoVarChanging(new CoreName("bad"));
    //            Seed = Seed + 1;
    //            DoVarChanging(new CoreName("Seed"));
    //        }
    //    }
    //}

    //public class M : GraphBuilder
    //{

    //    public override Graph Build()
    //    {
    //        Graph M;
    //        M = new Graph();
    //        this.PushEmptyErdosRenyiGraph();
    //        this.PushEmptyGraph();
    //        this.FirstInStackGraph.DeclareNode(new CoreNameRange("A", 0, 49));
    //        this.FirstInStackGraph.DeclarePolusInAllNodes(new CoreNameRange("pol", 0, 49));
    //        this.SecondInStackGraph.Add(this.FirstInStackGraph);
    //        this.PopGraph();
    //        this.FirstInStackGraph.CompleteGraph(0.5);
    //        M = this.PopGraph();
    //        M[0].RemoveAllPoluses();
    //        M[0].DeclarePolus(new CoreNameRange("pol", 0, 49));
    //        M.AddEdge(M[0].Name, M[0][1].Name, M[1].Name, M[1][0].Name);
    //        return M;
    //    }

    //    public void setrout(Graph g)
    //    {
    //        Routine r;
    //        r = new Routine();
    //        r = new MyRout(false, true, 0.5, 0.5);
    //        r.ClearPolusPairList();
    //        r.AddPolusPair(new CoreNameRange("pol", 0, 49), new CoreNameRange("pol", 0, 49));
    //        g.RegisterRoutine(new CoreName("A", 0), r);
    //        r = new MyRout(false, false, 0.5, 0.5);
    //        r.ClearPolusPairList();
    //        r.AddPolusPair(new CoreNameRange("pol", 0, 49), new CoreNameRange("pol", 0, 49));
    //        g.RegisterRoutine(new CoreNameRange("A", 1, 49), r);
    //    }
    //}

    //public class ip : IProcedure
    //{

    //    private Int32 cbad;

    //    private Int32 cdef;

    //    private Int32 cfree;

    //    private Int32 i;

    //    private Double prevtime = 0;

    //    public override void DoInitialize()
    //    {
    //        PrintMessage("time\t" + "bad\t" + "def\t" + "\tfree");
    //    }

    //    public void RegisterSpyObjects(SpyObject[] bad, SpyObject[] Defence)
    //    {
    //        RegisterSpyObject(bad, new CoreNameRange("bad", 0, 49));
    //        RegisterSpyHandler(bad, DoHandling);
    //        RegisterSpyObject(Defence, new CoreNameRange("Defence", 0, 49));
    //        RegisterSpyHandler(Defence, DoHandling);
    //    }

    //    public void GetOutVariables()
    //    {
    //    }

    //    protected override void DoHandling(SpyObject spyObject, Double SystemTime)
    //    {
    //        if (prevtime != SystemTime)
    //        {
    //            PrintMessage(Convertion.ToStr(prevtime) + "\t" + Convertion.ToStr(cbad) + "\t" + Convertion.ToStr(cdef) + "\t" + Convertion.ToStr(cfree));
    //        }
    //        cbad = 0;
    //        cdef = 0;
    //        cfree = 0;
    //        for (i = 0; (i <= 49); i = (i + 1))
    //        {
    //            if (getDefence(i))
    //            {
    //                cdef = cdef + 1;
    //            }
    //            else
    //            {
    //                if (bad[i])
    //                {
    //                    cbad = cbad + 1;
    //                }
    //                else
    //                {
    //                    cfree = cfree + 1;
    //                }
    //            }
    //        }
    //        prevtime = SystemTime;
    //    }

    //    public void DoProcessing()
    //    {
    //    }

    //    private Boolean[] bad
    //    {
    //        get
    //        {
    //            return ((Boolean[])GetSpyVarValue(new CoreName("bad")));
    //        }
    //    }

    //    private Boolean getDefence(params int[] indexList)
    //    {
    //        return ((Boolean)GetSpyVarValue(new CoreName("Defence", indexList)));
    //    }
    //}

    //public class ic : ICondition
    //{

    //    private Double time;

    //    public ic(Double time)
    //    {
    //        this.time = time;
    //    }

    //    public override void DoInitialize()
    //    {
    //        AddIProcedure(new ip(), 0);
    //        ((ip)GetIProcedure(0)).RegisterSpyObjects(GetSpyObject(new CoreNameRange("bad", 0, 49)), GetSpyObject(new CoreNameRange("Defence", 0, 49)));
    //        InitializeIProcedure(0);
    //    }

    //    public void RegisterSpyObjects(SpyObject[] bad, SpyObject[] Defence)
    //    {
    //        SpyObject[] var = new SpyObject[10];
    //        for

    //        RegisterSpyObject(var, new CoreNameRange("var", 0, 9));
    //        Int32[] v = ((Int32[])GetSpyVarValue(new CoreName("var")));
    //        RegisterSpyObject(bad, new CoreNameRange("bad", 0, 49));
    //        RegisterSpyHandler(bad, DoHandling);
    //        RegisterSpyObject(Defence, new CoreNameRange("Defence", 0, 49));
    //        RegisterSpyHandler(Defence, DoHandling);
    //    }

    //    public void GetOutVariables()
    //    {
    //    }

    //    protected override void DoHandling(SpyObject spyObject, Double SystemTime)
    //    {
    //        String message = spyObject.Data;
    //    }

    //    public void DoProcessing()
    //    {
    //    }

    //    public override Boolean DoCheck(Double SystemTime)
    //    {
    //        if (SystemTime > time)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    private Boolean getbad(params int[] indexList)
    //    {
    //        return ((Boolean)GetSpyVarValue(new CoreName("bad", indexList)));
    //    }

    //    private Boolean getDefence(params int[] indexList)
    //    {
    //        return ((Boolean)GetSpyVarValue(new CoreName("Defence", indexList)));
    //    }
    //}

    //public class D : Design
    //{

    //    public override Graph Build()
    //    {
    //        Graph D;
    //        D = new Graph();
    //        Graph m;
    //        m = new Graph();
    //        m = new M().Build();
    //        PrintMessage("Количество связей: "+StandartFunctions.GetNodeDegree(m[0]));
    //        ic IC1 = new ic(24);
    //        ic IC2 = new ic(24);
    //        SpyObject[] bad = new SpyObject[50];
    //        for (int i = 0
    //        ; (i <= 49); i++
    //        )
    //        {
    //            bad[i] = m[i].CreateSpyObject(new CoreName("bad"), SpyObjectType.Var);
    //        }
    //        SpyObject[] Defence = new SpyObject[50];
    //        for (int i = 0
    //        ; (i <= 49); i++
    //        )
    //        {
    //            Defence[i] = m[i].CreateSpyObject(new CoreName("Defence"), SpyObjectType.Var);
    //        }
    //        Graph ex2 = m.Clone();
    //        IC1.RegisterSpyObjects(bad, Defence);
    //        IC2.RegisterSpyObjects(bad, Defence);
    //        m.DoSimulate(IC1);

    //        Set ns = StandartFunctions.GetAdjacentNodes(m[0]);
    //        int count = 0;
    //        for(int i=1;i<50;i++)
    //        {
    //            if (!ns.In(m[i]) && count<5)
    //            {
    //                count++;
    //                m.AddEdge(m[0].Name, m[0][i].Name, m[i].Name, m[i][0].Name);
    //            }
    //        }
    //        PrintMessage("Количество связей: " + StandartFunctions.GetNodeDegree(m[0]));

    //        ex2.DoSimulate(IC2);

    //        return D;
    //    }
    //}

    public class R : Routine
    {

        private Int32[,] a;

        public override void DoInitialize()
        {
            this.a = new Int32[10, 10];
            Schedule(0, this.E);
            a[1, 5] = 0;
            DoVarChanging(new CoreName("a", 1, 5));
        }

        private void E()
        {
            Schedule(10, this.E);
            PrintMessage("E");
            a[1, 5] = a[1, 5] + 1;
            DoVarChanging(new CoreName("a", 1, 5));
        }

        protected override void ReceiveMessageVia(CoreName polusName, String message)
        {
            Int32 polusIndex = GetPolusIndex(polusName);
            if (polusName.Equals(new CoreName("P")))
            {
                PrintMessage("Сообщение на полюсе P - " + message);
            }
        }
    }

    public class Send : Routine
    {

        public override void DoInitialize()
        {
            Schedule(30, this.SendOut);
        }

        private void SendOut()
        {
            SendMessageViaAllPoluses("mess");
            Schedule(30, this.SendOut);
        }
    }

    public class M : GraphBuilder
    {

        public override Graph Build()
        {
            Graph M;
            M = new Graph();
            this.PushEmptyGraph();
            this.FirstInStackGraph.DeclareNode(new CoreName("A"));
            this.FirstInStackGraph.DeclarePolusInAllNodes(new CoreName("Pol"));
            this.PushEmptyGraph();
            this.FirstInStackGraph.DeclareNode(new CoreName("B"));
            this.FirstInStackGraph.DeclarePolusInAllNodes(new CoreName("Pol"));
            this.SecondInStackGraph.Add(this.FirstInStackGraph);
            this.PopGraph();
            this.PushEmptyGraph();
            this.FirstInStackGraph.DeclareNode(new CoreName("B"), new CoreName("Pol"));
            this.FirstInStackGraph.DeclareNode(new CoreName("A"), new CoreName("Pol"));
            this.FirstInStackGraph.AddArc(new CoreName("B"), new CoreName("Pol"), new CoreName("A"), new CoreName("Pol"));
            this.SecondInStackGraph.Add(this.FirstInStackGraph);
            this.PopGraph();
            M = this.PopGraph();
            Routine r;
            r = new Routine();
            r = new R();
            R rr;

            r.ClearPolusPairList();
            r.AddPolusPair(new CoreName("P"), new CoreName("Pol"));
            M.RegisterRoutine(new CoreName("A"), r);
            r = new Send();
            r.ClearPolusPairList();
            r.AddPolusPair(new CoreName("P"), new CoreName("Pol"));
            M.RegisterRoutine(new CoreName("B"), r);
            return M;
        }
    }

    public class IP : IProcedure
    {

        private Int32[,] var
        {
            get
            {
                return ((Int32[,])GetSpyVarValue(new CoreName("var")));
            }
            set
            {
                SetSpyVarValue(new CoreName("var"), value);
            }
        }

        public override void DoInitialize()
        {
        }

        public void RegisterSpyObjects(SpyObject ev, SpyObject[] var)
        {
            RegisterSpyObject(ev, new CoreName("ev"));
            RegisterSpyHandler(ev, DoHandling);
            RegisterSpyObject(var, new CoreNameRange("var", 0, 9, 0, 9));
            RegisterSpyHandler(var, DoHandling);
        }

        public void GetOutVariables()
        {
        }

        protected override void DoHandling(SpyObject spyObject, Double SystemTime)
        {
            String message = spyObject.Data;
            if (spyObject.Equals(GetSpyObject(new CoreName("ev"))))
            {
                PrintMessage("Сработало событие ev");
            }
            else
            {
                if (spyObject.Equals(GetSpyObject(new CoreNameRange("var", 1, 1, 0, 9))))
                {
                    PrintMessage("Изменилась переменная var");
                }
            }
        }

        public Int32 DoProcessing()
        {
            Int32 IP = new Int32();
            IP = var[1, 5];
            return IP;
        }
    }

    public class IP2 : IProcedure
    {

        public override void DoInitialize()
        {
        }

        public void RegisterSpyObjects(SpyObject Q)
        {
            RegisterSpyObject(Q, new CoreName("Q"));
            RegisterSpyHandler(Q, DoHandling);
        }

        public void GetOutVariables()
        {
        }

        protected override void DoHandling(SpyObject spyObject, Double SystemTime)
        {
            String message = spyObject.Data;
            if (spyObject.Equals(GetSpyObject(new CoreName("Q"))))
            {
                PrintMessage("На полюс Q пришло сообщение - " + message);
                BlockPolus(new CoreName("Q"));
            }
        }

        public void DoProcessing()
        {
        }
    }

    public class IC : ICondition
    {

        private Double terminateTime;

        private Int32 result;

        public IC(Double terminateTime)
        {
            this.terminateTime = terminateTime;
        }

        private Int32[,] var
        {
            get
            {
                return ((Int32[,])GetSpyVarValue(new CoreName("var")));
            }
            set
            {
                SetSpyVarValue(new CoreName("var"), value);
            }
        }

        public override void DoInitialize()
        {
            AddIProcedure(new IP(), 2);
            ((IP)GetIProcedure(2)).RegisterSpyObjects(GetSpyObject(new CoreName("ev")), GetSpyObject(new CoreNameRange("var", 0, 9, 0, 9)));
            InitializeIProcedure(2);
            AddIProcedure(new IP2(), 3);
            ((IP2)GetIProcedure(3)).RegisterSpyObjects(GetSpyObject(new CoreName("Q")));
            InitializeIProcedure(3);
        }

        public void RegisterSpyObjects(SpyObject ev, SpyObject[] var, SpyObject Q)
        {
            RegisterSpyObject(ev, new CoreName("ev"));
            RegisterSpyHandler(ev, DoHandling);
            RegisterSpyObject(var, new CoreNameRange("var", 0, 9, 0, 9));
            RegisterSpyHandler(var, DoHandling);
            RegisterSpyObject(Q, new CoreName("Q"));
            RegisterSpyHandler(Q, DoHandling);
        }

        public void GetOutVariables(out Int32 result)
        {
            result = this.result;
        }

        protected override void DoHandling(SpyObject spyObject, Double SystemTime)
        {
            String message = spyObject.Data;
        }

        public void DoProcessing()
        {
        }

        public override Boolean DoCheck(Double SystemTime)
        {
            result = ((IP)GetIProcedure(2)).DoProcessing();
            PrintMessage("Результат IP = " + Convertion.IntToStr(result));
            if (SystemTime > terminateTime)
            {
                return false;
            }
            return true;
        }
    }

    public class D : Design
    {

        private Int32 result;

        public override Graph Build()
        {
            Graph D;
            D = new Graph();
            Graph m;
            m = new Graph();
            m = new M().Build();
            PrintMessage("Первый прогон");
            AddIProcedure(new IC(200), 0);
            ((IC)GetIProcedure(0)).RegisterSpyObjects(m[new CoreName("A")].CreateSpyObject(new CoreName("E"), SpyObjectType.Event), m[new CoreName("A")].CreateSpyObject(new CoreNameRange("a", 0, 9, 0, 9), SpyObjectType.Var), m[new CoreName("A")].CreateSpyObject(new CoreName("Pol"), SpyObjectType.Polus));
            m.DoSimulate(GetICondition(0));
            ((IC)GetIProcedure(0)).GetOutVariables(out result);
            if (result > 20)
            {
                PrintMessage("\n\n************************************\nВторой прогон");
                AddIProcedure(new IC(result), 1);
                ((IC)GetIProcedure(1)).RegisterSpyObjects(m[new CoreName("A")].CreateSpyObject(new CoreName("E"), SpyObjectType.Event), m[new CoreName("A")].CreateSpyObject(new CoreNameRange("a", 0, 9, 0, 9), SpyObjectType.Var), m[new CoreName("A")].CreateSpyObject(new CoreName("Pol"), SpyObjectType.Polus));
                m.DoSimulate(GetICondition(1));
            }
            PrintMessage("Результат IC=" + Convertion.IntToStr(result));
            return D;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //M bilder;
            //bilder = new M();
            //Graph m = bilder.Build();
            //Graph ex2 = m.Clone();
            //bilder.setrout(m);
            //bilder.setrout(ex2);
            //Console.WriteLine("Количество связей: " + StandartFunctions.GetNodeDegree(m[0]));
            //ic IC1 = new ic(24);
            //ic IC2 = new ic(24);
            //SpyObject[] bad = new SpyObject[50];
            //for (int i = 0
            //; (i <= 49); i++
            //)
            //{
            //    bad[i] = m[i].CreateSpyObject(new CoreName("bad"), SpyObjectType.Var);
            //}
            //SpyObject[] Defence = new SpyObject[50];
            //for (int i = 0
            //; (i <= 49); i++
            //)
            //{
            //    Defence[i] = m[i].CreateSpyObject(new CoreName("Defence"), SpyObjectType.Var);
            //}

            //IC1.RegisterSpyObjects(bad, Defence);
            //m.DoSimulate(IC1);

            //Set ns = StandartFunctions.GetAdjacentNodes(m[0]);
            //int count = 0;
            //for (int i = 1; i < 50; i++)
            //{
            //    if (!ns.In(m[i]) && count < 5)
            //    {
            //        count++;
            //        m.AddEdge(m[0].Name, m[0][i].Name, m[i].Name, m[i][0].Name);
            //    }
            //}
            //bilder.setrout(m);
            //Console.WriteLine("Количество связей: " + StandartFunctions.GetNodeDegree(m[0]));
            //bad = new SpyObject[50];
            //for (int i = 0
            //; (i <= 49); i++
            //)
            //{
            //    bad[i] = m[i].CreateSpyObject(new CoreName("bad"), SpyObjectType.Var);
            //}
            //Defence = new SpyObject[50];
            //for (int i = 0
            //; (i <= 49); i++
            //)
            //{
            //    Defence[i] = m[i].CreateSpyObject(new CoreName("Defence"), SpyObjectType.Var);
            //}
            //IC2.RegisterSpyObjects(bad, Defence);

            //m.DoSimulate(IC2);

            /////////////////////////
            //IC2 = new ic(24);
            //ns = StandartFunctions.GetAdjacentNodes(m[0]);
            //count = 0;
            //for (int i = 1; i < 50; i++)
            //{
            //    if (!ns.In(m[i]) && count < 5)
            //    {
            //        count++;
            //        m.AddEdge(m[0].Name, m[0][i].Name, m[i].Name, m[i][0].Name);
            //    }
            //}
            //bilder.setrout(m);
            //Console.WriteLine("Количество связей: " + StandartFunctions.GetNodeDegree(m[0]));
            //bad = new SpyObject[50];
            //for (int i = 0
            //; (i <= 49); i++
            //)
            //{
            //    bad[i] = m[i].CreateSpyObject(new CoreName("bad"), SpyObjectType.Var);
            //}
            //Defence = new SpyObject[50];
            //for (int i = 0
            //; (i <= 49); i++
            //)
            //{
            //    Defence[i] = m[i].CreateSpyObject(new CoreName("Defence"), SpyObjectType.Var);
            //}
            //IC2.RegisterSpyObjects(bad, Defence);

            //m.DoSimulate(IC2);
            D d = new D();
            d.Build();
        }
    }
}
