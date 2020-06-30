using NorthwindWpf.Core.Service;
using System.Windows;
using WpfApp1.Configuration;

namespace NorthwindWpf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AutomapperConfiguration.Initialise();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ServiceResolver.Get().Dispose();
            base.OnExit(e);
        }
    }
}
