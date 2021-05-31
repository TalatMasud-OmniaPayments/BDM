namespace Omnia.Pie.Vtm.Communication.Interface
{
	using Newtonsoft.Json.Linq;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public interface IESpaceTerminalCommunication : IDisposable
	{
		event EventHandler<CallEventArgs> CallEnded;
		event EventHandler CurrentSessionEnded;
		event EventHandler InvalidateCurrentSession;
		event EventHandler<RTMessageEventArgs> RTMessageReceived;
		event EventHandler<CallEventArgs> CallInitiatedEvent;
		event EventHandler<CallEventArgs> CallEstablishedEvent;

		Task<CallResult> CallTeller(CallRequest request);
		Task<CallResult> CallTeller(CancellationToken token, CallRequest request);
		bool IsInCall();
		short Logout();

		bool SendMessage(string message);
		bool SendStatus(StatusEnum statusCode, object properties = null);
		bool SendDeviceStatus(int deviceType, int deviceStatus, object properties = null);
	}

	public class CallRequest
	{
		public string LocalVideoHandle { get; set; }
		public string RemoteVideoHandle { get; set; }
		public CallMode CallMode { get; set; }
	}

	public interface IRTMessage
	{
		int Code { get; }
		string GetProperty(string propertyName);
	}

	public enum CallResult
	{
		NetWorkError,
		LoginFailed,
		NoTellerAvailable,
		NoTellerLoggedIn,
		CallCanceled,
		Success,
	}

	public enum LoginResult
	{
		Success,
		NoResponse,
		IncorrectCredentials,
		AlreadyLoggedIn,
		StatusError,
		MissingConfiguration,
		LoginFailed,
	}

	public enum CallMode
	{
		Audio = 0,
		AudioAndVideo = 1,
		OneWayVideo = 2
	}

	public class CallEventArgs : EventArgs
	{
		public string CallId { get; set; }
		public string CallMode { get; set; }
		public string Message { get; set; }
		public string RemoteNumber { get; set; }
		public string RetCode { get; set; }
		public string RoleType { get; set; }
	}

	public class RTMessageEventArgs : EventArgs
	{
		public RTMessageEventArgs(IRTMessage _messageCode)
		{
			MessageCode = _messageCode;
		}

		public IRTMessage MessageCode { get; private set; }
	}

	public class RTMessage : IRTMessage
	{
		public RTMessage(int commandCode, string jsonData)
		{
			Code = commandCode;
			JsonData = jsonData;
		}

		public int Code { get; private set; }
		private string JsonData { get; set; }

		public string GetProperty(string propertyName)
		{
			string propertyValue = JsonParser.FindValueFromJson(JsonData, propertyName);
			return propertyValue;
		}
	}

	public class JsonParser
	{
		private static JsonParser parser;
		public static JsonParser Instance()
		{
			if (parser == null)
			{
				parser = new JsonParser();
			}

			return parser;
		}

		public static string FindValueFromJson(string sMsg, string retValue)
		{
			string returnValue = string.Empty;

			if (!string.IsNullOrEmpty(sMsg) && !string.IsNullOrEmpty(retValue))
			{
				var job = JObject.Parse(sMsg);

				if (job != null)
				{
					if (null != job[retValue] && !string.IsNullOrEmpty(job[retValue].ToString()))
					{
						returnValue = job[retValue].ToString();
					}
				}
			}

			return returnValue;
		}

		public JsonData ParseEspaceMessage(string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				var objData = new JsonData();
				if (!string.IsNullOrEmpty(str))
				{
					var parsedData = JObject.Parse(str);

					if (!string.IsNullOrEmpty(parsedData["Data"].ToString()))
						objData.Data = parsedData["Data"].ToString();

					if (!string.IsNullOrEmpty(parsedData["DataLen"].ToString()))
						objData.DataLength = parsedData["DataLen"].ToString();

					if (!string.IsNullOrEmpty(parsedData["SenderID"].ToString()))
						objData.SenderID = parsedData["SenderID"].ToString();

					if (!string.IsNullOrEmpty(parsedData["retcode"].ToString()))
						objData.StatusCode = parsedData["retcode"].ToString();
				}

				return objData;
			}
			else
			{
				return new JsonData();
			}
		}
	}

	public struct JsonData
	{
		public string Data;
		public string DataLength;
		public string SenderID;
		public string StatusCode;
	}
}