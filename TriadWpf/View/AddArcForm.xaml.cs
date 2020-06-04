using System.Collections.Generic;
using System.Windows;
using TriadCore;

namespace TriadWpf.View
{
    /// <summary>
    /// Логика взаимодействия для AddArcForm.xaml
    /// </summary>
    public partial class AddArcForm : Window
    {
        public AddArcForm(IEnumerable<CoreName> from, IEnumerable<CoreName> to)
        {
            InitializeComponent();
            comboFrom.ItemsSource = from;
            comboTo.ItemsSource = to;
            comboFrom.SelectedIndex = 0;
            comboTo.SelectedIndex = 0;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public CoreName PolusFrom
        {
            get => comboFrom.SelectedItem as CoreName;
        }

        public CoreName PolusTo
        {
            get => comboTo.SelectedItem as CoreName;
        }
    }
}
