using Ninject;
using System;
using System.Windows;
using WpfApp1.Configuration;

namespace NorthwindWpf
{
    public partial class App : Application
    {
        protected IKernel Kernal { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            AutomapperConfiguration.Initialise();
            Kernal = NinjectConfiguration.InitKernal();
            base.OnStartup(e);
        }

        public T GetService<T>() where T : class => Kernal.Get<T>();

        public object GetService(Type type) => Kernal.Get(type);
    }
}
