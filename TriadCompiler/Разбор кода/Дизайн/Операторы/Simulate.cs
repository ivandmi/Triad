using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;

using TriadCompiler.Parser.Common.Function;
using TriadCompiler.Parser.Common.Statement;
using TriadCompiler.Parser.Common.Expr;
using TriadCompiler.Parser.Model.DesignVar;
using TriadCompiler.Parser.Common.ObjectRef;
using TriadCompiler.Parser.SimCondition.Statement;
using TriadCompiler.Parser.Common.Var;
using TriadCompiler.Parser.Common.Polus;
using TriadCompiler.Parser.Common.Ev;

namespace TriadCompiler.Parser.Design.Statement
{
    /// <summary>
    /// ������ ��������� simulate
    /// </summary>
    internal class Simulate : CommonParser
    {
        /// <summary>
        /// ���������� ����� ������ ��
        /// </summary>
        private static int icCallNumber = 0;
        /// <summary>
        /// ��� ������� ������
        /// </summary>
        public static string modelName = string.Empty; //?????


        /// <summary>
        ///  ��������� ����
        /// </summary>
        private static GraphCodeBuilder codeBuilder
        {
            get
            {
                return Fabric.Instance.Builder as GraphCodeBuilder;
            }
        }


        /// <summary>
        /// ������ ���������
        /// </summary>
        /// <param name="endKeys">��������� �������� ��������</param> 
        public static CodeStatementCollection Parse(EndKeyList endKeys)
        {
            CodeStatementCollection Code = new CodeStatementCollection();

            Accept(Key.Simulate);

            //��� ������
            modelName = DesignVariable.Parse(endKeys.UniteWith(Key.On), DesignTypeCode.Model).StrCode;

            //������� ������� �������������
            if (currKey == Key.On)
            {
                GetNextKey();

                while (true)
                {
                    IPCallInfo icCallInfo = new IPCallInfo();
                    icCallInfo.Type = null;
                    //��� ������� �������������
                    string icName = (currSymbol as IdentSymbol).Name;
                    //�������� ��� �� �� �����
                    icCallInfo.Type = CommonArea.Instance.GetType<IConditionType>(icName);
                    //���������� ����� ��
                    icCallInfo.ipCallNumber = icCallNumber;

                    Accept(Key.Identificator);

                    //������ ���������� 
                    List<ExprInfo> paramList = new List<ExprInfo>();
                    //�������� ������ ����������
                    if (currKey == Key.LeftBracket)
                    {
                        paramList.AddRange(FunctionInvoke.ParameterList(endKeys.UniteWith(Key.LeftPar, Key.LeftFigurePar, Key.Colon, Key.Comma),
                            icCallInfo.Type.ParamVarList, Key.LeftBracket, Key.RightBracket));
                    }
                    //���� ��������� �� �������, � ��� ������ ����
                    else if (icCallInfo.Type != null && icCallInfo.Type.ParamVarList.ParameterCount > 0)
                    {
                        Fabric.Instance.ErrReg.Register(Err.Parser.Usage.ParameterList.NotEnoughParameters);
                    }

                    //������� �����, ����������� ��
                    icCallInfo.Code.AddRange(IPCall.GenerateIProcedureCreation(icName, icCallNumber, paramList));

                    //������ spy-��������
                    icCallInfo.Code.AddRange(SpyParametrList(endKeys.UniteWith(Key.LeftFigurePar, Key.Colon, Key.Comma),
                        icCallInfo));

                    //���������� �����, ���������������� �� - �� ����!!!
                    //icCallInfo.Code.AddRange( IPCall.GenerateIProcedureInitialization( icCallNumber ) );

                    //��������� ����� doSimulate
                    icCallInfo.Code.Add(GenerateSimulateMethod(modelName, icCallNumber));

                    //������ Out-����������
                    if (currKey == Key.LeftFigurePar)
                    {
                        icCallInfo.Code.Add(IPCall.OutVarList(endKeys.UniteWith(Key.Colon, Key.Comma), icCallInfo));
                    }

                    icCallNumber++;
                    Code.AddRange(icCallInfo.Code);

                    if (currKey == Key.Comma)
                        GetNextKey();
                    else
                        break;
                }
            }
            return Code;
        }


