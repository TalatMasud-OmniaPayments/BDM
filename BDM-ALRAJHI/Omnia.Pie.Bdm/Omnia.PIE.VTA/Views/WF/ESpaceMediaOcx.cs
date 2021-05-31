using AxeSpaceMediaLib;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Newtonsoft.Json.Linq;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.PIE.VTA.Common;
using Omnia.PIE.VTA.Core.Model;
using Omnia.PIE.VTA.Views.MsgBoxes;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;

namespace Omnia.PIE.VTA.Views.WF
{
	public partial class eSpaceMediaOcx : Form
	{
		#region "Static Instance"

		private static eSpaceMediaOcx eSpaceMediaOcx1;

		public AnswerIncomingCall AnswerIncomingCall { get; set; }

		/// <summary>
		/// Instances this instance.
		/// </summary>
		/// <returns></returns>
		public static eSpaceMediaOcx Instance()
		{
			if (eSpaceMediaOcx1 == null)
			{
				eSpaceMediaOcx1 = new eSpaceMediaOcx();
			}

			return eSpaceMediaOcx1;
		}

		#endregion

		#region "Events & Delegates"

		public delegate void InitializeSuccess();
		public static event InitializeSuccess InitializeSuccessEvent;

		public delegate void InitializeOcxFailed();
		public static event InitializeOcxFailed InitializeOcxFailedEvent;

		public delegate void LoginSuccess();
		public static event LoginSuccess LoginSuccessEvent;

		public delegate void LogoutSuccess();
		public static event LogoutSuccess LogoutSuccessEvent;

		public delegate void TellerCallRelease();
		public static event TellerCallRelease TellerCallReleaseEvent;

		public delegate void MessageReceived(string msg);
		public static event MessageReceived MessageReceivedEvent;

		public static event EventHandler ChangeInitialPassword;

		#endregion

		#region "eSpaceMediaOcx"

		/// <summary>
		/// Initializes a new instance of the <see cref="eSpaceMediaOcx"/> class.
		/// </summary>
		public eSpaceMediaOcx()
		{
			InitializeComponent();

			#region "Init Events"

			this.axeSpaceMedia.OcxInitSuccEvent += AxeSpaceMedia_OcxInitSuccEvent;
			this.axeSpaceMedia.OcxInitFailedEvent += AxeSpaceMedia_OcxInitFailedEvent;

			#endregion

			#region "Login Events"

			this.axeSpaceMedia.TellerLoginSuccEvent += AxeSpaceMedia_TellerLoginSuccEvent; ;
			this.axeSpaceMedia.TellerLoginFailedEvent += AxeSpaceMedia_TellerLoginFailedEvent;

			#endregion

			#region "Logout Events"

			this.axeSpaceMedia.TellerLogoutSuccEvent += AxeSpaceMedia_TellerLogoutSuccEvent;
			this.axeSpaceMedia.TellerLogoutFailedEvent += AxeSpaceMedia_TellerLogoutFailedEvent;

			#endregion

			#region "Teller State Events"

			this.axeSpaceMedia.TellerIdleEvent += AxeSpaceMedia_TellerIdleEvent;
			this.axeSpaceMedia.TellerWorkEvent += AxeSpaceMedia_TellerWorkEvent;
			this.axeSpaceMedia.TellerRestSuccessEvent += AxeSpaceMedia_TellerRestSuccessEvent;
			this.axeSpaceMedia.TellerSetNotReadyEvent += AxeSpaceMedia_TellerSetNotReadyEvent;

			#endregion

			#region "Call Related Events"

			this.axeSpaceMedia.TellerAlertingEvent += AxeSpaceMedia_TellerAlertingEvent;

			this.axeSpaceMedia.TellerTalkingEvent += AxeSpaceMedia_TellerTalkingEvent;
			this.axeSpaceMedia.TellerNoAnswerEvent += AxeSpaceMedia_TellerNoAnswerEvent;
			this.axeSpaceMedia.OpenCameraFailedEvent += AxeSpaceMedia_OpenCameraFailedEvent;

			this.axeSpaceMedia.TellerHoldEvent += AxeSpaceMedia_TellerHoldEvent; ;

			#endregion

			this.axeSpaceMedia.TellerGetTerminalVideoParamResultEvent += AxeSpaceMedia_TellerGetTerminalVideoParamResultEvent;
			this.axeSpaceMedia.MsgArrivedEvent += AxeSpaceMedia_MsgArrivedEvent;

			this.axeSpaceMedia.ShareDesktopWndSizeEvent += AxeSpaceMedia_ShareDesktopWndSizeEvent;

			this.axeSpaceMedia.TellerCallingReleaseEvent += AxeSpaceMedia_TellerCallingReleaseEvent;

		}

