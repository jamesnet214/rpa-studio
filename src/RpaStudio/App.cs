using RpaStudio.UI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RpaStudio
{
    public class App : Application
    {
        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            StudioWindow main = new();
            MainWindow = main;

            main.ShowDialog();
        }
    }
}