        /// <summary>
        /// ������������� �����, ����������
        /// </summary>
        /// <param name="modelName">��� ������</param>
        /// <param name="icCallNumber">���������� ����� ��</param>
        /// <returns>���</returns>
        private static CodeStatement GenerateSimulateMethod(string modelName, int icCallNumber)
        {
            //�����, ���������� ������� �������������
            CodeMethodInvokeExpression simulateStat = new CodeMethodInvokeExpression();
            simulateStat.Method = new CodeMethodReferenceExpression();
            simulateStat.Method.MethodName = Builder.Design.DoSimulate;
            simulateStat.Method.TargetObject = new CodeVariableReferenceExpression(modelName);

            //�����, ����������� ��������� ��
            CodeMethodInvokeExpression getICMethod = new CodeMethodInvokeExpression();
            getICMethod.Method.MethodName = Builder.Design.GetICondition;
            getICMethod.Parameters.Add(new CodePrimitiveExpression(icCallNumber));
            simulateStat.Parameters.Add(getICMethod);

            return new CodeExpressionStatement(simulateStat);
        }


        /// <summary>
        /// ���� spy-������
        /// </summary>
        /// <syntax>Variable | PolusVar | EventVar</syntax>
        /// <param name="endKeys">���������� �������� �������</param>
        /// <param name="enumerator">��� ����������� ���������</param>
        /// <returns>��� ������, ������������ ���� spy-������</returns>
        public static CodeMethodInvokeExpression SingleSpyObject(EndKeyList endKeys, IEnumerator<ISpyType> enumerator)
        {
            CodeMethodInvokeExpression getSpyObjectStat = new CodeMethodInvokeExpression();
            getSpyObjectStat.Method = new CodeMethodReferenceExpression();

            ISpyType spyFormalType = enumerator.Current;

            if (spyFormalType != null)
            {
                //��� �������
                ObjectRefInfo nodeInfo = ObjectReference.Parse(endKeys.UniteWith(Key.Point), false);

                Accept(Key.Point);

                ObjectRefInfo spyObjectInfo;
                //������ �� ������ ������ �������
                spyObjectInfo = ObjectReference.Parse(endKeys, true);

                //���� ������ ��������� ������ ��������� ������
                if (enumerator.Current is IndexedType && !spyObjectInfo.IsRange)
                {
                    Fabric.Instance.ErrReg.Register(Err.Parser.Usage.NeedRange);
                }
                else if (!(enumerator.Current is IndexedType) && spyObjectInfo.IsRange)
                {
                    Fabric.Instance.ErrReg.Register(Err.Parser.Usage.NeedNotRange);
                }

                //���������� ��������� � ������� ����� ������ ������
                getSpyObjectStat.Method.MethodName = Builder.Design.CreateSpyObject;

                CodeExpression objExpr = null;
                if (modelName.Length == 0) // ???????????
                {
                    objExpr = new CodeVariableReferenceExpression(nodeInfo.Name);
                }
                else
                {
                    CodeIndexerExpression objectIndex = new CodeIndexerExpression();
                    objectIndex.TargetObject = new CodeVariableReferenceExpression(modelName);
                    objectIndex.Indices.Add(nodeInfo.CoreNameCode);
                    objExpr = objectIndex;
                }

                getSpyObjectStat.Method.TargetObject = objExpr;

                //������� ������ �������� (����������� ������)
                getSpyObjectStat.Parameters.Add(spyObjectInfo.CoreNameCode);

                //������� ������ �������� (��� ������������ �������)
                if (spyFormalType is IExprType)
                {
                    getSpyObjectStat.Parameters.Add(new CodeSnippetExpression(Builder.Design.SpyObjectType + ".Var"));
                }
                else if (spyFormalType is IPolusType)
                {
                    getSpyObjectStat.Parameters.Add(new CodeSnippetExpression(Builder.Design.SpyObjectType + ".Polus"));
                }
                else if (spyFormalType is EventType)
                {
                    getSpyObjectStat.Parameters.Add(new CodeSnippetExpression(Builder.Design.SpyObjectType + ".Event"));
                }

                enumerator.MoveNext();
            }
            //������� �������� ��������� �� ����
            else
            {
                Fabric.Instance.ErrReg.Register(Err.Parser.Usage.ParameterList.TooManyParameters);
            }

            return getSpyObjectStat;
        }

