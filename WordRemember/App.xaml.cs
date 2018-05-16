using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using log4net;
namespace WordRemember
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { 
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            this.DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
             
            bootstrapper.Run();
            
        }
         
        private void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var log = LogManager.GetLogger(typeof(Bootstrapper));
            log.Error($"GUI Exception: {e.Exception}");
            e.Handled = true; 
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            var log = LogManager.GetLogger(typeof(Bootstrapper));
            log.Error($"GUI Exception: {exception}"); 
        } 
    }
}
