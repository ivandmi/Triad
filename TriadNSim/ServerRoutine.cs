﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TriadCore;

namespace TriadCore
{
    public class RServer : TriadCore.Routine 
    {
        private Double maxLength;
        
        private Double deltaT;
        
        private Boolean busy;
        
        private Double length;

        public RServer(Double maxLength, Double deltaT)
        {
            this.maxLength = maxLength;
            this.deltaT = deltaT;
        }
        
        public override void DoInitialize() 
        {
            busy = false;
            DoVarChanging(new CoreName("busy"));
            length = 0;
            DoVarChanging(new CoreName("length"));
            PrintMessage("Инициализация сервера");
        }
        
        protected override void ReceiveMessageVia(CoreName polusName, String message) 
        {
            Int32 polusIndex = GetPolusIndex(polusName);
            if (length < maxLength) 
            {
                length++;
                DoVarChanging(new CoreName("length"));
                if (!busy) 
                {
                    Schedule(deltaT, this.EndService);
                }
                busy = true;
                DoVarChanging(new CoreName("busy"));
                PrintMessage("Постановка запроса в очередь");
            }
            else 
            {
                PrintMessage("Сервер отказал в обслуживании");
            }
        }

        //окончание облсуживания
        private void EndService() 
        {
            if (length > 0) 
            {
                length--;
                DoVarChanging(new CoreName("length"));
            }
            if (length>0) 
            {
                Schedule(deltaT, this.EndService);
            }
            else 
            {
                busy = false;
                DoVarChanging(new CoreName("busy"));
            }
            PrintMessage("Окончание обслуживания. Текущая длина очереди: " + length.ToString());
        }
    }
}
