using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace SocketBusiness
{
    public class ServerSocket : SocketBase
    {
        protected StreamSocketListener Listener;

        public ServerSocket(string ip,string serviceName)
        {
            IpAddress = ip;
            RemoteServiceName = serviceName;
        }
        public override async void Dispose()
        {
            Working = false;
            await SendMsg(new MessageModel
            {
                MessageType = MessageType.Disconnect
            });
            foreach(var clientSocket in ClientSockets)
            {
                clientSocket.Dispose();
            }
            ClientSockets.Clear();
            ClientSockets = null;

            Listener.ConnectionReceived -= OnConnection;
            Listener?.CancelIOAsync();
            Listener.Dispose();
            Listener = null;
        }

        public override async Task Start()
        {
            try
            {
                if (Working) return;
                IsServer = true;
                ClientSockets = new List<StreamSocket>();
                Listener = new StreamSocketListener()
                {
                    Control = { KeepAlive = false }
                };
                Listener.ConnectionReceived += OnConnection;
                var hostname = new HostName(IpAddress);
                await Listener.BindEndpointAsync(hostname, RemoteServiceName);
                Working = true;
                OnStartSucess?.Invoke();
            }
            catch(Exception e)
            {
                OnStartFailed?.Invoke(e);
            }
        }

        private async void OnConnection(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Writer = null;
            ClientSockets.Add(args.Socket);

            var reader = new DataReader(args.Socket.InputStream);           
            try
            {
                while (Working)
                {
                    var sizeFaildCount = await reader.LoadAsync(sizeof(uint));

                    var stringLength = reader.ReadUInt32();
                    var actualStringLength = await reader.LoadAsync(stringLength);

                    if (sizeFaildCount != sizeof(uint) ||actualStringLength!=stringLength)
                    {
                        reader.DetachStream();
                        reader.Dispose();
                        ClientSockets?.Remove(args.Socket);
                        return;
                    }

                    var dataArrary = new byte[actualStringLength];
                    reader.ReadBytes(dataArrary);
                    var dataJson = Encoding.UTF8.GetString(dataArrary);
                    var data = JsonConvert.DeserializeObject<MessageModel>(dataJson);
                    await SendMsg(data, args.Socket);
                    MsgReceivedAction?.Invoke(data);
                }
            }
            catch(Exception e)
            {
                if (SocketError.GetStatus(e.HResult) == SocketErrorStatus.Unknown)
                {

                }
                Debug.WriteLine(string.Format("Received data: \"{0}\"", "Read stream failed with error: " + e.Message));
                reader.DetachStream();
                reader.Dispose();
                ClientSockets?.Remove(args.Socket);
            }
        }
    }
}
