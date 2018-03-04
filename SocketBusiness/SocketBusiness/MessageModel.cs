using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SocketBusiness
{
    [DataContract]
    public class MessageModel
    {
        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public DateTime Published { get; set; }

        [DataMember]
        public string Sender { get; set; }

        [DataMember]
        public bool IsDisConnected { get; set; }
    }
}
