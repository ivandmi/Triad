using System;
using System.Collections.Generic;
using System.Text;

namespace TriadCore.Рутины.Базовые
{
    public class PetriNetTransition : Routine
    {

        private Double T;

        private Int32 requestsCount;

        private Int32 marksCount;

        Dictionary<CoreName, bool> inputs;
        List<CoreName> outputs;

        public PetriNetTransition(Double T)
        {
            this.T = T;
        }

        public override void DoInitialize()
        {
            List<CoreName> names = new List<CoreName>();
            names.AddRange(routineNodePolusPairs.Keys);

            inputs = new Dictionary<CoreName, bool>();
            foreach(var name in names.FindAll(x => x.BaseName == "Input"))
            {
                inputs.Add(name, false);
            }

            outputs = names.FindAll(x => x.BaseName == "Output");

            PrintMessage("Инициализация вершины");
        }

        protected override void ReceiveMessageVia(CoreName polusName, String message)
        {
            Int32 polusIndex = GetPolusIndex(polusName);
            PetriNetMessage mess = (PetriNetMessage)Enum.Parse(typeof(PetriNetMessage), message);
            if (mess == PetriNetMessage.TRY_REQUEST)
            {
                PrintMessage("Получен запрос из полюса " + polusName.ToString());
                if (!inputs[polusName])
                {
                    inputs[polusName] = true;
                    
                    requestsCount = requestsCount + 1;
                    DoVarChanging(new CoreName("requestsCount"));

                    Schedule(0, this.Verify);
                }
            }
            else
            {
                if (mess == PetriNetMessage.MARK_SENT)
                {
                    PrintMessage("Фишка получена через полюс " + polusName.ToString());
                    marksCount = marksCount + 1;
                    DoVarChanging(new CoreName("marksCount"));
                    Schedule(0, this.Send);
                }
            }
        }

        private void Send()
        {
            if (marksCount == inputs.Count)
            {
                foreach (var name in outputs) 
                { 
                    SendMessageVia(PetriNetMessage.MARK_RECIEVE.ToString(), name);
                    PrintMessage("Передача фишки через полюс " + name.ToString());
                }
                requestsCount = 0;
                DoVarChanging(new CoreName("requestsCount"));

                List<CoreName> keys = new List<CoreName>(inputs.Keys);

                foreach (var key in keys)
                {
                    inputs[key] = false;
                }
                marksCount = 0;
                DoVarChanging(new CoreName("marksCount"));
            }
        }

        private void Verify()
        {
            if (requestsCount == inputs.Count)
            {
                PrintMessage("Получен запрос от всех вершин");

                foreach(var key in inputs.Keys)
                {
                    SendMessageVia(PetriNetMessage.REQEST_ACCEPTED.ToString(), key);
                    
                    PrintMessage("Отправляем ответ через полюс " + key.ToString());
                }
            }
        }
    }
}
