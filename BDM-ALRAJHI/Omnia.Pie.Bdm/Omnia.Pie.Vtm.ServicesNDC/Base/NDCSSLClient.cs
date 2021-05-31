using Omnia.Pie.Vtm.Framework.Extensions;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Framework.Interface.Tcp;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Omnia.Pie.Vtm.ServicesNdc.Base
{
	public class NDCSSLClient : ISSLClient
	{
		private DispatcherTimer _timer;
		private readonly ILogger _logger;
		private string Ip { get; set; }
		private int Port { get; set; }
		private bool IsReceiving { set; get; }
		private int NdcRequestTimeOut { get; set; }

		public bool Ssl { get; set; } = false;
		public bool InService { get; set; }
		public string CertificateLocation { get; set; }
		public TcpClient Client { get; set; }
		public SslStream SslStream { get; set; }

		public NDCSSLClient(IResolver container)
		{
			_logger = container.Resolve<ILogger>();

			Ip = ConfigurationManager.AppSettings["NdcIp"].ToString();

			if (string.IsNullOrEmpty(Ip))
				throw new ArgumentNullException($"Missing Ndc {nameof(Ip)} in the config file.");

			CertificateLocation = ConfigurationManager.AppSettings["CertificateLocation"].ToString();

			if (string.IsNullOrEmpty(CertificateLocation))
				throw new ArgumentNullException($"Missing Ndc {nameof(CertificateLocation)} in the config file.");

			int.TryParse(ConfigurationManager.AppSettings["NdcPort"].ToString(), out var port);
			Port = port;

			var timeout = 20000;
			int.TryParse(ConfigurationManager.AppSettings["NDCRequestTimeOut"].ToString(), out timeout);
			NdcRequestTimeOut = timeout;

			_timer = new DispatcherTimer(DispatcherPriority.Background)
			{
				Interval = new TimeSpan(0, 4, 0)
			};

			_timer.Tick += _timer_Tick;
			_timer.Start();

			_logger?.Info($"{GetType()} => Initialized.");
		}

		public bool IsConnected()
		{
			if (Client != null)
			{
				return Client.Connected && SslStream.CanWrite;
			}

			return false;
		}

		public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		private async void _timer_Tick(object sender, EventArgs e)
		{
			if (!IsReceiving)
			{
				if (IsConnected())
				{
					await SendAsync(AppendLength2Bytes(ObjectToByteArray(GetReadyRequest())), false);
				}
				else
				{
					await StartClientAsync();
				}
			}
		}

		public byte[] ObjectToByteArray(Object obj)
		{
			return Encoding.UTF8.GetBytes(obj.ToString());
		}

		private string GetReadyRequest()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("2", "Message Class");
			stringBuilder.Append("2", "Message Sub-Class");
			stringBuilder.Append("\u001c", "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append("\u001c", "Field Separator");
			stringBuilder.Append("\u001c", "Field Separator");
			stringBuilder.Append("B", "data");
			return stringBuilder.ToStringExtend();
		}

		public async Task StartClientAsync()
		{
			try
			{
				if (Client != null && !IsConnected())
				{
					Client.Dispose();
				}

				Client = new TcpClient(ConfigurationManager.AppSettings["NdcIp"].ToString(), int.Parse(ConfigurationManager.AppSettings["NdcPort"].ToString()));
				Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 5000);
				SslStream = new SslStream(Client.GetStream(), true, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

				var serverCertificate = new X509Certificate2(CertificateLocation);
				var col = new X509Certificate2Collection { serverCertificate };

				SslStream.ReadTimeout = NdcRequestTimeOut;
				await SslStream.AuthenticateAsClientAsync(Ip, col, SslProtocols.Tls12, false);

				_logger?.Info("Connected to NDC");
			}
			catch (Exception ex)
			{
				_logger?.Info("Error while connecting to NDC");
				_logger?.Exception(ex);
			}
		}

		public byte[] AppendLength2Bytes(byte[] data)
		{
			var len = data.Length;
			var buf = new byte[len + 2];
			buf[0] = (byte)(len >> 8 & 255);
			buf[1] = (byte)(len & 255);
			Array.Copy(data, 0, buf, 2, len);

			return buf;
		}

		public async Task<string> SendAsync(byte[] msg, bool waitForResponse)
		{
			try
			{
				_logger?.Info($"Ndc Request Message From BIB : {Encoding.Default.GetString(msg)}");
			}
			catch (Exception) { }

			_timer.Stop();

			if (waitForResponse)
			{
				IsReceiving = true;
			}

			if (!IsConnected())
			{
				await StartClientAsync();
			}

			SslStream.Write(msg);
			SslStream.Flush();

			var messageData = new StringBuilder();
			var charBuffer = 512;

			if (waitForResponse)
			{
				SslStream.ReadTimeout = NdcRequestTimeOut;
				var decoder = Encoding.UTF8.GetDecoder();

				using (var reader = new StreamReader(SslStream, Encoding.UTF8, false, charBuffer, true))
				{
					do
					{
						try
						{
							var buffer = new char[charBuffer];
							var read = await reader.ReadAsync(buffer, 0, buffer.Length);
							messageData.Append(buffer.Take(read).ToArray());
						}
						catch (Exception ex)
						{
							_logger?.Exception(ex);
							break;
						}
					}
					while (reader.Peek() != -1);
				}
			}

			_logger?.Info($"Ndc Response Message From Server : {messageData.ToString()}");

			if (waitForResponse)
			{
				IsReceiving = false;
			}

			_timer.Start();

			return messageData.ToString();
		}

		public void Dispose()
		{
			_logger?.Info($"{GetType()} => Dispose()");
		}
	}
}