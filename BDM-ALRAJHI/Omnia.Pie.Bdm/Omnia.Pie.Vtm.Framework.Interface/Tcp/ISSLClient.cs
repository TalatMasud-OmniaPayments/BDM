using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Framework.Interface.Tcp
{
	public interface ISSLClient : IDisposable
	{
		bool Ssl { get; set; }
		bool InService { get; set; }
		TcpClient Client { get; set; }
		SslStream SslStream { get; set; }
		Task StartClientAsync();
		bool IsConnected();
		Task<string> SendAsync(byte[] msg, bool waitForResponse);
		byte[] AppendLength2Bytes(byte[] data);
	}
}