namespace Omnia.Pie.Vtm.ServicesNDC
{
	using Omnia.Pie.Vtm.Framework.ControlExtenders;
	using System.Threading;

	public interface IAsyncNdcTcpClient
	{
		string NdcIp { get; set; }
		int NdcPort { get; set; }
		bool Ssl { get; set; }
		AsyncTcpClient Client { get; set; }
	}

	public class AsyncNdcTcpClient : IAsyncNdcTcpClient
	{
		public string NdcIp { get; set; }
		public int NdcPort { get; set; }
		public bool Ssl { get; set; }
		public AsyncTcpClient Client { get; set; }

		public AsyncNdcTcpClient()
		{
			NdcIp = "192.168.1.210";
			NdcPort = 44444;

			ConnectClientAsync();
		}

		private async void ConnectClientAsync()
		{
			await Client.ConnectAsync(NdcIp, NdcPort, Ssl, CancellationToken.None);
		}
	}
}