        public static CodeStatementCollection SpyParametrList(EndKeyList endKeys, IPCallInfo ipCallInfo)
        {
            //��� ���������� ��������. �� �������� ������
            CodeStatementCollection statList = new CodeStatementCollection();
            
            CodeMethodInvokeExpression registerSpyObjectsStat = new CodeMethodInvokeExpression();
            registerSpyObjectsStat.Method = new CodeMethodReferenceExpression();
            registerSpyObjectsStat.Method.MethodName = Builder.IProcedure.RegisterAllSpyObjects;
            CodeSnippetExpression cse = new CodeSnippetExpression("((" + ipCallInfo.Type.Name + ")" +
                Builder.ICondition.GetIProcedure + "(" + ipCallInfo.ipCallNumber.ToString() + "))");

            registerSpyObjectsStat.Method.TargetObject = cse;

            if (currKey != Key.LeftPar)
            {
                Fabric.Instance.ErrReg.Register(Err.Parser.WrongStartSymbol.FunctionParameterList, Key.LeftPar);
                SkipTo(endKeys.UniteWith(Key.LeftPar));
            }
            if (currKey == Key.LeftPar)
            {
                GetNextKey();

                //������� ����������
                IEnumerator<ISpyType> paramEnumerator = ipCallInfo.Type.GetEnumerator();
                paramEnumerator.MoveNext();

                if (currKey != Key.RightPar)
                {
                    registerSpyObjectsStat.Parameters.Add(mysingle(endKeys.UniteWith(Key.RightPar, Key.Comma), paramEnumerator, statList));

                    while (currKey == Key.Comma)
                    {
                        GetNextKey();
                        registerSpyObjectsStat.Parameters.Add(mysingle(endKeys.UniteWith(Key.RightPar, Key.Comma), paramEnumerator, statList));
                    }
                }

                Accept(Key.RightPar);
                //���� ���� ������� �� ��� ���������
                if (paramEnumerator.Current != null)
                {
                    Fabric.Instance.ErrReg.Register(Err.Parser.Usage.ParameterList.NotEnoughParameters);
                }

                if (!endKeys.Contains(currKey))
                {
                    Fabric.Instance.ErrReg.Register(Err.Parser.WrongEndSymbol.FunctionParameterList, endKeys.GetLastKeys());
                    SkipTo(endKeys);
                }
            }
            statList.Add(registerSpyObjectsStat);
            return statList;
        }

