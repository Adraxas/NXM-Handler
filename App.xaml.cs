using Hardcodet.Wpf.TaskbarNotification;
using SingleInstanceCore;
using System.Windows;

namespace NXM_Handler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstance
    {
        public void OnInstanceInvoked(string[] args)
        {
            // What to do with the args another instance has sent
            if (args.Length > 0)
            {
                Relay.RelayURL(args[1]);
            }
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool isFirstInstance = this.InitializeAsFirstInstance("NXMH");
            if (!isFirstInstance)
            {
                //If it's not the first instance, arguments are automatically passed to the first instance
                //OnInstanceInvoked will be raised on the first instance
                //You may shut down the current instance
                Current.Shutdown();
            }
            Relay.RegisterNXM();
            if (e.Args.Length > 0)
            {
                Relay.RelayURL(e.Args[1]);
            }
        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //Do not forget to cleanup
            SingleInstance.Cleanup();
        }
    }
}
