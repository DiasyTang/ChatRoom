using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class ChatRecord
    {
        public string UserName { get; set; }
        public string Published { get; set; }
        public string Comment { get; set; }
        public bool IsOther { get; set; }
    }
}
