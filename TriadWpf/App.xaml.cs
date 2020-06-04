using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TriadWpf.Presenter;

namespace TriadWpf
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            MainWindow window = new MainWindow();
            AppPresenter presenter = new AppPresenter(window);

            App app = new App();
            app.Run(window);
        }
    }
}
