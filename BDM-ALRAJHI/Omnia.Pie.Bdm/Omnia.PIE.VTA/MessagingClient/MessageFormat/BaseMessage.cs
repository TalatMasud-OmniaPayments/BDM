using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingClient.MessageFormat
{
    public class BaseMessage
    {
        public string Flag { get; set; }
        public string Header { get; set; }
        public string Data { get; set; }
    }
}
