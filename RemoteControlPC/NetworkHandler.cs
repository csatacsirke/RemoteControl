using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.WebSockets;
using System.CodeDom;

namespace RemoteControlPC {


    interface NetworkHandlerDelegate {
        void OnMessageReceived(string message);
        void OnConnectionStatusChanged();
    }


    class NetworkHandler {

        public static readonly int PORT = 16666;

        TcpListener tcpListener;
        private Action<string> m_OnMessageReceivedCallback;
        private Thread m_thread;

        CancellationTokenSource m_cancellationTokenSource = new CancellationTokenSource();

        

        public NetworkHandler(Action<string> OnMessageReceived) {
            this.m_OnMessageReceivedCallback = OnMessageReceived;
            tcpListener = new TcpListener(IPAddress.Any, PORT);
        }


        public void StartHostThread() {
            try {
                m_thread = new Thread(HostThread);
                m_thread.Start();
            } catch (TaskCanceledException) {
                // skip
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }

        public void Close() {
            m_cancellationTokenSource.Cancel();

            tcpListener.Stop();
        }

        private async void ReaderLoop(WebSocket websocket) {
            try {
                var buffer = new Byte[8192];
                ArraySegment<Byte> bufferWrapper = new ArraySegment<byte>(buffer);


                while (websocket.State == WebSocketState.Open) {
                    WebSocketReceiveResult message = await websocket.ReceiveAsync(bufferWrapper, m_cancellationTokenSource.Token);
                    if (message.MessageType == WebSocketMessageType.Text) {
                        string messsageText = UTF8Encoding.UTF8.GetString(buffer, 0, message.Count);
                        m_OnMessageReceivedCallback.Invoke(messsageText);
                    }


                }
            } catch (TaskCanceledException) {
                // skip
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
            
        }

        private async void HostThread() {

            HttpListener httpListener = new HttpListener();
            //httpListener.Prefixes.Add("ws://localhost:16666/");
            httpListener.Prefixes.Add("http://localhost:16666/");
            httpListener.Start();

            while (true) {
                HttpListenerContext context = await httpListener.GetContextAsync();
                if (context.Request.IsWebSocketRequest) {
                    HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                    WebSocket webSocket = webSocketContext.WebSocket;

                    ReaderLoop(webSocket);
                }

            }

        }



    }
}
