using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketBusiness
{
    public enum MessageType
    {
        Disconnect,
        Connect,
    }

    public class MessageModel
    {
        public MessageType MessageType { get; set; }
        public string Content { get; set; }
        public DateTime Published { get; set; }
    }
}
