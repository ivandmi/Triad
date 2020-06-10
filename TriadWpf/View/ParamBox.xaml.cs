using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TriadCore;

namespace TriadWpf.View
{
    /// <summary>
    /// Логика взаимодействия для ParamBox.xaml
    /// </summary>
    public partial class ParamBox : UserControl
    {
        public ParamBox()
        {
            InitializeComponent();
        }

        public string ParamName
        {
            get
            {
                return paramName.Text;
            }

            set
            {
                paramName.Text = value;
            }
        }

        // Добавить валидацию вводимого значения
        public string ParamValue
        {
            get
            {
                return paramValue.Text;
            }

            set
            {
                paramValue.Text = value;
            }
        }
    }
}
