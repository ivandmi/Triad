using System;
using System.Collections.Generic;
using System.CodeDom;

using TriadCompiler.Parser.Common.Statement;

namespace TriadCompiler
{
    /// <summary>
    /// ��� ����� ������ CommonParser, ���������� �� ������ ����������.
    /// </summary>
    public partial class CommonParser
    {
        /// <summary>
        /// ���������� ��������
        /// </summary>
        /// <param name="endKeys">��������� �������� ��������</param>
        /// <param name="context">������� ��������</param>
        /// <returns>������������� ��� ��������� ����</returns>
        public virtual CodeStatementCollection Statement(EndKeyList endKeys, StatementContext context)
        {
            return new CodeStatementCollection();
        }


        /// <summary>
        /// ���������������� ����������� �������
        /// </summary>
        protected void RegisterStandardFuntions()
        {
            RegisterConvertionFunctions();
            RegisterRandomFunctions();
            RegisterMathFunctions();
            RegisterSetFunctions();
            //====By Jum======
            RegisteGraphFunction();

            //������� ��������� ������
            //RegistreRandomGraphFunc();
        }


        /// <summary>
        /// ���������������� ������� �������������� �����
        /// </summary>
        private void RegisterConvertionFunctions()
        {
            //StrToInt
            FunctionType function = new FunctionType();
            function.Name = "StrToInt";
            function.MethodCodeName = "Convertion.StrToInt";
            function.AddParameter(new VarType(TypeCode.String));
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);

            //IntToStr
            function = new FunctionType();
            function.Name = "IntToStr";
            function.MethodCodeName = "Convertion.IntToStr";
            function.AddParameter(new VarType(TypeCode.Integer));
            function.ReturnedType = new VarType(TypeCode.String);
            CommonArea.Instance.Register(function);

            //StrToReal
            function = new FunctionType();
            function.Name = "StrToReal";
            function.MethodCodeName = "Convertion.StrToReal";
            function.AddParameter(new VarType(TypeCode.String));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //RealToStr
            function = new FunctionType();
            function.Name = "RealToStr";
            function.MethodCodeName = "Convertion.RealToStr";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.String);
            CommonArea.Instance.Register(function);

            //StrToBoolean
            function = new FunctionType();
            function.Name = "StrToBoolean";
            function.MethodCodeName = "Convertion.StrToBoolean";
            function.AddParameter(new VarType(TypeCode.String));
            function.ReturnedType = new VarType(TypeCode.Boolean);
            CommonArea.Instance.Register(function);

            //BooleanToStr
            function = new FunctionType();
            function.Name = "BooleanToStr";
            function.MethodCodeName = "Convertion.BooleanToStr";
            function.AddParameter(new VarType(TypeCode.Boolean));
            function.ReturnedType = new VarType(TypeCode.String);
            CommonArea.Instance.Register(function);

            //StrToBit
            function = new FunctionType();
            function.Name = "StrToBit";
            function.MethodCodeName = "Convertion.StrToBit";
            function.AddParameter(new VarType(TypeCode.String));
            function.ReturnedType = new VarType(TypeCode.Bit);
            CommonArea.Instance.Register(function);

            //BitToStr
            function = new FunctionType();
            function.Name = "BitToStr";
            function.MethodCodeName = "Convertion.BitToStr";
            function.AddParameter(new VarType(TypeCode.Bit));
            function.ReturnedType = new VarType(TypeCode.String);
            CommonArea.Instance.Register(function);

            //StrToChar
            function = new FunctionType();
            function.Name = "StrToChar";
            function.MethodCodeName = "Convertion.StrToChar";
            function.AddParameter(new VarType(TypeCode.String));
            function.ReturnedType = new VarType(TypeCode.Char);
            CommonArea.Instance.Register(function);

            //CharToStr
            function = new FunctionType();
            function.Name = "CharToStr";
            function.MethodCodeName = "Convertion.CharToStr";
            function.AddParameter(new VarType(TypeCode.Char));
            function.ReturnedType = new VarType(TypeCode.String);
            CommonArea.Instance.Register(function);

