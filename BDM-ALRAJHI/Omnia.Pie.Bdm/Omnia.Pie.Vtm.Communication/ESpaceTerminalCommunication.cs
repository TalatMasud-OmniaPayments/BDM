namespace Omnia.Pie.Vtm.Communication
{
	using AxeSpaceMediaLib;
	using Newtonsoft.Json.Linq;
	using Omnia.Pie.Vtm.Communication.Interface;
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.Framework.Extensions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Configuration;
	using System.Threading;
	using System.Threading.Tasks;

	public class ESpaceTerminalCommunication : IESpaceTerminalCommunication
	{
		#region Private Fields

		private AxeSpaceMedia _eSpaceMedia;
		private readonly IResolver _container;
		private readonly ILogger _logger;
		private TaskCompletionSource<LoginResult> _loginTaskCompletion;
		private TaskCompletionSource<CallResult> _callTellerTaskCompletion;
		private bool _inCall;

		public event EventHandler<CallEventArgs> CallEnded;
		public event EventHandler CurrentSessionEnded;
		public event EventHandler InvalidateCurrentSession;
		public event EventHandler<RTMessageEventArgs> RTMessageReceived;
		public event EventHandler<CallEventArgs> CallInitiatedEvent;
		public event EventHandler<CallEventArgs> CallEstablishedEvent;

		#endregion

		public ESpaceTerminalCommunication(IResolver container)
		{
			var host = new ESpaceTerminalHost();
			_eSpaceMedia = host.Media;
			_container = container;
			_logger = container.Resolve<ILogger>();

			_eSpaceMedia.OcxInitSuccEvent += _eSpaceMedia_OcxInitSuccEvent;
			_eSpaceMedia.TerminalLoginSuccEvent += _eSpaceMedia_TerminalLoginSuccEvent;

			_eSpaceMedia.TerminalLogoutFailEvent += _eSpaceMedia_TerminalLogoutFailEvent;
			_eSpaceMedia.TerminalRingBackEvent += _eSpaceMedia_TerminalRingBackEvent;
			_eSpaceMedia.TerminalTalkingEvent += _eSpaceMedia_TerminalTalkingEvent;
			_eSpaceMedia.MsgArrivedEvent += _eSpaceMedia_MsgArrivedEvent;
			_eSpaceMedia.TerminalWaitingInQueueEvent += _eSpaceMedia_TerminalWaitingInQueueEvent;
			_eSpaceMedia.TerminalCallingReleaseEvent += _eSpaceMedia_TerminalCallingReleaseEvent;

			_eSpaceMedia.TerminalLoginFailEvent += _eSpaceMedia_TerminalLoginFailEvent;
			_eSpaceMedia.TerminalLogoutSuccEvent += _eSpaceMedia_TerminalLogoutSuccEvent;
			_eSpaceMedia.OpenCameraFailedEvent += _eSpaceMedia_OpenCameraFailedEvent;
		}

		#region ESpace Events

		private void _eSpaceMedia_OcxInitSuccEvent(object sender, EventArgs e)
		{
			_logger.Info(nameof(_eSpaceMedia_OcxInitSuccEvent));
		}

		private void _eSpaceMedia_TerminalLoginSuccEvent(object sender, EventArgs e)
		{
			_loginTaskCompletion.TrySetResult(LoginResult.Success);
			_logger.Info("eSpace Logged In Successfully.");
		}

		private void _eSpaceMedia_TerminalLoginFailEvent(object sender, _DeSpaceMediaEvents_TerminalLoginFailEventEvent e)
		{
			_loginTaskCompletion.TrySetResult(LoginResult.LoginFailed);
			_loginTaskCompletion = null; // Need to test, not sure if its needed here
			_logger.Info("eSpace Logged In Failed.");
		}

		private void _eSpaceMedia_TerminalLogoutSuccEvent(object sender, EventArgs e)
		{
			_loginTaskCompletion = null;
			_logger.Info("eSpace Logout Successfully.");
		}

		private void _eSpaceMedia_TerminalWaitingInQueueEvent(object sender, EventArgs e)
		{
			_logger.Info("eSpace _eSpaceMedia_TerminalWaitingInQueueEvent.");
			_callTellerTaskCompletion.TrySetResult(CallResult.NoTellerAvailable);
			ReleaseCall();
			Logout();
		}

		private void _eSpaceMedia_TerminalTalkingEvent(object sender, _DeSpaceMediaEvents_TerminalTalkingEventEvent e)
		{
			_callTellerTaskCompletion.TrySetResult(CallResult.Success);
			StartDesktopSharing();
			CallEstablishedEvent.Invoke(sender, null);
			_inCall = true;
		}

		private void _eSpaceMedia_MsgArrivedEvent(object sender, _DeSpaceMediaEvents_MsgArrivedEventEvent e)
		{
			var data = JsonParser.Instance().ParseEspaceMessage(e.sEventInfo);
			var messageType = JsonParser.FindValueFromJson(data.Data, "MessageType");
			var commandCode = JsonParser.FindValueFromJson(data.Data, "StatusEnum");

			StatusEnum parsedCommandCode;
			if (!Enum.TryParse(commandCode, out parsedCommandCode))
			{
				_logger.Error($"{GetType()}: Can't parse StatusEnum: [{commandCode}].");
				return;
			}

			switch (parsedCommandCode)
			{
				case StatusEnum.ResetCall:
					{
						InvalidateCurrentSession?.Invoke(this, EventArgs.Empty);
						break;
					}
				case StatusEnum.EndCurrentSession:
				case StatusEnum.AuthenticationFailedEndCurrentSession:
					{
						CurrentSessionEnded?.Invoke(this, EventArgs.Empty);
						break;
					}
				case StatusEnum.BackButtonCode:
				case StatusEnum.Back:
				case StatusEnum.ParseException:
					break;
				default:
					{
						RTMessageReceived?.Invoke(this, new RTMessageEventArgs(new RTMessage((int)parsedCommandCode, data.Data)));
						break;
					}
			}
		}

		private void _eSpaceMedia_TerminalCallingReleaseEvent(object sender, _DeSpaceMediaEvents_TerminalCallingReleaseEventEvent e)
		{
			var info = e.sEventInfo;
			var parsedData = JObject.Parse(info);
			var eventArgs = new CallEventArgs();

			if (!string.IsNullOrEmpty(parsedData["callid"].ToString()))
				eventArgs.CallId = parsedData["callid"].ToString();

			if (!string.IsNullOrEmpty(parsedData["callmode"].ToString()))
				eventArgs.CallMode = parsedData["callmode"].ToString();

			if (!string.IsNullOrEmpty(parsedData["remote_number"].ToString()))
				eventArgs.RemoteNumber = parsedData["remote_number"].ToString();

			if (!string.IsNullOrEmpty(parsedData["roletype"].ToString()))
				eventArgs.RoleType = parsedData["roletype"].ToString();

			if (!string.IsNullOrEmpty(parsedData["retcode"].ToString()))
				eventArgs.RetCode = parsedData["retcode"].ToString();

			if (!string.IsNullOrEmpty(parsedData["message"].ToString()))
				eventArgs.Message = parsedData["message"].ToString();

			var jData = JsonParser.FindValueFromJson(e.sEventInfo, "retcode");
			if (jData == "9025")
			{
				//9025 means no teller is even logged in
				eventArgs.RetCode = "9025";
				_callTellerTaskCompletion?.TrySetResult(CallResult.NoTellerLoggedIn);
			}
			else
			{
				CallEnded?.Invoke(this, eventArgs);
			}

			_callTellerTaskCompletion = null;
			Logout();
			_inCall = false;
		}

		private void _eSpaceMedia_TerminalRingBackEvent(object sender, _DeSpaceMediaEvents_TerminalRingBackEventEvent e)
		{
			try
			{
				_logger.Info(nameof(_eSpaceMedia_TerminalRingBackEvent));

				var info = e.sEventInfo;
				var parsedData = JObject.Parse(info);
				var eventArgs = new CallEventArgs();

				if (!string.IsNullOrEmpty(parsedData["callid"].ToString()))
					eventArgs.CallId = parsedData["callid"].ToString();

				if (!string.IsNullOrEmpty(parsedData["callmode"].ToString()))
					eventArgs.CallMode = parsedData["callmode"].ToString();

				if (!string.IsNullOrEmpty(parsedData["remote_number"].ToString()))
					eventArgs.RemoteNumber = parsedData["remote_number"].ToString();

				if (!string.IsNullOrEmpty(parsedData["roletype"].ToString()))
					eventArgs.RoleType = parsedData["roletype"].ToString();

				if (!string.IsNullOrEmpty(eventArgs.CallId))
				{
					CallInitiatedEvent?.Invoke(this, eventArgs);
				}
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
			}
		}

		private void _eSpaceMedia_OpenCameraFailedEvent(object sender, EventArgs e)
		{
			_logger.Error($"_eSpaceMedia_OpenCameraFailedEvent Event Raised.");

			SendDeviceStatus((int)DeviceType.Camera, (int)RTDeviceStatus.Offline);
		}

		private void _eSpaceMedia_TerminalLogoutFailEvent(object sender, _DeSpaceMediaEvents_TerminalLogoutFailEventEvent e)
		{
			_logger.Info("eSpace Logout Successfully.");
		}

		#endregion

		#region Public Methods

		public Task<CallResult> CallTeller(CallRequest request) => CallTeller(CancellationToken.None, request);
		public async Task<CallResult> CallTeller(CancellationToken token, CallRequest request)
		{
			if (_callTellerTaskCompletion != null)
			{
				throw new InvalidOperationException($"Already in a Call : {_callTellerTaskCompletion}.");
			}

			try
			{
				_callTellerTaskCompletion = new TaskCompletionSource<CallResult>();
				var res = await Login(token);

				switch (res)
				{
					case LoginResult.IncorrectCredentials:
					case LoginResult.StatusError:
					case LoginResult.NoResponse:
					case LoginResult.MissingConfiguration:
					case LoginResult.LoginFailed:
						{
							_loginTaskCompletion = null;
							throw new CallFailureException($"Exception : {CallResult.LoginFailed}.");
						}
					case LoginResult.AlreadyLoggedIn:
					case LoginResult.Success:
						{
							var result = _eSpaceMedia.mediaTerminalCall(((int)request.CallMode).ToString(), ConfigurationManager.AppSettings["AccessCode"].ToString(), "");
							if (result == 0)
							{
								SetRemoteVideoHandle(request?.RemoteVideoHandle);
								SetLocalVideoHandle(request?.LocalVideoHandle);
							}
							break;
						}
				}

				return await _callTellerTaskCompletion?.Task.OrWhenCancelled(token);
			}
			catch (OperationCanceledException ex)
			{
				_logger.Exception(ex);
				ReleaseCall();
				//Logout();
				//CallEnded?.Invoke(this, null);
				throw;
			}
			catch (CallFailureException ex)
			{
				_logger.Exception(ex);
				throw;
			}
			finally
			{
				_callTellerTaskCompletion = null;
			}
		}

		public short ReleaseCall()
		{
			StopDesktopSharing();
			return _eSpaceMedia.mediaTerminalReleaseCall();
		}

		public bool SendMessage(string message)
		{
			var result = true;

			if (_eSpaceMedia.mediaTerminalSendMsg(message.Trim()) != 0)
			{
				result = false;
			}

			return result;
		}

		public bool SendDeviceStatus(int deviceType, int deviceStatus, object properties = null)
		{
			_logger.Info($"SendDeviceStatus(DeviceType = {deviceType}, DeviceStatus = {deviceStatus}, Properties = {properties})");

			var messageObject = (properties != null) ? JObject.FromObject(properties) : new JObject();

			messageObject.Add(nameof(MessageType), ((int)MessageType.Device).ToString());
			messageObject.Add(nameof(DeviceType), deviceType.ToString());
			messageObject.Add(nameof(RTDeviceStatus), deviceStatus.ToString());

			var message = messageObject.ToString();

			return SendMessage(message);
		}

		public bool SendStatus(StatusEnum statusCode, object properties = null)
		{
			_logger.Info($"SendStatus(statusCode = {statusCode}, Properties = {properties})");

			var messageObject = (properties != null) ? JObject.FromObject(properties) : new JObject();

			messageObject.Add("MessageType", MessageType.Command.ToString());
			messageObject.Add("StatusEnum", statusCode.ToString());

			var message = messageObject.ToString();

			return SendMessage(message);
		}

		public bool IsInCall() => _inCall;

		#endregion

		#region Private Methods

		private async Task<LoginResult> Login(CancellationToken token)
		{
			if (_loginTaskCompletion != null)
			{
				throw new InvalidOperationException($"Already Logged In : {_loginTaskCompletion}.");
			}

			_loginTaskCompletion = new TaskCompletionSource<LoginResult>();

			try
			{
				Login();
				return await _loginTaskCompletion.Task.OrWhenCancelled(token);
			}
			catch (Exception)
			{
				// Task cancelled.
				throw;
			}
			finally
			{
				_loginTaskCompletion = null;
			}
		}

		private LoginResult Login()
		{
			var result = LoginResult.LoginFailed;
			var callerIds = ConfigurationManager.AppSettings["CallerIDs"].ToString();
			var res = -1;

			foreach (var item in callerIds.Split(','))
			{
				res = _eSpaceMedia.mediaTerminalLogin(item, string.Empty);

				_logger.Info($"Login Result : {res} for Id {item}.");

				if (res == 0)
					break;
			}

			if (res == 0)
			{
				result = LoginResult.Success;
			}
			else if (res != 0)
			{
				if (res == 3)
				{
					result = LoginResult.NoResponse;
				}
				else if (res == 7)
				{
					result = LoginResult.IncorrectCredentials;
				}
				else if (res == 8)
				{
					result = LoginResult.AlreadyLoggedIn;
				}
				else if (res == 11)
				{
					result = LoginResult.StatusError;
				}
				else if (res == 1904)
				{
					result = LoginResult.MissingConfiguration;
				}
				else
				{
					result = LoginResult.LoginFailed;
				}

				_loginTaskCompletion.TrySetResult(result);
			}

			return result;
		}

		private short SetRemoteVideoHandle(string remoteHandle) =>
			_eSpaceMedia.mediaTerminalSetRemoteVedioHandle(remoteHandle);

		private short SetLocalVideoHandle(string localHandle) =>
			_eSpaceMedia.mediaTerminalSetLocalVedioHandle(localHandle);

		private short StartDesktopSharing() =>
			_eSpaceMedia.mediaTerminalStartShareDesktop();

		private short StopDesktopSharing() =>
			_eSpaceMedia.mediaTerminalStopShareDesktop();

		public short Logout() =>
			_eSpaceMedia.mediaTerminalLogout();

		#endregion

		public void Dispose()
		{
			_eSpaceMedia.OcxInitSuccEvent -= _eSpaceMedia_OcxInitSuccEvent;
			_eSpaceMedia.TerminalLoginSuccEvent -= _eSpaceMedia_TerminalLoginSuccEvent;
			_eSpaceMedia.TerminalLoginFailEvent -= _eSpaceMedia_TerminalLoginFailEvent;
			_eSpaceMedia.TerminalLogoutFailEvent -= _eSpaceMedia_TerminalLogoutFailEvent;
			_eSpaceMedia.TerminalRingBackEvent -= _eSpaceMedia_TerminalRingBackEvent;
			_eSpaceMedia.TerminalTalkingEvent -= _eSpaceMedia_TerminalTalkingEvent;
			_eSpaceMedia.MsgArrivedEvent -= _eSpaceMedia_MsgArrivedEvent;
			_eSpaceMedia.TerminalWaitingInQueueEvent -= _eSpaceMedia_TerminalWaitingInQueueEvent;
			_eSpaceMedia.TerminalCallingReleaseEvent -= _eSpaceMedia_TerminalCallingReleaseEvent;
			_eSpaceMedia.TerminalLogoutSuccEvent -= _eSpaceMedia_TerminalLogoutSuccEvent;

			_logger?.Info($"{GetType()} => Dispose()");
		}
	}
}