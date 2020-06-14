using System;
using System.Collections.Generic;
using System.Text;

namespace TriadCore
{
    public class MessageGenerator : Routine
    {
        private double Delay;

        /// <summary>
        /// Инициализирует новый экземпляр класса MessageGenerator с интервалом отправки delay
        /// </summary>
        /// <param name="Delay">Интервал времени между отправкой сообщений</param>
        public MessageGenerator(double Delay)
        {
            this.Delay = Delay;
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
