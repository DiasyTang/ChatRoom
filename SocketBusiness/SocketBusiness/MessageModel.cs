using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SocketBusiness
{
    public enum MessageType
    {
        Disconnect,
        Connect,
    }

    [DataContract]
    public class MessageModel
    {
        [DataMember]
        public MessageType MessageType { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public DateTime Published { get; set; }
    }
}
