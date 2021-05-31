namespace Omnia.Pie.Vtm.ServicesNdc
{
	using Omnia.Pie.Vtm.Framework.Extensions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Tcp;
    using Omnia.Pie.Vtm.ServicesNdc.Interface;
    using System;
	using System.ServiceModel;
	using System.Text;
	using System.Threading.Tasks;

	public abstract class ServiceBase : IDisposable
	{
		private IResolver _container { get; }
		public ILogger _logger { get; }
		//public ISSLClient NdcTcpClient;
        public INdcClient NdcClient;

        public const string FieldSeparator = "\u001c";
		public const string GroupSeparator = "\u001d";
		public const string EscapeCharacter = "\u001b";
		public const string GreaterThan = "\u003e";
		public const string ShiftOutCharacter = "\u000e";

		public Char FieldSeparatorChar = '\u001c';
		public Char EscapeCharacterChar = '\u001b';
		public Char ShiftOutCharacterChar = '\u000e';
		public Char FieldCharacterChar = '\u000f';

		public ServiceBase(IResolver container)
		{
			_container = container;
			_logger = container.Resolve<ILogger>();

            _logger?.Info($"{GetType()} => Resolving INdcClient.");

            NdcClient = container.Resolve<INdcClient>();

            _logger?.Info($"{GetType()} => StartClientAsync.");
            NdcClient.StartClientAsync();
            
        }

		protected async Task<string> ExecuteServiceAsync<TRequest, TResponse>(TRequest request, bool waitForResponse = true)
		{
			return await ExecuteServiceAsync(request, waitForResponse);
		}

		private async Task<string> ExecuteServiceAsync<TRequest>(TRequest request, bool waitForResponse)
		{
            return await NdcClient.SendAsync(NdcClient.AppendLength2Bytes(ObjectToByteArray(request)), waitForResponse);
        }

		public byte[] ObjectToByteArray(Object obj)
		{
			return Encoding.UTF8.GetBytes(obj.ToString());
		}

		public string BuildHeader()
		{
			var stringBuilder = new StringBuilder();
			var value = AdjustDataFillRight("CWD", 3);

			stringBuilder.Append(FieldSeparator, "    Field Separator");
			stringBuilder.Append(value, "[d] Logical Unit Number");
			stringBuilder.Append("terminalNumber", "[d] Security Terminal Number");
			stringBuilder.Append(FieldSeparator, "    Field Separator");
			stringBuilder.Append(FieldSeparator, "    Field Separator");

			return stringBuilder.ToStringExtend();
		}

		public static string AdjustDataFillRight(string sourceData, int expectedLength)
		{
			if (expectedLength == 0)
			{
				throw new ArgumentOutOfRangeException("pExpectedLength", "Wrong parameter");
			}
			if (sourceData.Length != expectedLength)
			{
				if (sourceData.Length < expectedLength)
				{
					int count = expectedLength - sourceData.Length;
					sourceData += new string('0', count);
				}
				else
				{
					sourceData = sourceData.Substring(0, expectedLength);
				}
			}

			return sourceData;
		}

		protected async Task<TResult> ExecuteFaultHandledOperationAsync<TRequest, TResult>(Func<string, Task<TResult>> codetoExecute)
		{
			var result = default(TResult);

			try
			{
				result = await codetoExecute(string.Empty);
			}
			catch (ArgumentException ex)
			{
				_logger.Error("The service host has a problem ArgumentException. " + ex.Message);
				throw new FaultException<ArgumentException>(ex, ex.Message);
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				_logger.Error("The service operation timed out. " + ex.Message);
				throw new FaultException<Framework.Exceptions.TimeoutException>(ex, ex.Message);
			}
			catch (AddressAlreadyInUseException ex)
			{
				_logger.Error("An AddressAlreadyInUseException was received. " + ex.Message);
				throw new FaultException<AddressAlreadyInUseException>(ex, ex.Message);
			}
			catch (AddressAccessDeniedException ex)
			{
				_logger.Error("An AddressAccessDeniedException was received. " + ex.Message);
				throw new FaultException<AddressAccessDeniedException>(ex, ex.Message);
			}
			catch (CommunicationObjectFaultedException ex)
			{
				_logger.Error("A CommunicationObjectFaultedException was received. " + ex.Message);
				throw new FaultException<CommunicationObjectFaultedException>(ex, ex.Message);
			}
			catch (CommunicationObjectAbortedException ex)
			{
				_logger.Error("A CommunicationObjectAbortedException was received. " + ex.Message);
				throw new FaultException<CommunicationObjectAbortedException>(ex, ex.Message);
			}
			catch (EndpointNotFoundException ex)
			{
				_logger.Error("An EndpointNotFoundException was received. " + ex.Message);
				throw new FaultException<EndpointNotFoundException>(ex, ex.Message);
			}
			catch (ProtocolException ex)
			{
				_logger.Error("A ProtocolException was received. " + ex.Message);
				throw new FaultException<ProtocolException>(ex, ex.Message);
			}
			catch (ServerTooBusyException ex)
			{
				_logger.Error("A ServerTooBusyException was received. " + ex.Message);
				throw new FaultException<ServerTooBusyException>(ex, ex.Message);
			}
			catch (ObjectDisposedException ex)
			{
				_logger.Error("An ObjectDisposedException was received. " + ex.Message);
				throw new FaultException<ObjectDisposedException>(ex, ex.Message);
			}
			catch (FaultException ex)
			{
				_logger.Error("An FaultException was received. " + ex.Message);
				throw;
			}
			catch (CommunicationException ex)
			{
				_logger.Error("There was a communication problem. " + ex.Message + ex.StackTrace);
				throw new FaultException<CommunicationException>(ex, ex.Message);
			}
			catch (Exception)
			{
				throw;
			}

			return result;
		}

		public void Dispose()
		{

		}
	}
}