            //StrToCharArray
            function = new FunctionType();
            function.Name = "StrToCharArray";
            function.MethodCodeName = "Convertion.StrToCharArray";
            function.AddParameter(new VarType(TypeCode.String));
            VarArrayType arrayType = new VarArrayType(TypeCode.Char);
            arrayType.SetNewIndex(int.MaxValue);
            function.ReturnedType = arrayType;
            CommonArea.Instance.Register(function);

            //CharArrayToStr
            function = new FunctionType();
            function.Name = "CharArrayToStr";
            function.MethodCodeName = "Convertion.CharArrayToStr";
            function.AddParameter(arrayType);
            function.ReturnedType = new VarType(TypeCode.String);
            CommonArea.Instance.Register(function);

            //Split
            function = new FunctionType();
            function.Name = "Split";
            function.MethodCodeName = "Convertion.Split";
            function.AddParameter(new VarType(TypeCode.String));
            function.AddParameter(new VarType(TypeCode.Char));
            arrayType = new VarArrayType(TypeCode.String);
            arrayType.SetNewIndex(int.MaxValue);
            function.ReturnedType = arrayType;
            CommonArea.Instance.Register(function);

            //StrContains
            function = new FunctionType();
            function.Name = "StrContains";
            function.MethodCodeName = "Convertion.StrContains";
            function.AddParameter(new VarType(TypeCode.String));
            function.AddParameter(new VarType(TypeCode.String));
            function.ReturnedType = new VarType(TypeCode.Boolean);
            CommonArea.Instance.Register(function);

            //ToStr
            function = new FunctionType();
            function.Name = "ToStr";
            function.MethodCodeName = "Convertion.ToStr";
            function.AddParameter(new VarType(TypeCode.UndefinedType));
            function.ReturnedType = new VarType(TypeCode.String);
            CommonArea.Instance.Register(function);

            //IntArrayToStr
            function = new FunctionType();
            function.Name = "IntArrayToStr";
            function.MethodCodeName = "Convertion.IntArrayToStr";
            arrayType = new VarArrayType(TypeCode.Integer);
            arrayType.SetNewIndex(int.MaxValue);
            function.AddParameter(arrayType);
            function.ReturnedType = new VarType(TypeCode.String);
            CommonArea.Instance.Register(function);

            //StrToIntArray
            function = new FunctionType();
            function.Name = "StrToIntArray";
            function.MethodCodeName = "Convertion.StrToIntArray";
            function.AddParameter(new VarType(TypeCode.String));
            function.ReturnedType = arrayType;
            CommonArea.Instance.Register(function);

            //RealArrayToStr
            function = new FunctionType();
            function.Name = "RealArrayToStr";
            function.MethodCodeName = "Convertion.RealArrayToStr";
            arrayType = new VarArrayType(TypeCode.Real);
            arrayType.SetNewIndex(int.MaxValue);
            function.AddParameter(arrayType);
            function.ReturnedType = new VarType(TypeCode.String);
            CommonArea.Instance.Register(function);

            //StrToRealArray
            function = new FunctionType();
            function.Name = "StrToRealArray";
            function.MethodCodeName = "Convertion.StrToRealArray";
            function.AddParameter(new VarType(TypeCode.String));
            function.ReturnedType = arrayType;
            CommonArea.Instance.Register(function);

            //Real2DArrayToStr
            function = new FunctionType();
            function.Name = "Real2DArrayToStr";
            function.MethodCodeName = "Convertion.Real2DArrayToStr";
            arrayType = new VarArrayType(TypeCode.Real);
            arrayType.SetNewIndex(int.MaxValue);
            arrayType.SetNewIndex(int.MaxValue);//second
            function.AddParameter(arrayType);
            function.ReturnedType = new VarType(TypeCode.String);
            CommonArea.Instance.Register(function);

