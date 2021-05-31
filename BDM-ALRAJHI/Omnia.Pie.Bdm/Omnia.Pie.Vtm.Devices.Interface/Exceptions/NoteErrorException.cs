using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Devices.Interface.Exceptions
{
    public class NoteErrorException : DeviceMalfunctionException
    {
        public string ErrorType { get; private set; }
        public NoteErrorException(string message) : base(message)
        {
        }

        public NoteErrorException(string operation, string result) :
            this($"Operation {operation} has failed with Note Error = {result}.")
        {
            ErrorType = result;
        }
    }
}
