namespace Omnia.Pie.Vtm.Services
{
	using Newtonsoft.Json;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.ISO;
	using System;
    using System.Configuration;
    using System.Diagnostics;
	using System.Globalization;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Runtime.CompilerServices;
	using System.ServiceModel;
	using System.Text;
	using System.Threading.Tasks;
	using System.Web;

	/// <summary>
	///	 Credits: https://github.com/systemsymbiosis/PublishSubscribeWithDiscovery/blob/master/ServiceLibrary/FaultHandledOperations.cs
	///	 For WCF Exceptions docs see 
	///	 http://msdn.microsoft.com/en-us/library/ms789039.aspx 	//	 http://delicious.com/austindimmer/WCF+exceptions
	/// </summary>
	public abstract class ServiceBase
	{
		protected IResolver _container { get; }
		protected ILogger _logger { get; }
		protected IJournal _journal { get; }
		private IServiceEndpoint EndpointsProvider { get; }
		public IServiceManager ServiceManager { get; }
		private IContractExceptionManager ContractExceptionManager { get; }

        private bool LogReqResp { get; set; }

        public ServiceBase(IResolver container, IServiceEndpoint endpointsProvider)
		{
			_container = container;
			_logger = container.Resolve<ILogger>();
			try { _journal = container.Resolve<IJournal>(); } catch { }

            ServiceManager = container.Resolve<IServiceManager>();
			EndpointsProvider = endpointsProvider ?? throw new ArgumentNullException(nameof(endpointsProvider));
			ContractExceptionManager = container.Resolve<IContractExceptionManager>();

            LogReqResp = (ConfigurationManager.AppSettings["AppState"].ToString() == "TraceState" ? true : false);

            //_logger.Info("Log request response: " + LogReqResp);
        }

		protected T ExecuteFaultHandledOperation<T>(Func<T> codetoExecute)
		{
			try
			{
				return codetoExecute.Invoke();
			}
			catch (ArgumentException ex)
			{
				Debug.WriteLine("The service host has a problem ArgumentException. " + ex.Message);
				throw new FaultException<ArgumentException>(ex, ex.Message);
			}
			catch (AuthorizationValidationException ex)
			{
				throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				Debug.WriteLine("The service operation timed out. " + ex.Message);
				throw new FaultException<Framework.Exceptions.TimeoutException>(ex, ex.Message);
			}
			catch (AddressAlreadyInUseException ex)
			{
				Debug.WriteLine("An AddressAlreadyInUseException was received. " + ex.Message);
				throw new FaultException<AddressAlreadyInUseException>(ex, ex.Message);
			}
			catch (AddressAccessDeniedException ex)
			{
				Debug.WriteLine("An AddressAccessDeniedException was received. " + ex.Message);
				throw new FaultException<AddressAccessDeniedException>(ex, ex.Message);
			}
			catch (CommunicationObjectFaultedException ex)
			{
				Debug.WriteLine("A CommunicationObjectFaultedException was received. " + ex.Message);
				throw new FaultException<CommunicationObjectFaultedException>(ex, ex.Message);
			}
			catch (CommunicationObjectAbortedException ex)
			{
				Debug.WriteLine("A CommunicationObjectAbortedException was received. " + ex.Message);
				throw new FaultException<CommunicationObjectAbortedException>(ex, ex.Message);
			}
			catch (EndpointNotFoundException ex)
			{
				Debug.WriteLine("An EndpointNotFoundException was received. " + ex.Message);
				throw new FaultException<EndpointNotFoundException>(ex, ex.Message);
			}
			catch (ProtocolException ex)
			{
				Debug.WriteLine("A ProtocolException was received. " + ex.Message);
				throw new FaultException<ProtocolException>(ex, ex.Message);
			}
			catch (ServerTooBusyException ex)
			{
				Debug.WriteLine("A ServerTooBusyException was received. " + ex.Message);
				throw new FaultException<ServerTooBusyException>(ex, ex.Message);
			}
			catch (ObjectDisposedException ex)
			{
				Debug.WriteLine("An ObjectDisposedException was received. " + ex.Message);
				throw new FaultException<ObjectDisposedException>(ex, ex.Message);
			}
			catch (FaultException ex)
			{
				Debug.WriteLine("An FaultException was received. " + ex.Message);
				throw;
			}
			catch (CommunicationException ex)
			{
				Debug.WriteLine("There was a communication problem. " + ex.Message + ex.StackTrace);
				throw new FaultException<CommunicationException>(ex, ex.Message);
			}
			catch (Exception)
			{
				throw;
			}
		}

