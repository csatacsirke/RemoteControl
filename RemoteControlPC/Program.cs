using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

    class RemoteControlApplication : NetworkHandlerDelegate {
        private CommandProcessor commandProcessor = new CommandProcessor();
        private NetworkHandler networkHandler;
        private MainDialog mainDialog;
        
        private CancellationTokenSource m_cancellationTokenSource = new CancellationTokenSource();

        public RemoteControlApplication() {
            //networkHandler = new NetworkHandler(OnMessage);
            
        }

        public void Run() {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mainDialog = new MainDialog();
            mainDialog.Show();
            
            networkHandler = new NetworkHandler(this, m_cancellationTokenSource.Token);
            Task hostTask = networkHandler.StartHostAsync();
            
            
            Application.Run(mainDialog);

            m_cancellationTokenSource.Cancel();

            // todo majd ezt nem muszáj bezárni 
            networkHandler.Close();

            // ottrohadjonmeg... valamiért nem lehet rendesen le-cancelezni
            //hostTask.Wait();
        }

        void NetworkHandlerDelegate.OnConnectionStatusChanged(ConnectionInfo connectionInfo) {
            mainDialog.ThreadSafeSetNetworkStatusText(connectionInfo.connectionCount == 0 ? "No clients connected." : "Clients: " + connectionInfo.connectionCount.ToString());
        }

        void NetworkHandlerDelegate.OnFatalError(string message) {
            mainDialog.Invoke(new Action(() => {
                mainDialog.ThreadSafeSetNetworkStatusText(message);
            }));
        }

        void NetworkHandlerDelegate.OnMessageReceived(string message) {
            commandProcessor.Process(message);
        }
    }
}