            //StrToReal2DArray
            function = new FunctionType();
            function.Name = "StrToReal2DArray";
            function.MethodCodeName = "Convertion.StrToReal2DArray";
            function.AddParameter(new VarType(TypeCode.String));
            arrayType = new VarArrayType(TypeCode.Real);
            arrayType.SetNewIndex(int.MaxValue);
            arrayType.SetNewIndex(int.MaxValue);//second
            function.ReturnedType = arrayType;
            CommonArea.Instance.Register(function);
        }


        /// <summary>
        /// ���������������� ������� ��� ������ �� ���������� ����������
        /// </summary>
        private void RegisterRandomFunctions()
        {
            //Random
            FunctionType function = new FunctionType();
            function.Name = "Random";
            function.MethodCodeName = "Rand.Random";
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);

            //RandomIn
            function = new FunctionType();
            function.Name = "RandomIn";
            function.MethodCodeName = "Rand.RandomIn";
            function.AddParameter(new VarType(TypeCode.Integer));
            function.AddParameter(new VarType(TypeCode.Integer));
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);

            //RandomReal
            function = new FunctionType();
            function.Name = "RandomReal";
            function.MethodCodeName = "Rand.RandomReal";
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //RandomRealIn
            function = new FunctionType();
            function.Name = "RandomRealIn";
            function.MethodCodeName = "Rand.RandomRealIn";
            function.AddParameter(new VarType(TypeCode.Real));
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);
        }


        /// <summary>
        /// ���������������� �������������� �������
        /// </summary>
        private void RegisterMathFunctions()
        {
            //Round
            FunctionType function = new FunctionType();
            function.Name = "Round";
            function.MethodCodeName = "TMath.Round";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);

            //Sin
            function = new FunctionType();
            function.Name = "Sin";
            function.MethodCodeName = "TMath.Sin";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //Cos
            function = new FunctionType();
            function.Name = "Cos";
            function.MethodCodeName = "TMath.Cos";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //Tan
            function = new FunctionType();
            function.Name = "Tan";
            function.MethodCodeName = "TMath.Tan";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //Sign
            function = new FunctionType();
            function.Name = "Sign";
            function.MethodCodeName = "TMath.Sign";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);

            //Abs
            function = new FunctionType();
            function.Name = "Abs";
            function.MethodCodeName = "TMath.Abs";
            function.AddParameter(new VarType(TypeCode.Integer));
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);

            //AbsReal
            function = new FunctionType();
            function.Name = "AbsReal";
            function.MethodCodeName = "TMath.AbsReal";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //Ln
            function = new FunctionType();
            function.Name = "Ln";
            function.MethodCodeName = "TMath.Ln";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //Log
            function = new FunctionType();
            function.Name = "Log";
            function.MethodCodeName = "TMath.Log";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //Asin
            function = new FunctionType();
            function.Name = "Asin";
            function.MethodCodeName = "TMath.Asin";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //Acos
            function = new FunctionType();
            function.Name = "Acos";
            function.MethodCodeName = "TMath.Acos";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //Atan
            function = new FunctionType();
            function.Name = "Atan";
            function.MethodCodeName = "TMath.Atan";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //Exp
            function = new FunctionType();
            function.Name = "Exp";
            function.MethodCodeName = "TMath.Exp";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //Pow
            function = new FunctionType();
            function.Name = "Pow";
            function.MethodCodeName = "TMath.Pow";
            function.AddParameter(new VarType(TypeCode.Real));
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);

            //Sqrt
            function = new FunctionType();
            function.Name = "Sqrt";
            function.MethodCodeName = "TMath.Sqrt";
            function.AddParameter(new VarType(TypeCode.Real));
            function.ReturnedType = new VarType(TypeCode.Real);
            CommonArea.Instance.Register(function);
        }

        /// <summary>
        /// ���������������� ������� ��� ��-����
        /// </summary>
        private void RegisterSetFunctions()
        {
            //SetSize
            FunctionType function = new FunctionType();
            function.Name = "GetSetSize";
            function.MethodCodeName = "SetFunctions.GetSetSize";
            function.AddParameter(new SetType(TypeCode.UndefinedType));
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);
        }

        //==========By Jum===============
        /// <summary>
        /// ���������������� ������� ��� ������ � �������
        /// </summary>
        private void RegisteGraphFunction()
        {
            //����� ������ � �����
            FunctionType function = new FunctionType();
            function.Name = "GetNodeCount";
            function.MethodCodeName = "StandartFunctions.GetNodeCount";
            function.AddParameter(new DesignVarType(DesignTypeCode.Structure));
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);

            //�������� ���� ��� ����� ( �������� ���� �������� )
            function = new FunctionType();
            function.Name = "GetGraphWithoutRoutines";
            function.MethodCodeName = "StandartFunctions.GetGraphWithoutRoutines";
            function.AddParameter(new DesignVarType(DesignTypeCode.Structure));
            function.ReturnedType = new DesignVarType(DesignTypeCode.Structure);
            CommonArea.Instance.Register(function);

            //������� �������
            function = new FunctionType();
            function.Name = "GetNodeDegree";
            function.MethodCodeName = "StandartFunctions.GetNodeDegree";
            function.AddParameter(new VarType(TypeCode.Node));
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);

            //��������� ������� ������
            function = new FunctionType();
            function.Name = "GetAdjacentNodes";
            function.MethodCodeName = "StandartFunctions.GetAdjacentNodes";
            function.AddParameter(new VarType(TypeCode.Node));
            function.ReturnedType = new SetType(TypeCode.Node);
            CommonArea.Instance.Register(function);

            //������� ������� �� ������
            function = new FunctionType();
            function.Name = "GetNodeDegreeIn";
            function.MethodCodeName = "StandartFunctions.GetNodeDegreeIn";
            function.AddParameter(new VarType(TypeCode.Node));
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);

            //��������� ������� �� ����� ������
            function = new FunctionType();
            function.Name = "GetAdjacentNodesIn";
            function.MethodCodeName = "StandartFunctions.GetAdjacentNodesIn";
            function.AddParameter(new VarType(TypeCode.Node));
            function.ReturnedType = new SetType(TypeCode.Node);
            CommonArea.Instance.Register(function);

            //������� ������� �� �������
            function = new FunctionType();
            function.Name = "GetNodeDegreeOut";
            function.MethodCodeName = "StandartFunctions.GetNodeDegreeOut";
            function.AddParameter(new VarType(TypeCode.Node));
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);

            //��������� ������� �� ������ ������
            function = new FunctionType();
            function.Name = "GetAdjacentNodesOut";
            function.MethodCodeName = "StandartFunctions.GetAdjacentNodesOut";
            function.AddParameter(new VarType(TypeCode.Node));
            function.ReturnedType = new SetType(TypeCode.Node);
            CommonArea.Instance.Register(function);

            //������ ������� � �����
            function = new FunctionType();
            function.Name = "GetNodeIndex";
            function.MethodCodeName = "StandartFunctions.GetNodeIndex";
            function.AddParameter(new DesignVarType(DesignTypeCode.Structure));
            function.AddParameter(new VarType(TypeCode.Node));
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);

            //��������� ���� ������ �����
            function = new FunctionType();
            function.Name = "GetNodeNames";
            function.MethodCodeName = "StandartFunctions.GetNodeNames";
            function.AddParameter(new DesignVarType(DesignTypeCode.Structure));
            function.ReturnedType = new SetType(TypeCode.String);
            CommonArea.Instance.Register(function);

            function = new FunctionType();
            function.Name = "GetArrayOfNodeNames";
            function.MethodCodeName = "StandartFunctions.GetArrayOfNodeNames";
            function.AddParameter(new DesignVarType(DesignTypeCode.Structure));
            VarArrayType stringArrayType = new VarArrayType(TypeCode.String);
            stringArrayType.SetNewIndex(int.MaxValue);
            function.ReturnedType = stringArrayType;
            CommonArea.Instance.Register(function);

            //��������� ������ �����
            function = new FunctionType();
            function.Name = "GetNodeSet";
            function.MethodCodeName = "StandartFunctions.GetNodeSet";
            function.AddParameter(new DesignVarType(DesignTypeCode.Structure));
            function.ReturnedType = new SetType(TypeCode.Node);
            CommonArea.Instance.Register(function);

            //����� ����������� ���� ����� 2 ���������
            function = new FunctionType();
            function.Name = "FindShortestPath";
            function.MethodCodeName = "StandartFunctions.FindShortestPath";
            function.AddParameter(new DesignVarType(DesignTypeCode.Structure));
            function.AddParameter(new VarType(TypeCode.Node));
            function.AddParameter(new VarType(TypeCode.Node));
            function.ReturnedType = new SetType(TypeCode.Node);
            CommonArea.Instance.Register(function);

            //������ ������� ���������� �����
            function = new FunctionType();
            function.Name = "GetStronglyConnectedComponents";
            function.MethodCodeName = "StandartFunctions.GetStronglyConnectedComponents";
            function.AddParameter(new DesignVarType(DesignTypeCode.Structure));
            VarArrayType arrayType = new VarArrayType(TypeCode.Integer);
            arrayType.SetNewIndex(int.MaxValue);
            function.ReturnedType = arrayType;
            CommonArea.Instance.Register(function);

            //������� �����
            function = new FunctionType();
            function.Name = "GetGraphDiameter";
            function.MethodCodeName = "StandartFunctions.GetGraphDiameter";
            function.AddParameter(new DesignVarType(DesignTypeCode.Structure));
            function.ReturnedType = new VarType(TypeCode.Integer);
            CommonArea.Instance.Register(function);

            //�������� ������ �������
            function = new FunctionType();
            function.Name = "GetRoutine";
            function.MethodCodeName = "StandartFunctions.GetRoutine";
            function.AddParameter(new VarType(TypeCode.Node));
            function.ReturnedType = new DesignVarType(DesignTypeCode.Routine);
            CommonArea.Instance.Register(function);

            //��������� ����� �����
            function = new FunctionType();
            function.Name = "GetRoutines";
            function.MethodCodeName = "StandartFunctions.GetRoutines";
            function.AddParameter(new DesignVarType(DesignTypeCode.Structure));
            function.ReturnedType = new SetType(TypeCode.UndefinedType);
            CommonArea.Instance.Register(function);

            //��������� ��������� ������ �� ��������� ���� �����
            function = new FunctionType();
            function.Name = "GetNodes";
            function.MethodCodeName = "StandartFunctions.GetNodes";
            function.AddParameter(new DesignVarType(DesignTypeCode.Structure));
            function.AddParameter(new SetType(TypeCode.String));
            function.ReturnedType = new SetType(TypeCode.Node);
            CommonArea.Instance.Register(function);
        }

        //public void RegistreRandomGraphFunc()
        //{
        //    var function = new FunctionType();
        //    function.Name = "ERGraph";
        //    function.MethodCodeName = "PushEmptyErdosRenyiGraph";
        //    function.AddParameter(new VarType(TypeCode.Real));
        //    function.ReturnedType= new DesignVarType(DesignTypeCode.Structure);
        //    CommonArea.Instance.Register(function);

        //    function = new FunctionType();
        //    function.Name = "BOGraph";
        //    function.MethodCodeName = "PushEmptyBaklyOktusGraph";
        //    function.AddParameter(new VarType(TypeCode.Integer));
        //    function.AddParameter(new VarType(TypeCode.Integer));
        //    function.ReturnedType = new DesignVarType(DesignTypeCode.Structure);
        //    CommonArea.Instance.Register(function);

        //    function = new FunctionType();
        //    function.Name = "BRGraph";
        //    function.MethodCodeName = "PushEmptyBollRiordanGraph";
        //    function.AddParameter(new VarType(TypeCode.Integer));
        //    function.ReturnedType = new DesignVarType(DesignTypeCode.Structure);
        //    CommonArea.Instance.Register(function);

        //    function = new FunctionType();
        //    function.Name = "CopyGraph";
        //    function.MethodCodeName = "PushEmptyCopyGraph";
        //    function.AddParameter(new VarType(TypeCode.Integer));
        //    function.AddParameter(new VarType(TypeCode.Real));
        //    function.ReturnedType = new DesignVarType(DesignTypeCode.Structure);
        //    CommonArea.Instance.Register(function);
        //}

    }

}

