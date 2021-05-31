using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.ServicesNdc.Interface
{
    public interface INdcClient : IDisposable
    {
        Task StartClientAsync();
        bool IsConnected();
        Task<string> SendAsync(byte[] msg, bool waitForResponse);
        byte[] AppendLength2Bytes(byte[] data);

        Task SendKeepAliveReadyBMsg();

        string LastMessage { get; set; }

    }
}
