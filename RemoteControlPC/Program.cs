using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteControlPC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            RemoteControlApplication app = new RemoteControlApplication();
            app.Run();

        }
    }

    class RemoteControlApplication {
        private CommandProcessor commandProcessor = new CommandProcessor();
        private NetworkHandler networkHandler;

        public RemoteControlApplication() {
            //networkHandler = new NetworkHandler(OnMessage);
            
        }

        public void Run() {

            networkHandler = new NetworkHandler(commandProcessor.Process);
            networkHandler.StartHostThread();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // todo majd ezt nem muszáj bezárni 
            networkHandler.Close();
        }
        

    }
}
