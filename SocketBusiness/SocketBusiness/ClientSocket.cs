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
    public class ClientSocket : SocketBase
    {
        private HostName hostName;

        public ClientSocket(string ip, string serviceName)
        {
            IsServer = false;
            IpAddress = ip;
            RemoteServiceName = serviceName;
        }
        public override void Dispose()
        {
            Working = false;
            Writer = null;
            Socket?.Dispose();
            Socket = null;
        }

        public override async Task Start()
        {
            try
            {
                if (Working) return;
                hostName = new HostName(IpAddress);
                Socket = new StreamSocket();
                Socket.Control.KeepAlive = false;

                Debug.WriteLine("Connecting to:" + hostName.DisplayName);

                await Socket.ConnectAsync(hostName, RemoteServiceName);
                OnStartSucess?.Invoke();
                Debug.WriteLine("Connected");
                Working = true;
                await Task.Run(async () =>
                {
                    var reader = new DataReader(Socket.InputStream);

                    try
                    {
                        while (Working)
                        {
                            var sizeFieldCount = await reader.LoadAsync(sizeof(uint));                          

                            var stringLength = reader.ReadUInt32();
                            var actualStringLength = await reader.LoadAsync(stringLength);
                            if (stringLength != actualStringLength|| sizeFieldCount != sizeof(uint))
                            {
                                //数据接收中断开连接  
                                reader.DetachStream();
                                OnStartFailed?.Invoke(new Exception("断开连接"));
                                Dispose();
                                return;
                            }
                            //接受数据  
                            var dataArray = new byte[actualStringLength];
                            reader.ReadBytes(dataArray);
                            //转为json字符串  
                            var dataJson = Encoding.UTF8.GetString(dataArray);
                            //反序列化为数据对象  
                            var data = JsonConvert.DeserializeObject<MessageModel>(dataJson);
                            //新消息到达通知  
                            MsgReceivedAction?.Invoke(data);
                        }
                    }
                    catch (Exception exception)
                    {
                        if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                        {
                        }
                        Debug.WriteLine(string.Format("Received data: \"{0}\"",
                            "Read stream failed with error: " + exception.Message));
                        reader.DetachStream();
                        OnStartFailed?.Invoke(exception);
                        Dispose();
                    }

                });
            }
            catch(Exception e)
            {
                if (SocketError.GetStatus(e.HResult) == SocketErrorStatus.Unknown)
                {

                }
                Debug.WriteLine(string.Format("Received data: \"{0}\"",
                            "Read stream failed with error: " + e.Message));
                OnStartFailed?.Invoke(e);
                Dispose();
            }
        }
    }
}
