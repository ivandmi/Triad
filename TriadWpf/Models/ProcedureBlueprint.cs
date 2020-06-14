using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriadWpf.Common.GraphEventArgs;
using TriadWpf.Common.Interfaces;

namespace TriadWpf.Models
{
    /// <summary>
    /// Заготовка для создания процедуры для модели
    /// </summary>
    public class ProcedureBlueprint : INotifyPropertyChanged
    {
        /// <summary>
        /// Дополнительная информация о типе процедуры
        /// </summary>
        public IProcedureMetadata Metadata { get; set; }

        /// <summary>
        /// Соответствие параметров процедры параметром из модели
        /// </summary>
        public Dictionary<IPParamMetadata, string> ModelParamByProcParam { get; set; }

        /// <summary>
        /// Имя, которое дает пользователь
        /// </summary>
        public string Name 
        {
            get => name; 
            set
            {
                name = value;
                OnPropertyChanged("Name");
            } 
        }

        private string name;

        public ProcedureBlueprint()
        {
            ModelParamByProcParam = new Dictionary<IPParamMetadata, string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
