using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace SocketBusiness
{
    public class SocketFactory
    {
        public static SocketBase CreateInkSocket(bool isServer,string ip, string serviceNamd)
        {
            return isServer ? (SocketBase)new ServerSocket(ip, serviceNamd) : new ClientSocket(ip, serviceNamd);
        }
    }
    public abstract class SocketBase
    {
        protected List<StreamSocket> ClientSockets;

        protected string IpAddress;

        protected bool IsServer;

        public Action<MessageModel> MsgReceivedAction;

        public Action<Exception> OnStartFailed;

        public Action OnStartSucess;

        protected string RemoteServiceName;

        protected StreamSocket Socket;

        public bool Working;

        protected DataWriter Writer;

        public abstract Task Start();

        public abstract void Dispose();

        public async Task SendMsg(MessageModel msg,StreamSocket client = null)
        {
            client = client ?? Socket;
            await SendData(client, JsonConvert.SerializeObject(msg));
        }

        private async Task SendData(StreamSocket client, string data)
        {
            try
            {
                if (!Working) return;
                if(!IsServer)
                {
                    if (Writer == null)
                    {
                        Writer = new DataWriter(client.OutputStream);
                    }
                    await WriterData(data);
                }
                else
                {
                    foreach(var clientSocket in ClientSockets.Where(s => s != client))
                    {
                        try
                        {
                            Writer = new DataWriter(clientSocket.OutputStream);
                            await WriterData(data);
                            Writer.DetachStream();
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch(Exception e)
            {
                if (SocketError.GetStatus(e.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }
                Debug.WriteLine("Send failed with error: " + e.Message);
            }
        }
        
        private async Task WriterData(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            Writer.WriteInt32(bytes.Length);
            Writer.WriteBytes(bytes);
            await Writer.StoreAsync();
            Debug.WriteLine("Data sent successfully.");
        }
    }
}