		protected void ExecuteFaultHandledOperation(Action codetoExecute)
		{
			//USAGE

			//FaultHandledOperations.ExecuteFaultHandledOperation(() =>
			//{
			//    //Code here

			//});

			try
			{
				codetoExecute.Invoke();
			}
			// For WCF Exceptions docs see http://msdn.microsoft.com/en-us/library/ms789039.aspx
			// http://delicious.com/austindimmer/WCF+exceptions
			catch (AuthorizationValidationException ex)
			{
				throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
			}
			catch (Framework.Exceptions.TimeoutException ex)
			{
				Debug.WriteLine("The service operation timed out. " + ex.Message);
				throw new FaultException<Framework.Exceptions.TimeoutException>(ex, ex.Message);
			}
			catch (AddressAlreadyInUseException ex)
			{
				Debug.WriteLine("An AddressAlreadyInUseException was received. " + ex.Message);
				throw new FaultException<AddressAlreadyInUseException>(ex, ex.Message);
			}
			catch (AddressAccessDeniedException ex)
			{
				Debug.WriteLine("An AddressAccessDeniedException was received. " + ex.Message);
				throw new FaultException<AddressAccessDeniedException>(ex, ex.Message);
			}
			catch (CommunicationObjectFaultedException ex)
			{
				Debug.WriteLine("A CommunicationObjectFaultedException was received. " + ex.Message);
				throw new FaultException<CommunicationObjectFaultedException>(ex, ex.Message);
			}
			catch (CommunicationObjectAbortedException ex)
			{
				Debug.WriteLine("A CommunicationObjectAbortedException was received. " + ex.Message);
				throw new FaultException<CommunicationObjectAbortedException>(ex, ex.Message);
			}
			catch (EndpointNotFoundException ex)
			{
				Debug.WriteLine("An EndpointNotFoundException was received. " + ex.Message);
				throw new FaultException<EndpointNotFoundException>(ex, ex.Message);
			}
			catch (ProtocolException ex)
			{
				Debug.WriteLine("A ProtocolException was received. " + ex.Message);
				throw new FaultException<ProtocolException>(ex, ex.Message);
			}
			catch (ServerTooBusyException ex)
			{
				Debug.WriteLine("A ServerTooBusyException was received. " + ex.Message);
				throw new FaultException<ServerTooBusyException>(ex, ex.Message);
			}
			catch (ObjectDisposedException ex)
			{
				Debug.WriteLine("An ObjectDisposedException was received. " + ex.Message);
				throw new FaultException<ObjectDisposedException>(ex, ex.Message);
			}
			catch (FaultException ex)
			{
				Debug.WriteLine("An FaultException was received. " + ex.Message);
				throw;
			}
			catch (CommunicationException ex)
			{
				Debug.WriteLine("There was a communication problem. " + ex.Message + ex.StackTrace);
				throw new FaultException<CommunicationException>(ex, ex.Message);
			}
			catch (Exception)
			{
				throw;
			}
		}

