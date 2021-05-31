using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.ServicesNdc.Interface.Exceptions
{
    public class HostNoResponseException : Exception
    {
        public HostNoResponseException(string message) : base(message)
        {
        }

        public HostNoResponseException(string operation, string message) : base($"{operation} - {message}")
        {
        }
    }
}
