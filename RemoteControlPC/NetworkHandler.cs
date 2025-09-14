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
using System.Xml.Schema;

namespace RemoteControlPC {

    public struct ConnectionInfo {
        public int connectionCount;
    }


    public interface NetworkHandlerDelegate {
        void OnMessageReceived(string message);
        void OnConnectionStatusChanged(ConnectionInfo connectionInfo);
        void OnFatalError(string message);
    }


    class NetworkHandler {

        public static readonly int PORT = 16666;

        TcpListener tcpListener;
        //private Action<string> m_OnMessageReceivedCallback;
        private NetworkHandlerDelegate m_delegate;
        private Thread m_thread;

        CancellationTokenSource m_cancellationTokenSource = new CancellationTokenSource();

        

        public NetworkHandler(NetworkHandlerDelegate @delegate) {
            m_delegate = @delegate;
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

        private async Task ReaderLoop(WebSocket websocket) {
            try {
                var buffer = new Byte[8192];
                ArraySegment<Byte> bufferWrapper = new ArraySegment<byte>(buffer);


                while (websocket.State == WebSocketState.Open) {
                    WebSocketReceiveResult message = await websocket.ReceiveAsync(bufferWrapper, m_cancellationTokenSource.Token);
                    if (message.MessageType == WebSocketMessageType.Text) {
                        string messsageText = UTF8Encoding.UTF8.GetString(buffer, 0, message.Count);
                        m_delegate.OnMessageReceived(messsageText);
                    }


                }
            } catch (TaskCanceledException) {
                // skip
            } catch (Exception e) {
                //MessageBox.Show(e.ToString());
                m_delegate.OnFatalError(e.ToString());
            }
            
        }


        private int m_connectionCount = 0;
        private void ClientConnectionStarted() {
            ++m_connectionCount;
            m_delegate.OnConnectionStatusChanged(new ConnectionInfo { connectionCount = m_connectionCount });
        }

        private void ClientConnectionEnded() {
            --m_connectionCount;
            m_delegate.OnConnectionStatusChanged(new ConnectionInfo { connectionCount = m_connectionCount });
        }

        private async Task StartAsyncReaderLoop(WebSocket webSocket) {

            ClientConnectionStarted();
            
            await ReaderLoop(webSocket);

            ClientConnectionEnded();

        }

        private async void HostThread() {
            try {
                HttpListener httpListener = new HttpListener();
                //httpListener.Prefixes.Add("ws://localhost:16666/");
                //httpListener.Prefixes.Add("http://192.168.0.171:16666/");
                httpListener.Prefixes.Add("http://nagygep.local:16666/");
                //httpListener.Prefixes.Add("http://localhost:16666/");
                httpListener.Start();


                

                while (true) {
                    HttpListenerContext context = await httpListener.GetContextAsync();
                    if (context.Request.IsWebSocketRequest) {
                        HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                        WebSocket webSocket = webSocketContext.WebSocket;
                        //await StartAsyncReaderLoop(webSocket);
                        await Task.Factory.StartNew(() => StartAsyncReaderLoop(webSocket));
                    }

                }

            } catch (Exception e) {
                m_delegate.OnFatalError(e.Message);
            }
        }



    }
}
