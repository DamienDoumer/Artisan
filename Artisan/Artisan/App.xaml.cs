using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TimeManager;

namespace Artisan
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            ///this line of code causes every thing to close even running threads
            /// So that the app exits totally and normally.
            Environment.Exit(Environment.ExitCode);
        }
    }
}
