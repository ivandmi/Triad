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

        private int MarksCount;

        private Double T;

        List<CoreName> outputs;

        public PetriNetPlace(Int32 MarksCount, Double T)
        {
            this.MarksCount = MarksCount;
            this.T = T;
        }

        public override void DoInitialize()
        {
            List<CoreName> names = new List<CoreName>();
            names.AddRange(routineNodePolusPairs.Keys);
            outputs = names.FindAll(x => x.BaseName == "Output");

            Schedule(0, this.TryQuery);
            PrintMessage("Инициализация вершины");
        }

        private void TryQuery()
        {
            if (MarksCount > 0)
            {
                foreach(var polus in outputs)
                {
                    SendMessageVia(PetriNetMessage.TRY_REQUEST.ToString(), polus);
                    PrintMessage("Отправка запроса через полюс " + polus.ToString());
                }
                
            }
            Schedule(T, this.TryQuery);
        }

        protected override void ReceiveMessageVia(CoreName polusName, String message)
        {

            PetriNetMessage mess = (PetriNetMessage)Enum.Parse(typeof(PetriNetMessage), message);
            if (mess == PetriNetMessage.REQEST_ACCEPTED && MarksCount>0)
            {
                PrintMessage("Ответ получен через полюс " + polusName.ToString());
                MarksCount = MarksCount - 1;
                DoVarChanging(new CoreName("MarksCount"));

                SendMessageVia(PetriNetMessage.MARK_SENT.ToString(), polusName);
                PrintMessage("Отправляем фишку через полюс" + polusName.ToString());
            }
            if (mess == PetriNetMessage.MARK_RECIEVE)
            {
                PrintMessage("Фишка получена через полюс " + polusName.ToString());
                MarksCount = MarksCount + 1;
                DoVarChanging(new CoreName("MarksCount"));
                Schedule(0, this.Show);
            }      
        }

        private void Show()
        {
            PrintMessage("Количество фишек " + Convertion.ToStr(MarksCount));
        }
    }

}