		/// <summary>
		/// Disposes the object.
		/// </summary>
		public void DisposeObject()
		{
			eSpaceMediaOcx1.Dispose(true);
			GC.SuppressFinalize(eSpaceMediaOcx1);
		}

		#endregion

		#region "eSpace Media Events"

		/// <summary>
		/// Handles the OcxInitSuccEvent event of the AxeSpaceMedia control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void AxeSpaceMedia_OcxInitSuccEvent(object sender, EventArgs e)
		{
			try
			{
				InitializeSuccessEvent?.Invoke();
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		/// <summary>
		/// Handles the OcxInitFailedEvent event of the AxeSpaceMedia control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void AxeSpaceMedia_OcxInitFailedEvent(object sender, EventArgs e)
		{
			InitializeOcxFailedEvent?.Invoke();
		}

		/// <summary>
		/// Axes the space media teller login succ event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void AxeSpaceMedia_TellerLoginSuccEvent(object sender, _DeSpaceMediaEvents_TellerLoginSuccEventEvent e)
		{
			try
			{
				var initialPassword = FindValueFromJson(e.sEventInfo, "initPasswd");
				bool.TryParse(initialPassword, out var change);
				if (change)
				{
					ChangeInitialPassword?.Invoke(this, EventArgs.Empty);
				}
				else
				{
					LoginSuccessEvent?.Invoke();
				}

				SetLoginEnterMode(); // Set Teller to be Idle after login.
				var localPort = 6600;

				var entryVRP = eSpaceMediaOcx1.axeSpaceMedia.mediaGetConfig("EntryVrp");
				var localIP = eSpaceMediaOcx1.axeSpaceMedia.mediaGetConfig("RecordScreenLocalIP");
				int.TryParse(eSpaceMediaOcx1.axeSpaceMedia.mediaGetConfig("RecordScreenLocalPort"), out localPort);
				var pszToken = axeSpaceMedia.mediaTellerQueryVrcToken();

				var jObj = JObject.Parse(pszToken);
				var token = (string)jObj.SelectToken("token");
                var result = axVRCControl1.InitializeAgentEx(entryVRP, 2, GlobalInfo.TellerId, localIP, localPort, "", token);
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		/// <summary>
		/// Axes the space media teller login failed event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void AxeSpaceMedia_TellerLoginFailedEvent(object sender, _DeSpaceMediaEvents_TellerLoginFailedEventEvent e)
		{
			WpfMessageBox.Show("Login failed " + e.sEventInfo);
		}

		/// <summary>
		/// Axes the space media teller logout failed event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void AxeSpaceMedia_TellerLogoutFailedEvent(object sender, _DeSpaceMediaEvents_TellerLogoutFailedEventEvent e)
		{
			WpfMessageBox.Show("Logout failed " + e.sEventInfo);
		}

		/// <summary>
		/// Handles the TellerLogoutSuccEvent event of the AxeSpaceMedia control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void AxeSpaceMedia_TellerLogoutSuccEvent(object sender, EventArgs e)
		{
			axVRCControl1.ExitAgent();
			LogoutSuccessEvent?.Invoke();
		}

		/// <summary>
		/// Handles the TellerWorkEvent event of the AxeSpaceMedia control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void AxeSpaceMedia_TellerWorkEvent(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// Handles the TellerIdleEvent event of the AxeSpaceMedia control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void AxeSpaceMedia_TellerIdleEvent(object sender, EventArgs e)
		{
			MainWindow.Instance.TellerStatus.BackGround = Brushes.GreenYellow;
			MainWindow.Instance.DisableUI();
		}

		/// <summary>
		/// Handles the TellerRestSuccessEvent event of the AxeSpaceMedia control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void AxeSpaceMedia_TellerRestSuccessEvent(object sender, EventArgs e)
		{
			MainWindow.Instance.TellerStatus.BackGround = Brushes.Orange;
		}

		/// <summary>
		/// Handles the TellerSetNotReadyEvent event of the AxeSpaceMedia control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void AxeSpaceMedia_TellerSetNotReadyEvent(object sender, EventArgs e)
		{
			MainWindow.Instance.TellerStatus.BackGround = Brushes.Red;
			GlobalInfo.TellerState = TellerState.Busy;
		}

		/// <summary>
		/// Axes the space media share desktop WND size event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void AxeSpaceMedia_ShareDesktopWndSizeEvent(object sender, _DeSpaceMediaEvents_ShareDesktopWndSizeEventEvent e)
		{
			SetDesktopSharingDisplaySize(MainWindow.Instance.screenShare.Width.ToString(), MainWindow.Instance.screenShare.Height.ToString());
		}

		/// <summary>
		/// Axes the space media MSG arrived event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void AxeSpaceMedia_MsgArrivedEvent(object sender, _DeSpaceMediaEvents_MsgArrivedEventEvent e)
		{
			if (!string.IsNullOrEmpty(e.sEventInfo))
			{
				MessageReceivedEvent?.Invoke(e.sEventInfo);
			}
		}

		/// <summary>
		/// Axes the space media teller talking event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void AxeSpaceMedia_TellerTalkingEvent(object sender, _DeSpaceMediaEvents_TellerTalkingEventEvent e)
		{
			MainWindow.Instance.TellerStatus.BackGround = Brushes.Red;
			GlobalInfo.TellerState = TellerState.Busy;

			//VTM-3891 [3x] Common - sometimes screen to enter PIN is not shown up after RT initialized authentication by card
			//MainWindow.Instance.EnableUI();
		}

		/// <summary>
		/// Axes the space media teller no answer event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void AxeSpaceMedia_TellerNoAnswerEvent(object sender, _DeSpaceMediaEvents_TellerNoAnswerEventEvent e)
		{
			Logger.Writer.Info($"{nameof(eSpaceMediaOcx)} TellerNoAnswerEvent (teller: {AnswerIncomingCall?.Terminal})");

			MainWindow.Instance.ReleaseCall();

			if (AnswerIncomingCall == null)
				return;

			AnswerIncomingCall?.Close();
			SetTellerIdle();

			if (!MainWindow.Instance.ShowMissedCallAlert)
				return;
			SetTellerBusy();
			WpfMessageBox.Show($"You missed a call from the Customer\nand your status is changed to busy.",
				"Information");
		}

		/// <summary>
		/// Handles the OpenCameraFailedEvent event of the AxeSpaceMedia control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void AxeSpaceMedia_OpenCameraFailedEvent(object sender, EventArgs e)
		{
			WpfMessageBox.Show("Open Camera Failed Event");
		}

		/// <summary>
		/// Axes the space media teller get terminal video parameter result event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void AxeSpaceMedia_TellerGetTerminalVideoParamResultEvent(object sender, _DeSpaceMediaEvents_TellerGetTerminalVideoParamResultEventEvent e)
		{

		}

		/// <summary>
		/// Axes the space media teller alerting event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void AxeSpaceMedia_TellerAlertingEvent(object sender, _DeSpaceMediaEvents_TellerAlertingEventEvent e)
		{
			JObject jObj = JObject.Parse(e.sEventInfo);
			string caller = "";

			if (jObj != null)
			{
				if (!string.IsNullOrEmpty((string)jObj.SelectToken("accessType")) && "anonymous".Equals((string)jObj.SelectToken("accessType")))
				{
					caller = "Anonymous User";
				}
				else
				{
					caller = (string)jObj.SelectToken("remote_number");
				}
			}


			AnswerIncomingCall = new AnswerIncomingCall();

			AnswerIncomingCall.Terminal = caller;
			AnswerIncomingCall.ShowDialog();
		}

		/// <summary>
		/// Axes the space media teller calling release event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void AxeSpaceMedia_TellerCallingReleaseEvent(object sender, _DeSpaceMediaEvents_TellerCallingReleaseEventEvent e)
		{
			TellerCallReleaseEvent?.Invoke();
		}

		/// <summary>
		/// Handles the TellerHoldEvent event of the AxeSpaceMedia control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void AxeSpaceMedia_TellerHoldEvent(object sender, EventArgs e)
		{

		}

		#endregion

		#region "Public Methods"

		/// <summary>
		/// Change Password for Agent
		/// </summary>
		/// <param name="oldPassword"></param>
		/// <param name="newPassword"></param>
		/// <returns></returns>
		public short ChangePassword(string oldPassword, string newPassword)
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerChangeAgentPasswd start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerChangeAgentPasswd(oldPassword, newPassword);
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerChangeAgentPasswd Result " + result);

			return result;
		}

		/// <summary>
		/// Gets the teller status.
		/// </summary>
		/// <param name="tellerId">The teller identifier.</param>
		/// <returns></returns>
		public TellerState GetTellerStatus(string tellerId)
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerGetDetailInfo start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerGetDetailInfo(tellerId);
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerGetDetailInfo Result " + result);

			return GetTellerState(result);
		}

		/// <summary>
		/// Sets the teller call release enter mode.
		/// </summary>
		/// <returns></returns>
		public short SetTellerCallReleaseEnterMode()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSetReleaseEnterMode start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerSetReleaseEnterMode("4"); // 4: idle 5: working
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSetReleaseEnterMode Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Sets the teller login enter mode.
		/// </summary>
		/// <returns></returns>
		public short SetLoginEnterMode()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSetLoginEnterMode start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerSetLoginEnterMode("4"); // 4: idle 5: working
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSetLoginEnterMode Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Logouts this instance.
		/// </summary>
		/// <returns></returns>
		public short Logout()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerLogout start.");
			short result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerLogout(); // This interface is invoked in asynchronous mode.
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerLogout Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Logouts this instance.
		/// </summary>
		/// <returns></returns>
		public short LogoutForce()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerForceLogout start.");
			short result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerForceLogout(); // This interface is invoked in asynchronous mode.
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerForceLogout Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Releases the call.
		/// </summary>
		/// <returns></returns>
		public short ReleaseCall()
		{
			short result = -1;

			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerRelease start.");
			result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerRelease(); // This interface is invoked in asynchronous mode.
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerRelease Result " + result.ToString());

			if (result != 0) //11 Incorrect status. OR 18 No call is available for releasing.
			{
				Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerForceRelease start.");
				result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerForceRelease(); // This interface is invoked in asynchronous mode.
				Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerForceRelease Result " + result.ToString());

				TellerCallReleaseEvent?.Invoke();
			}

			return result;
		}

		/// <summary>
		/// Answers the call.
		/// </summary>
		/// <returns></returns>
		public short AnswerCall()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerAnswer start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerAnswer();
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerAnswer Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Sets the handles.
		/// </summary>
		/// <param name="localVid">The local vid.</param>
		/// <param name="remoteVid">The remote vid.</param>
		/// <param name="screenShare">The screen share.</param>
		public void SetHandles(IntPtr localVid, IntPtr remoteVid, IntPtr screenShare)
		{
			var result1 = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerSetLocalVedioHandle(localVid.ToString());
			var result2 = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerSetRemoteVedioHandle(remoteVid.ToString());
			var result3 = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerSetDesktopWnd(screenShare.ToString());
		}

		/// <summary>
		/// Sets the display size of the desktop sharing.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns></returns>
		public short SetDesktopSharingDisplaySize(string width, string height)
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSetShareDesktopDisplaySize start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerSetShareDesktopDisplaySize(width, height);
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSetShareDesktopDisplaySize Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Gets the application list.
		/// </summary>
		/// <returns></returns>
		public string GetApplicationList()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerGetApplicationList start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerGetApplicationList();
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerGetApplicationList Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Starts the share application.
		/// </summary>
		/// <param name="appHandle">The application handle.</param>
		/// <returns></returns>
		public short StartShareApplication(string appHandle)
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerStartShareApplication start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerStartShareApplication(appHandle);
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerStartShareApplication Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Stops the share application.
		/// </summary>
		/// <param name="appHandle">The application handle.</param>
		/// <returns></returns>
		public short StopShareApplication(string appHandle)
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerStopShareApplication start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerStopShareApplication(appHandle);
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerStopShareApplication Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Sends the message.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		/// <returns></returns>
		public short SendMessage(string statusCode)
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSendMsgToTerminal start." + statusCode);

			//var baseMessage = MessageProcessor.CreateMessage(statusCode);
			//baseMessage.Flag = MessageFlag.RT;
			//var encMessage = JsonConvert.SerializeObject(baseMessage);

			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerSendMsgToTerminal(statusCode);

			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSendMsgToTerminal Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Sets the mute.
		/// </summary>
		/// <returns></returns>
		public short SetMute()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerMute start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerMute();
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerMute Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Sets the un mute.
		/// </summary>
		/// <returns></returns>
		public short SetUnMute()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerUnMute start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerUnMute();
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerUnMute Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Logins the specified teller identifier.
		/// </summary>
		/// <param name="tellerId">The teller identifier.</param>
		/// <param name="password">The password.</param>
		/// <param name="phoneNum">The phone number.</param>
		/// <param name="phonePass">The phone pass.</param>
		/// <returns></returns>
		public short Login(string tellerId, string password, string phoneNum, string phonePass)
		{
			short result = -1;

			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerLogin start.");

			result = Dispatcher.CurrentDispatcher.Invoke(new Func<short>(() =>
						eSpaceMediaOcx1.axeSpaceMedia.mediaTellerLogin(tellerId, password, phoneNum, phonePass)
					));

			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerLogin Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// UnHolds the call.
		/// </summary>
		/// <returns></returns>
		public short UnHoldCall()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerUnhold start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerUnhold();
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerUnhold Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Holds the call.
		/// </summary>
		/// <returns></returns>
		public short HoldCall()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerHold start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerHold();
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerHold Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Sets the teller busy.
		/// </summary>
		/// <returns></returns>
		public short SetTellerBusy()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSayBusy start.");
			short result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerSayBusy();
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSayBusy Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Sets the teller idle.
		/// </summary>
		/// <returns></returns>
		public short SetTellerIdle()
		{
			short result = -1;

			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSayIdle start.");
			result = Dispatcher.CurrentDispatcher.Invoke(new Func<short>(() =>
						eSpaceMediaOcx1.axeSpaceMedia.mediaTellerSayIdle()
					));
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSayIdle Result " + result.ToString());

			if (result == 0)
			{
				GlobalInfo.TellerState = TellerState.Idle;
			}

			return result;
		}

		/// <summary>
		/// Sets the teller rest.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <param name="reason">The reason.</param>
		/// <returns></returns>
		public short SetTellerRest(string reason = "0")
		{
			short result = -1;

			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSayRest start.");
			result = Dispatcher.CurrentDispatcher.Invoke(new Func<short>(() =>
					   eSpaceMediaOcx1.axeSpaceMedia.mediaTellerSayRest("3500", "0")
					));
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSayRest Result " + result.ToString());

			if (result == 0)
			{
				GlobalInfo.TellerState = TellerState.Rest;
			}

			return result;
		}

		/// <summary>
		/// Sets the teller cancel rest.
		/// </summary>
		/// <returns></returns>
		public short SetTellerCancelRest()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerCancelRest start.");
			short result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerCancelRest();
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerCancelRest Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Sets the teller work.
		/// </summary>
		/// <returns></returns>
		public short SetTellerWork()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSayWork start.");
			short result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerSayWork();
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSayWork Result " + result.ToString());

			if (result == 0)
			{
				GlobalInfo.TellerState = TellerState.Working;
			}

			return result;
		}

		/// <summary>
		/// Sets the teller cancel work.
		/// </summary>
		/// <returns></returns>
		public short SetTellerCancelWork()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerCancelWork start.");
			short result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerCancelWork();
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerCancelWork Result " + result.ToString());

			return result;
		}

		/// <summary>
		/// Gets the support vedio parameter.
		/// </summary>
		/// <returns></returns>
		public SupportVideoParam GetSupportVedioParam()
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerGetSupportVedioParam start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerGetSupportVedioParam();
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerGetSupportVedioParam Result " + result.ToString());

			return GetSupportVideoParam(result);
		}

