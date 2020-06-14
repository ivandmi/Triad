using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TriadCore;
using TriadWpf.Common.Interfaces;
using TriadWpf.Models;

namespace TriadWpf.View
{
    /// <summary>
    /// Логика взаимодействия для SimilationResultView.xaml
    /// </summary>
    public partial class SimilationResultView : Window, IResultView
    {
        public SimilationResultView()
        {
            InitializeComponent();
            tabs = new Dictionary<string, TabItem>();
        }

        Dictionary<string, TabItem> tabs;

        // Не нравится, надо придумать, как по человечески отображать эти значения
        public void AddProcedureOutputResult(string ProcedureName, string paramName, object value)
        {
            if (!tabs.ContainsKey(ProcedureName))
            {
                var tab = new TabItem();
                tab.Header = ProcedureName;
                var box = new TextBox();
                box.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                tab.Content = box;
                tabControl.Items.Add(tab);
                tabs.Add(ProcedureName, tab);
            }
            var tabItem = tabs[ProcedureName];
            var txtBox = tabItem.Content as TextBox;
            txtBox.Text += paramName + ": ";

            if(value is IEnumerable<object> array)
            {
                foreach(var element in array)
                {
                    txtBox.Text += "\n" + element.ToString();
                }
            }
            else
            {
                txtBox.Text += value.ToString();
            }
            txtBox.Text += "\n\n";
            
        }

        public void AddProcedureResult(ProcedureResult result)
        {
            commonProcResults.Items.Add(result);
        }

        public void ShowModelLog(List<LoggerRecord> records)
        {
            logTable.ItemsSource = records;
            
        }
    }
}
