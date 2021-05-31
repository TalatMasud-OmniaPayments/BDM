using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.DataAccess.Interface.Entities
{
    public class TerminalAccount
    {
        public int id { get; set; }
        public string terminalId { get; set; }
        public string AccountNumber { get; set; }
    }
}
