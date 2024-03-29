using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;

using TriadCompiler.Parser.Common.Declaration.Var;
using TriadCompiler.Parser.Common.Header;
using TriadCompiler.Parser.Common.Statement;
using TriadCompiler.Parser.InfProcedure.Header;

namespace TriadCompiler
{
    internal partial class InfProcedureParser : CommonParser
    {

        /// <summary>
        ///  ��������� ����
        /// </summary>
        private IProcedureCodeBuilder codeBuilder
        {
            get
            {
                return Fabric.Instance.Builder as IProcedureCodeBuilder;
            }
        }


        /// <summary>
        /// ������ ������ � ��������� ����
        /// </summary>
        /// <param name="endKey">��������� ���������� �������� ��������</param>
        public override void Compile(EndKeyList endKey)
        {
            if (!(Fabric.Instance.Builder is IProcedureCodeBuilder))
                throw new InvalidOperationException("������������ ��������� ����");

            this.codeBuilder.SetBaseClass(Builder.IProcedure.BaseClass);

            GetNextKey();
            designTypeInfo = new IPInfo();
            IProcedure(endKey);
        }


        /// <summary>
        /// �������������� ���������
        /// </summary>
        /// <syntax>InfProcedure HeaderName # IPHeader # # InitialPart # 
        /// # ProcessingPart # # EventPart # EndInf</syntax>
        /// <param name="endKey">��������� ���������� �������� ��������</param>
        private void IProcedure(EndKeyList endKey)
        {
            if (currKey != Key.IProcedure)
            {
                err.Register(Err.Parser.WrongStartSymbol.IProcedure, Key.IProcedure);
                SkipTo(endKey.UniteWith(Key.IProcedure));
            }
            if (currKey == Key.IProcedure)
            {
                Accept(Key.IProcedure);

                //��� �������������� ���������
                IProcedureType ipType = new IProcedureType("");

                //��� �������������� ���������
                HeaderName.Parse(endKey.UniteWith(InfHeader.StartKeys).UniteWith(Key.Colon, Key.Initial,
                    Key.Handling, Key.Processing, Key.EndInf),
                    delegate (string headerName)
                        {
                            ipType = new IProcedureType(headerName);
                            CommonArea.Instance.Register(ipType);
                        });

                //����������� ����� ������� ����� ��
                varArea.AddArea();
                //������������ ����������� �������
                RegisterStandardFuntions();

                this.designTypeName = ipType.Name;
                codeBuilder.SetClassName(this.designTypeName);

                //���������
                InfHeader.Parse(endKey.UniteWith(Key.Colon, Key.Initial, Key.Handling, Key.Processing, Key.EndInf),
                    ipType);

                //���� ��������� �������������� �������
                if (currKey == Key.Colon)
                {
                    Accept(Key.Colon);
                    IExprType returnType = TypeDeclaration.SimpleType(endKey.UniteWith(Key.Initial, Key.Processing,
                        Key.Handling, Key.EndInf));
                    ipType.ReturnCode = returnType.Code;

                    codeBuilder.SetIPResultType(returnType);
                }

                //������ �������������
                if (currKey == Key.Initial)
                {
                    InitialPart(endKey.UniteWith(Key.Handling, Key.Processing, Key.EndInf));
                }

                //������ ���������
                if (currKey == Key.Handling)
                {
                    Handling(endKey.UniteWith(Key.Processing, Key.EndInf));
                }

                //������ ������ ����������
                if (currKey == Key.Processing)
                {
                    Processing(endKey.UniteWith(Key.EndInf), ipType);
                }

                varArea.RemoveArea();

                Accept(Key.EndInf);

                //by jum
                (designTypeInfo as IPInfo).IPType = ipType;

                if (!endKey.Contains(currKey))
                {
                    Fabric.Instance.ErrReg.Register(Err.Parser.WrongEndSymbol.IProcedure, endKey.GetLastKeys());
                    SkipTo(endKey);
                }
            }
        }


