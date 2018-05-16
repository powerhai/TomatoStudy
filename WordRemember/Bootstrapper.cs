using Microsoft.Practices.Unity;
using Prism.Unity;
using WordRemember.Views;
using System.Windows;
using log4net;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;
namespace WordRemember
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell() {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell() {
            Application.Current.MainWindow.Show();
        }
        protected override IModuleCatalog CreateModuleCatalog () {

            return new DirectoryModuleCatalog() { ModulePath = "Modules"};
        }
        protected override IUnityContainer CreateContainer()
        {
            
            var container =  base.CreateContainer();
            container.RegisterInstance<ILog>(LogManager.GetLogger("Tomato")); 
            return container;
        }
 
    }
}
