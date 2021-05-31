namespace Omnia.Pie.Vtm.Framework.Interface
{
	using System;
	using System.Security.Cryptography.X509Certificates;

	public interface ITcpClient : IDisposable
	{
		string Ip { get; set; }
		int Port { get; set; }
		bool Ssl { get; set; }
		bool InService { get; set; }
		void StartClient();
		bool IsConnected();
		void Receive();
		void Send(byte[] msg);
		byte[] DiscardFirst2Bytes(byte[] data);
		byte[] AppendLength2Bytes(byte[] data);
		event EventHandler<MessageReceivedArguments> MessageReceived;
		void SetupSSL(X509Certificate2 certificate = null);
	}

	public class MessageReceivedArguments : EventArgs
	{
		public byte[] Buffer { get; set; }
	}
}