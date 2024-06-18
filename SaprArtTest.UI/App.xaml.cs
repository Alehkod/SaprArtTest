using Autofac;
using NLog;
using SaprArtTest.UI.Repositories;
using SaprArtTest.UI.Services;
using System.Windows;
using System.Windows.Navigation;

namespace SaprArtTest.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<RectangleRepository>().AsSelf().SingleInstance();
            builder.RegisterType<RectangleService>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterInstance(LogManager.GetLogger("log")).As<ILogger>();

            var container = builder.Build();


            var window = container.Resolve<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }
    }
}