        private static CodeVariableReferenceExpression mysingle(EndKeyList endKeys, IEnumerator<ISpyType> enumerator,CodeStatementCollection statList)
        {
            CodeMethodInvokeExpression createSpyObj = new CodeMethodInvokeExpression();
            CodeExpression res;
            createSpyObj.Method = new CodeMethodReferenceExpression();   
            if (currKey != Key.Identificator)
            {
                Fabric.Instance.ErrReg.Register(Err.Parser.WrongStartSymbol.SpyObject, Key.Identificator);
                SkipTo(endKeys.UniteWith(Key.Identificator));
            }

            ObjectRefInfo spyObjectInfo=null;
            if (currKey == Key.Identificator)
            {
                ISpyType spyFormalType = enumerator.Current;
                if (spyFormalType != null)
                {
                    //��� �������
                    ObjectRefInfo nodeInfo = ObjectReference.Parse(endKeys.UniteWith(Key.Point), true);

                    Accept(Key.Point);
                   
                    
                    //������ �� ������ ������ �������
                    spyObjectInfo = ObjectReference.Parse(endKeys, true);

                    ISpyType spyOjectType = null;

                    createSpyObj.Method.MethodName = Builder.Design.CreateSpyObject;

                    CodeExpression objExpr = null;
                    if (modelName.Length == 0) // ???????????
                    {
                        objExpr = new CodeVariableReferenceExpression(nodeInfo.Name);
                    }
                    else
                    {
                        CodeIndexerExpression objectIndex = new CodeIndexerExpression();
                        objectIndex.TargetObject = new CodeVariableReferenceExpression(modelName);
                        if (nodeInfo.IsRange)
                            objectIndex.Indices.Add(new CodeVariableReferenceExpression("i"));
                        else
                            objectIndex.Indices.Add(nodeInfo.CoreNameCode);
                        objExpr = objectIndex;
                    }
                    createSpyObj.Method.TargetObject = objExpr;

                    if (spyFormalType is IExprType)
                    {
                        //VarInfo varInfo = Variable.Parse(endKeys, /*Allow range*/ true,
                        //    /*Allow not indexed array*/ false);
                        //Assignement.CheckVarTypes(spyFormalType as IExprType, varInfo.Type);
                        //spyOjectType = varInfo.Type;
                        //��������
                        createSpyObj.Parameters.Add(spyObjectInfo.CoreNameCode);
                        createSpyObj.Parameters.Add(new CodeSnippetExpression(Builder.Design.SpyObjectType + ".Var"));
                    }
                    //���� ��� ������ ���� �����
                    else if (spyFormalType is IPolusType)
                    {
                        //PolusInfo polusInfo = PolusVar.Parse(endKeys);
                        //Assignement.CheckPolusTypes(spyFormalType as IPolusType, polusInfo.Type);
                        //spyOjectType = polusInfo.Type;

                        //��������
                        createSpyObj.Parameters.Add(spyObjectInfo.CoreNameCode);
                        createSpyObj.Parameters.Add(new CodeSnippetExpression(Builder.Design.SpyObjectType + ".Polus"));
                    }
                    //���� ��� ������ ���� �������
                    else if (spyFormalType is EventType)
                    {
                        //EventInfo eventInfo = EventVar.Parse(endKeys, /*Check registration*/ true);
                        //spyOjectType = eventInfo.Type;

                        //������� ��������
                        createSpyObj.Parameters.Add(spyObjectInfo.CoreNameCode);
                        createSpyObj.Parameters.Add(new CodeSnippetExpression(Builder.Design.SpyObjectType + ".Event"));
                    }

                    //���������, ��� spy-������?
                    if (spyOjectType != null)
                        if (spyOjectType != null && !spyOjectType.IsSpyObject)
                        {
                            Fabric.Instance.ErrReg.Register(Err.Parser.Usage.NeedSpyObject);
                        }
                    enumerator.MoveNext();

                    if (nodeInfo.IsRange)
                    {
                        CodeArrayCreateExpression ca = new CodeArrayCreateExpression("SpyObject",int.Parse(nodeInfo.IndexBoundArray[0].topIndexExpr.StrCode)+1);
                        CodeVariableDeclarationStatement st = new CodeVariableDeclarationStatement("SpyObject[]", spyObjectInfo.Name, ca);
                        statList.Add(st);
                        CodeIterationStatement codefor = new CodeIterationStatement();
                        codefor.InitStatement = new CodeSnippetStatement("int i=" + nodeInfo.IndexBoundArray[0].lowIndexExpr.StrCode);
                        codefor.IncrementStatement = new CodeSnippetStatement("i++");
                        CodeBinaryOperatorExpression exp = new CodeBinaryOperatorExpression();
                        exp.Left = new CodeSnippetExpression("i");
                        exp.Right = nodeInfo.IndexBoundArray[0].topIndexExpr.Code;
                        exp.Operator = CodeBinaryOperatorType.LessThanOrEqual;
                        codefor.TestExpression = exp;
                        CodeAssignStatement assign = new CodeAssignStatement();
                        assign.Left = new CodeIndexerExpression(new CodeVariableReferenceExpression(spyObjectInfo.Name),
                            new CodeSnippetExpression("i"));
                        assign.Right = createSpyObj;
                        codefor.Statements.Add(assign);
                        statList.Add(codefor);
                    }
                    else
                    {
                        CodeVariableDeclarationStatement c;
                        if (spyObjectInfo.IsRange)
                            c= new CodeVariableDeclarationStatement("SpyObject[]", spyObjectInfo.Name);
                        else
                            c= new CodeVariableDeclarationStatement("SpyObject", spyObjectInfo.Name);//new CodeVariableReferenceExpression(spyObjectInfo.Name);
                        c.InitExpression = createSpyObj;
                        statList.Add(c);
                    }
                }
                //������� �������� ��������� �� ����
                else
                {
                    Fabric.Instance.ErrReg.Register(Err.Parser.Usage.ParameterList.TooManyParameters);
                }
            }
            return new CodeVariableReferenceExpression(spyObjectInfo.Name);
        }

    }
}
