using System;
using System.Collections.Generic;
using System.Text;

namespace TriadCore
{
    public class MessageGenerator : Routine
    {
        public double Delay { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса MessageGenerator с интервалом отправки delay
        /// </summary>
        /// <param name="delay">Интервал времени между отправкой сообщений</param>
        public MessageGenerator(double delay)
        {
            Delay = delay;
        }

        public MessageGenerator()
        {
            Delay = 1;
        }

        public override void DoInitialize()
        {
            Schedule(Delay, SendMessage);
        }
        
        private void SendMessage()
        {
            SendMessageViaAllPoluses(" ");
        }
    }
}
