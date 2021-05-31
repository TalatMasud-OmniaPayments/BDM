namespace Omnia.Pie.Vtm.Framework
{
	using Omnia.Pie.Vtm.Framework.Extensions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Configuration;
	using System.Linq;
	using System.Net;
	using System.Net.Sockets;
	using System.Text;
	using System.Threading;
	using System.Net.Security;
	using System.Security.Cryptography.X509Certificates;
	using System.Security.Authentication;

	public sealed class TcpClient : ITcpClient
	{
		private Socket _socket;
		private readonly byte[] _buffer = new byte[_bufferSize];
		public byte[] Buffer => _buffer;
		private const int _bufferSize = 8192;
		private int bufferSize = 8192;
		private int BufferSize
		{
			get { return bufferSize; }
			set
			{
				bufferSize = value;
				if (_socket != null)
					_socket.ReceiveBufferSize = value;
			}
		}
		private readonly ManualResetEvent connected = new ManualResetEvent(false);
		private readonly ManualResetEvent sent = new ManualResetEvent(false);
		private readonly ManualResetEvent received = new ManualResetEvent(false);

		private const string FieldSeparator = "\u001c";
		private const string GroupSeparator = "\u001d";
		private const string EscapeCharacter = "\u001b";
		private const string GreaterThan = "\u003e";
		private const string ShiftOutCharacter = "\u000e";
		private Char FieldSeparatorChar = '\u001c';
		private Char EscapeCharacterChar = '\u001b';
		private Char ShiftOutCharacterChar = '\u000e';

		public bool IsReceiving { set; get; }
		public int MinBufferSize { get; set; } = 8192;
		public int MaxBufferSize { get; set; } = 15 * 1024 * 1024;
		public string Ip { get; set; } = ConfigurationManager.AppSettings["NdcIp"].ToString();
		public int Port { get; set; } = int.Parse(ConfigurationManager.AppSettings["NdcPort"].ToString());
		public bool Ssl { get; set; } = false;
		public bool InService { get; set; }

		public event EventHandler Connected;
		public event EventHandler<MessageReceivedArguments> MessageReceived;
		public event EventHandler MessageSent;

		public X509Certificate serverCertificate = null;

		public bool ValidateServerCertificate(
			  object sender,
			  X509Certificate certificate,
			  X509Chain chain,
			  SslPolicyErrors sslPolicyErrors)
		{
			//if (sslPolicyErrors == SslPolicyErrors.None)
			return true;

			// Do not allow this client to communicate with unauthenticated servers.
			//return false;
		}
		SslStream sslStream;
		public void SetupSSL(X509Certificate2 certificate = null)
		{
			serverCertificate = X509Certificate2.CreateFromCertFile(ConfigurationManager.AppSettings["CertificateLocation"].ToString());

			using (var str = new NetworkStream(_socket))
			{
				sslStream = new SslStream(
					str,
					false,
					new RemoteCertificateValidationCallback(ValidateServerCertificate),
					null
				);
				X509Certificate2 serverCertificate = new X509Certificate2(ConfigurationManager.AppSettings["CertificateLocation"].ToString());
				X509Certificate2Collection col = new X509Certificate2Collection();
				col.Add(serverCertificate);
				Console.WriteLine("Authenticating client...");
				sslStream.AuthenticateAsClient(ConfigurationManager.AppSettings["NdcIp"].ToString(), col, SslProtocols.Tls12, false);

				//sslStream.Write(Message);
			}
		}

		string ReadMessage(SslStream sslStream)
		{
			byte[] buffer = new byte[2048];
			StringBuilder messageData = new StringBuilder();
			int bytes = -1;
			do
			{
				bytes = sslStream.Read(buffer, 0, buffer.Length);

				// Use Decoder class to convert from bytes to UTF8
				// in case a character spans two buffers.
				Decoder decoder = Encoding.UTF8.GetDecoder();
				char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
				decoder.GetChars(buffer, 0, bytes, chars, 0);
				messageData.Append(chars);

				if (messageData.ToString().IndexOf("<EOF>") != -1)
				{
					break;
				}
			} while (bytes != 0);

			return messageData.ToString();
		}
		public void StartClient()
		{
			try
			{
				_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
				_socket.BeginConnect(new IPEndPoint(IPAddress.Parse(Ip), Port), OnConnectCallback, _socket);

				connected.WaitOne();
				Connected?.Invoke(this, EventArgs.Empty);
				Receive();

				//if (Ssl)
				//{
				//	SetupSSL();
				//}
			}
			catch (SocketException)
			{
				// TODO:
			}
		}

		public bool IsConnected()
		{
			return _socket.Connected;
		}

		private void OnConnectCallback(IAsyncResult result)
		{
			var server = (Socket)result.AsyncState;

			try
			{
				server.EndConnect(result);
				connected.Set();
			}
			catch (SocketException)
			{

			}
		}

		public void Receive()
		{
			_socket.BeginReceive(Buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, null);
		}

		private void ReceiveCallback(IAsyncResult result)
		{
			var receive = _socket.EndReceive(result);

			if (receive > 0)
			{
				if (receive == Buffer.Length)
					BufferSize = Math.Min(BufferSize * 10, MaxBufferSize);
				else
				{
					do
					{
						int reducedBufferSize = Math.Max(BufferSize / 10, MinBufferSize);
						if (receive < reducedBufferSize)
							BufferSize = reducedBufferSize;

					}
					while (receive > MinBufferSize);
				}

				byte[] data = new byte[receive];
				Array.Copy(Buffer, data, receive);

				if (ParseCommand(data))
				{

				}
				else
				{
					MessageReceived?.Invoke(this, new MessageReceivedArguments() { Buffer = data });
					received.Set();
				}
			}
			else if (receive == BufferSize)
			{
				_socket.BeginReceive(Buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, null);
			}
		}

		private bool ParseCommand(byte[] data)
		{
			var result = false;

			var dta = DiscardFirst2Bytes(data);
			var message = Encoding.ASCII.GetString(dta);
			var array = message.Split(new char[] { FieldSeparatorChar, EscapeCharacterChar, ShiftOutCharacterChar }, StringSplitOptions.RemoveEmptyEntries);

			if (array != null)
			{
				if (array[0] == "1" && array[1] == "1")
				{
					ComeOnlineCommand();
					result = true;
				}

				if (array[0] == "1" && array[1] == "2")
				{
					GoOfflineCommand();
					result = true;
				}
			}

			return result;
		}

		private void ComeOnlineCommand()
		{
			InService = true;
			// To Do Raise event
			Send(AppendLength2Bytes(GetOnlineResponse()));
		}

		private void GoOfflineCommand()
		{
			InService = false;
			Receive();
			// To Do Raise event
		}

		public byte[] DiscardFirst2Bytes(byte[] data)
		{
			var len = data.Length;
			var buf = new byte[len - 2];

			len = len - 2;
			Array.Copy(data, 2, buf, 0, len);

			return buf;
		}

		public byte[] AppendLength2Bytes(byte[] data)
		{
			var len = data.Length;
			var buf = new byte[len + 2];

			len = len + 2;
			buf[0] = (byte)(len >> 8 & 255);
			buf[1] = (byte)(len & 255);
			Array.Copy(data, 0, buf, 2, len - 2);

			return buf;
		}
		byte[] Message;
		public void Send(byte[] msg)
		{
			if (!IsConnected())
			{
				throw new Exception("Destination socket is not connected.");
			}

			Message = msg;
			_socket.BeginSend(msg, 0, msg.Length, SocketFlags.None, SendCallback, _socket);
		}

		private void SendCallback(IAsyncResult result)
		{
			try
			{
				var resceiver = (Socket)result.AsyncState;
				resceiver.EndSend(result);
			}
			catch (SocketException)
			{
				// TODO:
			}
			catch (ObjectDisposedException)
			{
				// TODO;
			}

			MessageSent?.Invoke(this, EventArgs.Empty);
			sent.Set();
		}

		private void Close()
		{
			try
			{
				if (!IsConnected())
				{
					return;
				}

				_socket.Shutdown(SocketShutdown.Both);
				_socket.Close();
			}
			catch (SocketException)
			{
				// TODO:
			}
		}

		public void Dispose()
		{
			connected.Dispose();
			sent.Dispose();
			received.Dispose();

			Close();
			GC.SuppressFinalize(this);
		}

		private byte[] GetOnlineResponse()
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.Append("2", "Message Class");
			stringBuilder.Append("2", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("9", "Ready Indicator");

			return stringBuilder.ToString().Select(Convert.ToByte).ToArray();
		}

		private byte[] GetOfflineResponse()
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.Append("2", "Message Class");
			stringBuilder.Append("2", "Message Sub-Class");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("000", "LUNO");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append(FieldSeparator, "Field Separator");
			stringBuilder.Append("9", "Ready Indicator");

			return stringBuilder.ToString().Select(Convert.ToByte).ToArray();
		}
	}

	//using System;
	//using System.IO;
	//using System.Net.Sockets;
	//using System.Net.Security;
	//using System.Threading;
	//using System.Threading.Tasks;
	///// <summary>
	///// Credits : https://github.com/sethcall/async-helper/blob/master/src/AsyncTcpClient/AsyncTcpClient.cs
	///// </summary>
	//public class AsyncTcpClient : IDisposable
	//{
	//	private TcpClient tcpClient;
	//	private Stream stream;
	//	private bool disposed = false;
	//	private int bufferSize = 8192;
	//	private int BufferSize
	//	{
	//		get { return bufferSize; }
	//		set
	//		{
	//			bufferSize = value;
	//			if (tcpClient != null)
	//				tcpClient.ReceiveBufferSize = value;
	//		}
	//	}

	//	public bool IsReceiving { set; get; }

	//	public int MinBufferSize { get; set; } = 8192;

	//	public int MaxBufferSize { get; set; } = 15 * 1024 * 1024;

	//	public int SendBufferSize
	//	{
	//		get
	//		{
	//			if (tcpClient != null)
	//				return tcpClient.SendBufferSize;
	//			else
	//				return 0;
	//		}
	//		set
	//		{
	//			if (tcpClient != null)
	//				tcpClient.SendBufferSize = value;
	//		}
	//	}

	//	public event EventHandler<byte[]> OnDataReceived;

	//	public event EventHandler OnDisconnected;

	//	public bool IsConnected
	//	{
	//		get
	//		{
	//			return tcpClient != null && tcpClient.Connected;
	//		}
	//	}

	//	public AsyncTcpClient()
	//	{

	//	}

	//	public async Task SendAsync(byte[] data, CancellationToken token = default(CancellationToken))
	//	{
	//		try
	//		{
	//			await stream.WriteAsync(data, 0, data.Length, token);
	//			await stream.FlushAsync(token);
	//		}
	//		catch (IOException ex)
	//		{
	//			var onDisconnected = OnDisconnected;
	//			if (ex.InnerException is ObjectDisposedException)
	//			{
	//				Console.WriteLine("innocuous ssl stream error");
	//				// for SSL streams
	//			}
	//			else
	//			{
	//				onDisconnected?.Invoke(this, EventArgs.Empty);
	//			}
	//		}
	//	}

	//	public async Task ConnectAsync(string host, int port, bool ssl = false, CancellationToken cancellationToken = default(CancellationToken))
	//	{
	//		try
	//		{
	//			//Connect async method
	//			await CloseAsync();
	//			cancellationToken.ThrowIfCancellationRequested();
	//			tcpClient = new TcpClient();
	//			cancellationToken.ThrowIfCancellationRequested();
	//			await tcpClient.ConnectAsync(host, port);
	//			await CloseIfCanceled(cancellationToken);
	//			// get stream and do SSL handshake if applicable

	//			stream = tcpClient.GetStream();
	//			await CloseIfCanceled(cancellationToken);
	//			if (ssl)
	//			{
	//				var sslStream = new SslStream(stream);
	//				await sslStream.AuthenticateAsClientAsync(host);
	//				stream = sslStream;
	//				await CloseIfCanceled(cancellationToken);
	//			}
	//		}
	//		catch (Exception)
	//		{
	//			CloseIfCanceled(cancellationToken).Wait();
	//			throw;
	//		}
	//	}

	//	public async Task Receive(CancellationToken token = default(CancellationToken))
	//	{
	//		try
	//		{
	//			if (!IsConnected || IsReceiving)
	//				throw new InvalidOperationException();

	//			IsReceiving = true;
	//			byte[] buffer = new byte[bufferSize];

	//			while (IsConnected)
	//			{
	//				token.ThrowIfCancellationRequested();

	//				int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
	//				if (bytesRead > 0)
	//				{
	//					if (bytesRead == buffer.Length)
	//						BufferSize = Math.Min(BufferSize * 10, MaxBufferSize);
	//					else
	//					{
	//						do
	//						{
	//							int reducedBufferSize = Math.Max(BufferSize / 10, MinBufferSize);
	//							if (bytesRead < reducedBufferSize)
	//								BufferSize = reducedBufferSize;

	//						}

	//						while (bytesRead > MinBufferSize);
	//					}

	//					var onDataReceived = OnDataReceived;
	//					if (onDataReceived != null)
	//					{
	//						byte[] data = new byte[bytesRead];
	//						Array.Copy(buffer, data, bytesRead);
	//						onDataReceived(this, data);
	//					}
	//				}

	//				buffer = new byte[bufferSize];
	//			}
	//		}
	//		catch (ObjectDisposedException)
	//		{
	//			Console.WriteLine("ODE Exception in receive");
	//		}
	//		catch (IOException ex)
	//		{
	//			var evt = OnDisconnected;
	//			if (ex.InnerException is ObjectDisposedException)
	//			{
	//				Console.WriteLine("innocuous ssl stream error");
	//				// for SSL streams
	//			}

	//			evt?.Invoke(this, EventArgs.Empty);
	//		}
	//		finally
	//		{
	//			IsReceiving = false;
	//		}
	//	}

	//	public async Task CloseAsync()
	//	{
	//		await Task.Yield();
	//		Close();
	//	}

	//	private void Close()
	//	{
	//		if (tcpClient != null)
	//		{
	//			tcpClient.Dispose();
	//			tcpClient = null;
	//		}
	//		if (stream != null)
	//		{
	//			stream.Dispose();
	//			stream = null;
	//		}
	//	}

	//	private async Task CloseIfCanceled(CancellationToken token, Action onClosed = null)
	//	{
	//		if (token.IsCancellationRequested)
	//		{
	//			await CloseAsync();
	//			onClosed?.Invoke();
	//			token.ThrowIfCancellationRequested();
	//		}
	//	}

	//	protected virtual void Dispose(bool disposing)
	//	{
	//		if (disposing)
	//		{
	//			if (!disposed)
	//			{
	//				Close();
	//			}
	//		}

	//		disposed = true;

	//		// If it is available, make the call to the
	//		// base class's Dispose(Boolean) method
	//		// base.Dispose(disposing);
	//	}

	//	public void Dispose()
	//	{
	//		Dispose(true);
	//		GC.SuppressFinalize(this);
	//	}
	//}
}