		/// <summary>
		/// Sets the support vedio parameter.
		/// </summary>
		/// <param name="parm">The parm.</param>
		/// <returns></returns>
		public short SetSupportVedioParam(SupportVideoParam parm)
		{
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSetTerminalVedioParam start.");
			var result = eSpaceMediaOcx1.axeSpaceMedia.mediaTellerSetTerminalVedioParam(parm.ToString());
			Logger.Writer.Info(nameof(eSpaceMediaOcx) + " mediaTellerSetTerminalVedioParam Result " + result.ToString());

			return result;
		}

		#endregion

		#region "Utility Methods"

		[DllImport("wininet.dll")]
		private extern static bool InternetGetConnectedState(out int description, int reservedValue);

		/// <summary>
		/// Determines whether [is internet available].
		/// </summary>
		/// <returns>
		///   <c>true</c> if [is internet available]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsInternetAvailable()
		{
			int description;
			var result = InternetGetConnectedState(out description, 0);
			return result;
		}

		/// <summary>
		/// Finds the retcode from json.
		/// </summary>
		/// <param name="sMsg">The s MSG.</param>
		/// <returns></returns>
		public static string FindRetcodeFromJson(string sMsg)
		{
			string returnValue = "";

			if (!string.IsNullOrEmpty(sMsg))
			{
				JObject job = JObject.Parse(sMsg);

				if (null != job && null != job["retcode"])
				{
					returnValue = job["retcode"].ToString();
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Finds the value from json.
		/// </summary>
		/// <param name="sMsg">The s MSG.</param>
		/// <param name="retValue">The ret value.</param>
		/// <returns></returns>
		public static string FindValueFromJson(string sMsg, string retValue)
		{
			string returnValue = "";

			if (!string.IsNullOrEmpty(sMsg) && !string.IsNullOrEmpty(retValue))
			{
				JObject job = JObject.Parse(sMsg);

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

		public static int FindIntValueFromJson(string sMsg, string retValue)
		{
			var str = FindValueFromJson(sMsg, retValue);
			int res;
			return int.TryParse(str, out res) ? res : 0;
		}

		/// <summary>
		/// Gets the state of the teller.
		/// </summary>
		/// <param name="json">The json.</param>
		/// <returns></returns>
		public static TellerState GetTellerState(string json)
		{
			string state = FindValueFromJson(json, "state_info");

			var enumType = TellerState.Idle;
			TellerState.TryParse(state, out enumType);

			return enumType;
		}

		/// <summary>
		/// Gets the type of the device.
		/// </summary>
		/// <param name="json">The json.</param>
		/// <returns></returns>
		public static DeviceType GetDeviceType(string json)
		{
			string state = FindValueFromJson(json, "DeviceType");

			var enumType = DeviceType.Auxiliaries;
			DeviceType.TryParse(state, out enumType);

			return enumType;
		}

		/// <summary>
		/// Gets the device status.
		/// </summary>
		/// <param name="json">The json.</param>
		/// <returns></returns>
		public static RTDeviceStatus GetDeviceStatus(string json)
		{
			string state = FindValueFromJson(json, "RTDeviceStatus");

			var enumType = RTDeviceStatus.Offline;
			RTDeviceStatus.TryParse(state, out enumType);

			return enumType;
		}

		/// <summary>
		/// Gets the customer selected language.
		/// </summary>
		/// <param name="json">The json.</param>
		/// <returns></returns>
		public static string GetSelectedLanguage(string json)
		{
			string selectedLanguage = FindValueFromJson(json, "SelectedLanguage");

			var enumType = Language.English;
			Language.TryParse(selectedLanguage, out enumType);

			return enumType.ToString();
		}

		/// <summary>
		/// Gets the support video parameter.
		/// </summary>
		/// <param name="json">The json.</param>
		/// <returns></returns>
		public static SupportVideoParam GetSupportVideoParam(string json)
		{
			string state = FindValueFromJson(json, "SupportVideoParam");

			var enumType = SupportVideoParam.FrameRate;
			SupportVideoParam.TryParse(state, out enumType);

			return enumType;
		}

		/// <summary>
		/// Parses the espace message.
		/// </summary>
		/// <param name="msg">The MSG.</param>
		/// <returns></returns>
		public JSONData ParseEspaceMessage(string msg)
		{
			if (!string.IsNullOrEmpty(msg))
			{
				JSONData objData = new JSONData();

				JObject parsedData = JObject.Parse(msg);

				if (!string.IsNullOrEmpty(parsedData["Data"].ToString()))
					objData.Data = parsedData["Data"].ToString();

				if (!string.IsNullOrEmpty(parsedData["DataLen"].ToString()))
					objData.DataLength = parsedData["DataLen"].ToString();

				if (!string.IsNullOrEmpty(parsedData["SenderID"].ToString()))
					objData.SenderID = parsedData["SenderID"].ToString();

				if (!string.IsNullOrEmpty(parsedData["retcode"].ToString()))
					objData.StatusCode = parsedData["retcode"].ToString();

				return objData;
			}
			else
				return new JSONData();
		}

		/// <summary>
		/// Determines whether the specified input is alphabetic.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns>
		///   <c>true</c> if the specified input is alphabetic; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsAlphabetic(string input)
		{
			return Regex.IsMatch(input, "^[a-zA-Z]+$");
		}

		/// <summary>
		/// Determines whether [is alpha numeric] [the specified input].
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns>
		///   <c>true</c> if [is alpha numeric] [the specified input]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsAlphaNumeric(string input)
		{
			return Regex.IsMatch(input, "^[a-zA-Z0-9]+$");
		}

		/// <summary>
		/// Determines whether the specified input is numeric.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns>
		///   <c>true</c> if the specified input is numeric; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNumeric(string input)
		{
			return Regex.IsMatch(input, "^[0-9]+$");
		}

		#endregion
	}
}
