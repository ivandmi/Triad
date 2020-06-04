using System;
using System.Collections.Generic;
using System.Text;

namespace TriadCore
{
    public class Receiver : Routine
    {
        public int count;

        public Receiver()
        {
            count = 0;
        }

        protected override void ReceiveMessageVia(CoreName polusName, string message)
        {
            
        }
    }
}