		protected async Task<TResult> ExecuteFaultHandledOperationAsync<TRequest, TResult>(Func<TRequest, Task<TResult>> codetoExecute) where TRequest : RequestBase
		{
			var result = default(TResult);

			try
			{
				var request = CreateRequest<TRequest>();
				result = await codetoExecute(request);
			}
			catch (ArgumentException ex)
			{
				_logger.Error("The service host has a problem ArgumentException. " + ex.Message);
				throw new FaultException<ArgumentException>(ex, ex.Message);
			}
			catch (AuthorizationValidationException ex)
			{
				throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
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

		protected async Task ExecuteFaultHandledOperationAsync<TRequest>(Func<TRequest, Task> codetoExecute) where TRequest : RequestBase
		{
			try
			{
				var client = CreateRequest<TRequest>();
				await codetoExecute(client);
			}
			catch (ArgumentException ex)
			{
				_logger.Error("The service host has a problem ArgumentException. " + ex.Message);
				throw new FaultException<ArgumentException>(ex, ex.Message);
			}
			catch (AuthorizationValidationException ex)
			{
				throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
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
		}

		public virtual TServiceRequest CreateRequest<TServiceRequest>() where TServiceRequest : RequestBase
		{
			var req = _container?.Resolve<TServiceRequest>();
			return req;
		}

		private VTMTerminal BuildVTMTerminal(Terminal terminal) => new VTMTerminal
		{
			TerminalId = terminal.Id,
			BranchId = terminal.BranchId,
			Platform = terminal.Platform,
            MachineSerialNo = terminal.MachineSerialNo,
            IPAddress = ConfigurationManager.AppSettings["TransactionDataIP"].ToString() ?? string.Empty,
            CountryCode = terminal.CountryCode,
            TerminalLanguage = terminal.TerminalLanguage,
            //MerchantId = terminal.MerchantId,
            //Type = terminal.Type,
            //OwnerName = terminal.OwnerName,
            //StateName = terminal.StateName
        };

		public TServiceRequest AppendRequestData<TServiceRequest>(TServiceRequest req) where TServiceRequest : RequestBase
		{
			if (ServiceManager.Acquirer == null)
				throw new ServiceException($"{nameof(ServiceManager.Acquirer)} is null.");

			if (ServiceManager.Terminal == null)
				throw new ServiceException($"{nameof(ServiceManager.Terminal)} is null.");

			req.Terminal = BuildVTMTerminal(ServiceManager.Terminal);
            var sessionId = System.Guid.NewGuid().ToString();
            //string timestamp = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.sssZ");
            string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");

            //string timestamp = String.Format("{ 0:yyyy-MM-ddTHH:mm:ssZ}", System.DateTime.Now);

            //String.Format ("")

            req.TransactionData = new TransactionData()
            {
                TransactionType = string.Empty,
                SessionId = sessionId,
                TransactionNumber = ServiceManager?.TransactionNumber ?? string.Empty,
                SessionLanguage = ServiceManager.Terminal.TerminalLanguage,
                MessageTimestamp = timestamp
            };

			return req;
		}

		protected async Task<TResponse> ExecuteServiceAsync<TRequest, TResponse>(TRequest request, [CallerMemberName] string operation = null) where TRequest : RequestBase
		{
            request = AppendRequestData(request);
			return await ExecuteServiceAsync<TRequest, TResponse>(operation, request, EndpointsProvider.TimeoutSeconds);
		}

		private async Task<TResponse> ExecuteServiceAsync<TRequest, TResponse>(string contract, TRequest request, int timeoutSeconds)
		{

            try
            {
                _logger?.Info($"Call Service - Request Contract: {contract} ");
            }
            catch (Exception)
            {
                _logger?.Info($"Request Contract is Null");
            }

            const string MediaType = "application/json";
			var baseAddress = EndpointsProvider.BaseAddress;
			var requestUri = EndpointsProvider.GetContractAddress(contract);

			using (var client = CreateHttpClient(timeoutSeconds, MediaType))
			{
				var requestStr = SerializeData(request);

                if (LogReqResp) { 
                    _logger?.Info($" Start === {baseAddress} => {contract} => {requestStr} === End");
                }
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
				var requestContent = new StringContent(requestStr, Encoding.UTF8, MediaType);
				var response = await client.PostAsync($"{baseAddress}{requestUri}", requestContent);
				response.EnsureSuccessStatusCode();

				var responseContent = await response.Content.ReadAsStringAsync();
				var responseStr = HttpUtility.HtmlDecode(responseContent);

                if (LogReqResp)
                    _logger?.Info($" Start === {contract} => {responseStr} === End");

                var result = DeSerializeData<TResponse>(responseStr);
				EnsureSuccess(contract, result);
				return result;
			}
		}

		private string SerializeData(dynamic obj) => JsonConvert.SerializeObject(obj);
		private dynamic DeSerializeData<T>(string Data) => JsonConvert.DeserializeObject<T>(Data);
		private static HttpMessageHandler CreateHttpMessageHandler() => new WebRequestHandler();
		private static HttpClient CreateHttpClient(int timeoutSeconds, string mediaType)
		{
			var client = new HttpClient(CreateHttpMessageHandler(), disposeHandler: true)
			{
				Timeout = TimeSpan.FromSeconds(timeoutSeconds)
			};

			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
			//client.DefaultRequestHeaders.Add("Authorization", username/password);

			return client;
		}

		private void EnsureSuccess(string contract, IResponse message)
		{
			if (message.Error != null && message.ResponseCode != "050")
			{
                
                var errorCode = message?.Error?.ErrorCode;
                var errorMessage = message?.Error?.ErrorMessageText;

                _logger?.Error($"Service : {contract} has returned the following error: code = \"{errorCode}\", message = \"{errorMessage}\".");
                _journal?.Write($"{contract} returned error code :{errorCode}");

                var exception = ContractExceptionManager.GetContractException(contract, errorCode, errorMessage);
                var exceptionInstance = CreateException(exception);
                if (exceptionInstance != null)
                    throw exceptionInstance;

            }
		}

		private static ServiceException CreateException(string exception)
		{
			if (exception == "ignore")
				return null;

			if (string.IsNullOrEmpty(exception))
				return new ServiceException();

			var p = exception.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
			var typeName = p[0];
			var assemblyName = p[1];
			var handle = Activator.CreateInstance(assemblyName, typeName);

			return (ServiceException)handle.Unwrap();
		}

		#region Utility Functions

		public static double? ToNullableDouble(string str)
		{
			double result;

			if (double.TryParse(str, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			else
			{
				return null;
			}
		}

		protected static string ToISOAmount(string amount)
		{
			var result = string.Empty;

			try
			{
				decimal amountD = 0;
				Decimal.TryParse(amount, out amountD);
				amountD = amountD * 100;
				result = int.Parse(amountD.ToString()).ToString("D12");
			}
			catch (Exception)
			{

			}

			return result;
		}

		public static string ToString(DateTime? v, DateFormat format = DateFormat.Default) => v?.ToString(ToDateTimeFormatString(format));

		public static DateTime? ToNullableDateTime(string str, DateFormat format = DateFormat.Default)
		{
			DateTime result;

			if (DateTime.TryParseExact(str, ToDateTimeFormatString(format), CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
			{
				return result;
			}
			else
			{
				return null;
			}
		}

		public enum DateFormat
		{
			yyyyMMddHHmm,
			yyyyMMddhhmm,
			ddMMyyyy,
			yyyyMMdd,
			yyyyMMddDashed,
			MMddyyyy,
            yyyyMMddTHHmmsssssZ,
            yyyyMMddTHHmmss,
            Default = yyyyMMdd,
		}

		private static string ToDateTimeFormatString(DateFormat df)
		{
			string result;

			switch (df)
			{
				case DateFormat.ddMMyyyy:
					result = "dd/MM/yyyy";
					break;
				case DateFormat.yyyyMMdd:
					result = "yyyyMMdd";
					break;
				case DateFormat.MMddyyyy:
					result = "MM/dd/yyyy";
					break;
				case DateFormat.yyyyMMddHHmm:
					result = "yyyyMMddHHmm";
					break;
				case DateFormat.yyyyMMddDashed:
					result = "yyyy-MM-dd";
					break;
                case DateFormat.yyyyMMddTHHmmsssssZ:
                    result = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
                    break;
                case DateFormat.yyyyMMddTHHmmss:
                    result = "yyyy-MM-dd HH:mm:ss";
                    break;

                default:
					throw new NotSupportedException();
			}

			return result;
		}

		#endregion
	}
}