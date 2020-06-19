using System;
using System.Collections.Generic;
using System.Text;

namespace TriadCore
{
    enum PetriNetMessage
    {
        TRY_REQUEST,
        REQEST_ACCEPTED,
        MARK_SENT,
        MARK_RECIEVE
    }
    

    public class PetriNetPlace : Routine
    {

        private int InitialNumMarks;

        private Double T;

        private Int32 outarc;

        private string mess;

        List<CoreName> outputs;
        


        public PetriNetPlace(Int32 InitialNumMarks, Double T)
        {
            this.InitialNumMarks = InitialNumMarks;
            this.T = T;
        }

        public override void DoInitialize()
        {
            List<CoreName> names = new List<CoreName>();
            names.AddRange(routineNodePolusPairs.Keys);
            outputs = names.FindAll(x => x.BaseName == "Output");
            outarc = outputs.Count;
            DoVarChanging(new CoreName("outarc"));

            Schedule(0, this.TryQuery);
            PrintMessage("Инициализация вершины");
        }

        private void TryQuery()
        {
            if (InitialNumMarks > 0)
            {
                foreach(var polus in outputs)
                {
                    SendMessageVia(PetriNetMessage.TRY_REQUEST.ToString(), polus);
                    PrintMessage("Отправка запроса через полюс " + polus.ToString());
                    Schedule(T, this.TryQuery);
                }
                
            }
        }

        //private void Wait()
        //{
        //    if (curpol < outarc)
        //    {
        //        curpol = curpol + 1;
        //        DoVarChanging(new CoreName("curpol"));
        //        Schedule(0, this.TryQuery);
        //    }
        //    if (curpol == outarc)
        //    {
        //        curpol = 0;
        //        DoVarChanging(new CoreName("curpol"));
        //        Schedule(0, this.TryQuery);
        //    }
        //}

        protected override void ReceiveMessageVia(CoreName polusName, String message)
        {

            PetriNetMessage mess = (PetriNetMessage)Enum.Parse(typeof(PetriNetMessage), message);
            if (mess == PetriNetMessage.REQEST_ACCEPTED && InitialNumMarks>0)
            {
                PrintMessage("Ответ получен через полюс " + polusName.ToString());
                InitialNumMarks = InitialNumMarks - 1;
                DoVarChanging(new CoreName("InitialNumMarks"));

                SendMessageVia(PetriNetMessage.MARK_SENT.ToString(), polusName);
                PrintMessage("Отправляем фишку через полюс" + polusName.ToString());
            }
            if (mess == PetriNetMessage.MARK_RECIEVE)
            {
                PrintMessage("Фишка получена через полюс " + polusName.ToString());
                InitialNumMarks = InitialNumMarks + 1;
                DoVarChanging(new CoreName("InitialNumMarks"));
                Schedule(0, this.Show);

            }
            
        }

        private void Show()
        {
            PrintMessage("Количество фишек " + Convertion.ToStr(InitialNumMarks));
        }
    }

}
