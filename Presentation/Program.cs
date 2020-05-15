using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;

namespace Presentation
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            DependencyInjector.InjectDependencies();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(DependencyInjector.ServiceProvider.Resolve<LoginForm>());
        }
        
    }
}