using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriadWpf.GraphXModels
{
    class ob : INotifyCollectionChanged
    {
       
        public ob()
        {
            
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