        /// <summary>
        /// ��������� ��� �� � ���������
        /// </summary>
        /// <syntax>Identificator</syntax>
        /// <param name="endKeys">��������� ���������� �������� ��������</param>
        /// <returns>��� ��</returns>
        private IProcedureType IPHeaderName(EndKeyList endKeys)
        {
            //��� ��
            IProcedureType ipType = null;

            if (currKey != Key.Identificator)
            {
                Fabric.Instance.ErrReg.Register(Err.Parser.WrongStartSymbol.HeaderName, Key.Identificator);
                SkipTo(endKeys.UniteWith(Key.Identificator));
            }
            if (currKey == Key.Identificator)
            {
                ipType.Name = (currSymbol as IdentSymbol).Name;

                CommonArea.Instance.Register(ipType);

                Accept(Key.Identificator);

                if (!endKeys.Contains(currKey))
                {
                    Fabric.Instance.ErrReg.Register(Err.Parser.WrongEndSymbol.HeaderName, endKeys.GetLastKeys());
                    SkipTo(endKeys);
                }
            }
            return ipType;
        }


        /// <summary>
        /// ��������� �������
        /// </summary>
        /// <syntax>Initial StatementList EndInitial</syntax>
        /// <param name="endKey">��������� ���������� �������� ��������</param>
        protected void InitialPart(EndKeyList endKey)
        {
            Accept(Key.Initial);
            codeBuilder.SetInitialSection(StatementList.Parse(endKey.UniteWith(Key.EndInitial), StatementContext.Initial));
            Accept(Key.EndInitial);

            if (!endKey.Contains(currKey))
            {
                err.Register(Err.Parser.WrongEndSymbol.InitialPart, endKey.GetLastKeys());
                SkipTo(endKey);
            }
        }


        /// <summary>
        /// ������ �������������� ���������
        /// </summary>
        /// <syntax>Processing StatementList EndP</syntax>
        /// <param name="endKey">��������� ���������� �������� ��������</param>
        /// <param name="ipType">��� ��</param>
        private void Processing(EndKeyList endKey, IProcedureType ipType)
        {
            Accept(Key.Processing);

            CommonArea.Instance.AddArea();

            //������������ � ����� ������� ��������� ���������� � ������ �������
            CommonArea.Instance.Register(new VarType(ipType.ReturnCode, ipType.Name));

            codeBuilder.SetDoProcessing(StatementList.Parse(endKey.UniteWith(Key.EndProcessing), StatementContext.Common));

            CommonArea.Instance.RemoveArea();

            Accept(Key.EndProcessing);

            if (!endKey.Contains(currKey))
            {
                err.Register(Err.Parser.WrongEndSymbol.Processing);
                SkipTo(endKey);
            }
        }


        /// <summary>
        /// ������ ���������
        /// </summary>
        /// <syntax>Handling StatementList EndH</syntax>
        /// <param name="endKey"></param>
        private void Handling(EndKeyList endKey)
        {
            Accept(Key.Handling);

            CommonArea.Instance.AddArea();

            //������������ � ����� ������� ��������� ���������� � ������ ���������
            CommonArea.Instance.Register(new VarType(TypeCode.String, Builder.Routine.Receive.ReceivedMessage));
            //������������ � ����� ������� ��������� ���������� � ��������� ��������
            CommonArea.Instance.Register(new VarType(TypeCode.Real, Builder.Routine.SystemTime));

            codeBuilder.SetDoHandling(StatementList.Parse(endKey.UniteWith(Key.EndHandling), StatementContext.Handling));

            CommonArea.Instance.RemoveArea();

            Accept(Key.EndHandling);

            if (!endKey.Contains(currKey))
            {
                err.Register(Err.Parser.WrongEndSymbol.Handling);
                SkipTo(endKey);
            }
        }
    }

    public class IPInfo : DesignTypeInfo
    {
        public IProcedureType IPType = null;
    }

}
