namespace Omnia.Pie.Vtm.ServicesNdc
{
	using Omnia.Pie.Vtm.Framework.Interface;

	public interface INdcTcpClient
	{
		string NdcIp { get; set; }
		int NdcPort { get; set; }
		bool Ssl { get; set; }
		ITcpClient Client { get; set; }
	}

	public class NdcTcpClient : INdcTcpClient
	{
		public string NdcIp { get; set; } = "192.168.1.210";
		public int NdcPort { get; set; } = 44444;
		public bool Ssl { get; set; }
		public ITcpClient Client { get; set; }

		public NdcTcpClient()
		{
			Client.StartClient();
		}
	}
}