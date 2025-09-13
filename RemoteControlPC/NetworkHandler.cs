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

        private UdpClient m_udpClient = new UdpClient();
        private Action<string> m_OnMessageReceivedCallback;
        private Thread m_thread;

        public NetworkHandler(Action<string> OnMessageReceived) {
            this.m_OnMessageReceivedCallback = OnMessageReceived;
        }


        public void StartHostThread() {
            try {
                m_thread = new Thread(HostThread);
                m_thread.Start();

            } catch(Exception e) {
                MessageBox.Show(e.ToString());
            }
        }

        public void Close() {
            m_udpClient.Close();
        }

        private void HostThread() {
            
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 16666);
            m_udpClient.Client.Bind(RemoteIpEndPoint);



            try {
                while (true) {
                    Byte[] receiveBytes = m_udpClient.Receive(ref RemoteIpEndPoint);
                    string message = System.Text.Encoding.UTF8.GetString(receiveBytes);
                    m_OnMessageReceivedCallback(message);
                }
            } catch (SocketException) {
                // socket closed
                return;



            }

            
        }



    }
}
