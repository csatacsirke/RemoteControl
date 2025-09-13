using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using System.Net;

namespace RemoteControlPC {
    class NetworkHandler {

        private UdpClient udpClient = new UdpClient();
        private Action<string> OnMessageReceived;

        public NetworkHandler(Action<string> OnMessageReceived) {
            this.OnMessageReceived = OnMessageReceived;
        }


        public void Host() {
            try {
                Thread thread = new Thread(HostThread);
                thread.Start();

            } catch(Exception e) {
                MessageBox.Show(e.ToString());
            }
        }

        public void Close() {
            udpClient.Close();
        }

        private void HostThread() {
            
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 16666);
            udpClient.Client.Bind(RemoteIpEndPoint);



            try {
                while (true) {
                    Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    string message = System.Text.Encoding.UTF8.GetString(receiveBytes);
                    OnMessageReceived(message);
                }
            } catch (SocketException) {
                // socket closed
                return;



            }

            
        }



    }
}
