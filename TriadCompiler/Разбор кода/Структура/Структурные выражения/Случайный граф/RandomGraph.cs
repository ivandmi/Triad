using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;

using TriadCompiler.Parser.Structure.StructExpr.Node;
using TriadCompiler.Parser.Common.Expr;
using TriadCompiler.Parser.Common.Statement;
using TriadCompiler.Parser.Common.Function;

namespace TriadCompiler.Parser.Structure.StructExpr.RandGraph
{
    class RandomGraph:CommonParser
    {
        public static CodeStatementCollection Parse(EndKeyList endKeys,string pushName)
        {
            CodeStatementCollection resultStatList = new CodeStatementCollection();

            //разбор параметров
            List<ExprInfo> parsInfo = ParameterList(endKeys);

            if (currKey!=Key.LeftPar)
            {
                //регистрация ошибки, добавить в список
                Fabric.Instance.ErrReg.Register(Err.Parser.WrongStartSymbol.StructConstant, Key.LeftPar);
                SkipTo(endKeys.UniteWith(Key.LeftPar));
            }

            if(currKey==Key.LeftPar)
            {
                Accept(Key.LeftPar);

                //Создаем пустой граф
                CodeMethodInvokeExpression createRandGraph = new CodeMethodInvokeExpression();
                createRandGraph.Method.TargetObject = new CodeThisReferenceExpression();

                resultStatList.Add(new CodeExpressionStatement(createRandGraph));
                createRandGraph.Method.MethodName = pushName;

                //объявление вершин
                resultStatList.AddRange(NodeDeclaration.Parse(endKeys.UniteWith(Key.Comma, Key.RightPar)));
                //добавление этих вершин в граф
                resultStatList.AddRange(StructExpression.ExpressionCode(Key.Plus));

                while(currKey==Key.Comma)
                {
                    Accept(Key.Comma);
                    //объявление вершин
                    resultStatList.AddRange(NodeDeclaration.Parse(endKeys.UniteWith(Key.Comma, Key.RightPar)));
                    //добавление этих вершин в граф
                    resultStatList.AddRange(StructExpression.ExpressionCode(Key.Plus));
                }
                Accept(Key.RightPar);
                
                //Вызов метода CompleteGraph
                CodeMethodInvokeExpression completeGraph = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(new CodeThisReferenceExpression(),
                    Builder.Structure.BuildExpr.Stack.First),
                    Builder.Structure.BuildExpr.DeclareOperation.Complete);
                //Добавление параметров
                foreach(ExprInfo p in parsInfo)
                    completeGraph.Parameters.Add(p.Code);

                resultStatList.Add(new CodeExpressionStatement(completeGraph));
            }
            return resultStatList;
        }

        public static List<ExprInfo> ParameterList(EndKeyList endKeys)
        {
            List<ExprInfo> paramList = new List<ExprInfo>();

            if(currKey!=Key.LeftPar)
            {
                Fabric.Instance.ErrReg.Register(Err.Parser.WrongStartSymbol.FunctionParameterList, Key.LeftPar);
                SkipTo(endKeys.UniteWith(Key.LeftPar));
            }

            if(currKey==Key.LeftPar)
            {
                Accept(Key.LeftPar);
                //проверка на пустой список параметров
                if(currKey!=Key.RightPar)
                {
                    ExprInfo exprInfo = Expression.Parse(endKeys.UniteWith(Key.RightPar, Key.Comma));

                    //должна быть проверка типов
                    paramList.Add(exprInfo);
                    while(currKey==Key.Comma)
                    {
                        Accept(Key.Comma);
                        exprInfo= Expression.Parse(endKeys.UniteWith(Key.RightPar, Key.Comma));
                        //должна быть проверка типов
                        paramList.Add(exprInfo);
                    }
                }
                //нужна проверка что все параметры написаны
                Accept(Key.RightPar);
                //проверка что конечный символ
            }
            return paramList;
        }
    }
}
