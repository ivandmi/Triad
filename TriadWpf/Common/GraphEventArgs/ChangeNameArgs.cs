using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriadWpf.Common.GraphEventArgs
{
    public class ChangeNameArgs : EventArgs
    {
        public string OldName;
        public string NewName;

        public ChangeNameArgs(string oldName, string newName)
        {
            OldName = oldName;
            NewName = newName;
        }
    }
}
