using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using Omnia.PIE.VTA.Views;
using Omnia.PIE.VTA.Common;
using Omnia.PIE.VTA.Views.WF;
using Omnia.PIE.VTA.Core.Model;
using Omnia.PIE.VTA.Views.Workflow;
using Omnia.PIE.VTA.ViewModels;
using Newtonsoft.Json;
using Omnia.PIE.VTA.Views.MsgBoxes;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using AccountsAndDeposits = Omnia.PIE.VTA.Core.Model.AccountsAndDeposits;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Windows.Media;
using Microsoft.Win32;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Services.Interface;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System.Configuration;

namespace Omnia.PIE.VTA
{
	public partial class MainWindow : Window
	{
		#region "Properties"

		public static MainWindow Instance => ((App)Application.Current).Shell;

		[Dependency]
		public IAuthenticationService AuthenticationService { get; set; }

		[Dependency]
		public ICustomerService CustomerService { get; set; }

		[Dependency]
		public IServiceManager ServiceManager { get; set; }

		[Dependency]
		public ITransactionService TransactionService { get; set; }
		//[Dependency]
		//public IUserActivityService UserActivityService { get; set; }

		private CallTimeObserver _CallTimeObserver;
		public CallTimeObserver CallTimeObserver
		{
			get
			{
				if (_CallTimeObserver == null)
					_CallTimeObserver = new CallTimeObserver();

				return _CallTimeObserver;
			}
			set
			{
				_CallTimeObserver = value;
			}
		}

		private TellerStatus _TellerStatus;
		public TellerStatus TellerStatus
		{
			get
			{
				if (_TellerStatus == null)
					_TellerStatus = new TellerStatus();
				return _TellerStatus;
			}
			set
			{
				_TellerStatus = value;
			}
		}

		private static WorkFlowViewModel _WorkFlowViewModel;
		public static WorkFlowViewModel WorkFlowViewModel
		{
			get
			{
				if (_WorkFlowViewModel == null)
					_WorkFlowViewModel = new WorkFlowViewModel();
				return _WorkFlowViewModel;
			}
			set
			{
				_WorkFlowViewModel = value;
			}
		}

		private Customer _CustomerViewModel;
		public Customer CustomerViewModel
		{
			get
			{
				if (_CustomerViewModel == null)
					_CustomerViewModel = new Customer();
				return _CustomerViewModel;
			}
			set
			{
				_CustomerViewModel = value;
			}
		}

		public List<AccountsAndDeposits> AccountsAndDepositsViewModel { get; set; }

		public List<Card> CardViewModel { get; set; }

		public List<Loan> LoansViewModel { get; set; }

		public eSpaceMediaOcx eSpaceMediaOcxApi
		{
			get
			{
				return eSpaceMediaOcx.Instance();
			}
		}

		public bool ShowMissedCallAlert { get; set; }

		private DispatcherTimer Timer { get; set; }

		private int count = 0;

		//RemoteWindow remoteVid = new RemoteWindow();

		private bool _shutDownOnLogOut;

		#endregion

		#region "Main Window Events"

		public MainWindow()
		{
			InitializeComponent();
			CreateCassettesGrid(DevicesControl.ViewModel.CashCassettes);
			StartAuthenticationFlowWorkflow();
			//remoteVid.txtTitle.Text = "Customer Video";

			this.DataContext = WorkFlowViewModel;
			btnDashboard.IsChecked = true;

			Top = 0;
			Left = 0;
			Height = SystemParameters.WorkArea.Height;
			Width = SystemParameters.WorkArea.Width;

			eSpaceMediaOcx.MessageReceivedEvent += eSpaceMediaOcx_MessageReceivedEvent;
			eSpaceMediaOcx.TellerCallReleaseEvent += ESpaceMediaOcx_TellerCallReleaseEvent;

			_shutDownOnLogOut = false;
			eSpaceMediaOcx.LogoutSuccessEvent += ESpaceMediaOcx_LogoutSuccessEvent;
			SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;

			Timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = new TimeSpan(0, 0, 0, 2) };
			Timer.Tick += Timer_Tick;
		}

		private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
		{
			if (e.Reason == SessionSwitchReason.SessionLock)
			{
				//var result = eSpaceMediaOcxApi.SetTellerBusy();

				ReleaseCall();
				DevicesControl.StopNetworObserver();

				Dispatcher.Invoke(DispatcherPriority.Send, (Action)(() =>
				{
					_shutDownOnLogOut = true;
					eSpaceMediaOcxApi.Logout();
				}));

				Logger.Writer.Info(GetType() + " Logout() - SystemEvents_SessionSwitch.");
			}
			//else if (e.Reason == SessionSwitchReason.SessionUnlock)
			//{
			//	var result = eSpaceMediaOcxApi.SetTellerIdle();
			//}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			eSpaceMediaOcxApi.SetHandles(localVideo.Handle, customerVideo.Handle, screenShare.Handle);

			Binding binding = new Binding("BackGround");
			binding.Source = TellerStatus;
			btnTellerState.SetBinding(Button.BackgroundProperty, binding);
			txtAgentID.Text = GlobalInfo.TellerId;

			//if (GlobalInfo.AccessToken != null)
			//{
			//	ServiceManager.SecurityContext = new SecurityContext(GlobalInfo.AccessToken);
			//}
			//if (bool.Parse(ConfigurationManager.AppSettings["UseOpenId"].ToString()))
			//{
			//	StartUserActivityTimer();
			//}
		}

		//private async void StartUserActivityTimer()
		//{
		//	try
		//	{
		//		var trackingStartTime = DateTime.Now;
		//		bool timeLimitExceeded;
		//		do
		//		{
		//			await Task.Delay(1);
		//			var lastActivityTime = UserActivityService.LastActivityTime;
		//			if (lastActivityTime < trackingStartTime)
		//			{
		//				lastActivityTime = trackingStartTime;
		//			}
		//			var idlePeriod = DateTime.Now - lastActivityTime;
		//			timeLimitExceeded = idlePeriod.Minutes > int.Parse(ConfigurationManager.AppSettings["UseOpenId"].ToString());
		//		} while (!timeLimitExceeded);
		//		TimedOut();
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Writer.Exception(ex);
		//	}
		//}
		//private void TimedOut()
		//{
		//	GlobalInfo.AccessToken = null;
		//	ServiceManager.SecurityContext = null;
		//}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Logout();
			eSpaceMediaOcxApi.DisposeObject();
		}

		#endregion

		#region "ESpace Events"

		private void ESpaceMediaOcx_LogoutSuccessEvent()
		{
			if (_shutDownOnLogOut)
			{
				Application.Current.Shutdown();
				return;
			}

			Logger.Writer.Info(GetType() + " An unexpected logout.");

			WpfMessageBox.Show("An unexpected logout occurred. Application will be shutdown.", "Error",
				WpfMessageBoxButton.OK, WpfMessageBoxImage.Error);

			Application.Current.Shutdown();
		}

		private void ESpaceMediaOcx_TellerCallReleaseEvent()
		{
			CallTimeObserver.ObserverTimer.Stop();
			eSpaceMediaOcxApi.SetTellerIdle();
			localVideo.Refresh();
			customerVideo.Refresh();
			screenShare.Refresh();
			//remoteVid.Hide();
			btnTellerState.IsEnabled = true;
			btnCustomerInfo.IsEnabled = false;
			GlobalInfo.IsAuthenticated = false;
			btnDashboard_Click(null, null);

			InitiateWorkflow();
			StartAuthenticationFlowWorkflow();
			DevicesControl.SetDevicesInitializeState();
			CreateCassettesGrid(DevicesControl.ViewModel.CashCassettes);
			ReInitializeWorkflow();

			btnRelease.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF90A1BD"));
			BtnCancelTransaction.IsEnabled = true;
			DisableUI();
		}

		private void eSpaceMediaOcx_MessageReceivedEvent(string msg)
		{
			try
			{
				if (!string.IsNullOrEmpty(msg))
				{
					var parsedMessage = eSpaceMediaOcxApi.ParseEspaceMessage(msg);
					var messageType = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(MessageType));
					var enumType = MessageType.CardAccounts;

					Enum.TryParse(messageType, out enumType);

					switch (enumType)
					{
						case MessageType.Command:
							{
								var statusEnum = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(StatusEnum));
								var commandType = StatusEnum.BackButtonCode;
								Enum.TryParse(statusEnum, out commandType);

								WorkflowTracking.Instance().ManageWorkflowStepTracking(commandType);

								switch (commandType)
								{
									#region "Authentication Flow"

									case StatusEnum.AuthenticateWaitingForPin:
										{
											WorkFlowViewModel.Message = "Waiting for PIN...";

											break;
										}
									case StatusEnum.AuthenticateInvalidPin:
										{
											WorkFlowViewModel.Message = "Invalid PIN...";

											break;
										}
									case StatusEnum.AuthenticateWaitingForAuthentication:
										{
											WorkFlowViewModel.Message = "Waiting for authentication...";

											break;
										}
									case StatusEnum.AuthenticateCustomerValidated:
										{
											WorkFlowViewModel.Message = "Customer Validated.";
											EnableFlowUI();

											break;
										}
									case StatusEnum.AuthenticateOTPActivated:
										{
											WorkFlowViewModel.Message = "OTP Activated.";

											EnableFlowUI();
											break;
										}
									case StatusEnum.AuthenticateEmiratesIdAuthenticated:
										{
											WorkFlowViewModel.Message = "Emirates Id Authenticated";
											GlobalInfo.IsAuthenticated = true;

											WorkFlowViewModel.CustomerId = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "CustomerId");
											LoadCustomerDashboard();

											FlowType.Text = "None";
											break;
										}
									case StatusEnum.AuthenticateCardAuthenticated:
										{
											WorkFlowViewModel.Message = "Card and PIN Authenticated";
											GlobalInfo.IsAuthenticated = true;

											WorkFlowViewModel.CustomerId = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "CustomerId");
											LoadCustomerDashboard();

											FlowType.Text = "None";
											break;
										}
									case StatusEnum.AuthenticateIlliterateCustomer:
										{
											GlobalInfo.IsAuthenticated = false;
											btnCustomerInfo.IsEnabled = false;
											StartAuthenticationFlowWorkflow();
											var reason = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Reason");
											WpfMessageBox.Show(reason, "Error", WpfMessageBoxButton.OK);

											break;
										}
									case StatusEnum.AuthenticateOffUsCard:
										{
											WpfMessageBox.Show("Offus Card, Customer can try only self service.", "Error", WpfMessageBoxButton.OK);
											break;
										}
									case StatusEnum.AuthenticateEIDDetails:
										{
											var EIDDetails = "Name : ";
											EIDDetails += eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "EIDName");
											EIDDetails += Environment.NewLine;

											EIDDetails += "Id Number : ";
											EIDDetails += eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "EIDNumber");
											EIDDetails += Environment.NewLine;

											EIDDetails += "Expiry : ";
											EIDDetails += eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "EIDExpiry");

											WorkFlowViewModel.EIDDetail = EIDDetails;

											var exDate = DateTime.Now;
											DateTime.TryParse(eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "EIDExpiry"), out exDate);

											if (exDate.Date < DateTime.Now.Date)
											{
												WpfMessageBox.Show(EIDDetails, "Info", WpfMessageBoxButton.OK);
											}

											break;
										}
									case StatusEnum.SelfInstructionMode:
										{
											WpfMessageBox.Show("Vtm user needs your help to perform flows in self service mode.", "Info", WpfMessageBoxButton.OK);
											DisableFlowUI();
											BtnCancelTransaction.IsEnabled = false;
											break;
										}
									case StatusEnum.ChequeDepositMode:
										{
											WpfMessageBox.Show("Vtm user needs your help to perform Cheque Deposit.", "Info", WpfMessageBoxButton.OK);
											break;
										}
									case StatusEnum.ChequePrintingMode:
										{
											WpfMessageBox.Show("Vtm user needs your help to perform Cheque Printing.", "Info", WpfMessageBoxButton.OK);
											break;
										}
									case StatusEnum.AuthenticateTerminalId:
										{
											if (ServiceManager != null && ServiceManager.Terminal != null)
												ServiceManager.Terminal.Id = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "TerminalId");
											break;
										}
									case StatusEnum.AuthenticateBranchId:
										{
											if (ServiceManager != null && ServiceManager.Terminal != null)
												ServiceManager.Terminal.BranchId = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "BranchId");
											break;
										}

									#endregion

									#region "Leads"

									case StatusEnum.AccountLeadStarted:
										{
											WorkFlowViewModel.Message = "Customer filling chosen form...";

											if (newLeadCustomerForm == null)
												newLeadCustomerForm = new LeadCustomerForm();

											workflowFrame.Navigate(newLeadCustomerForm);

											break;
										}
									case StatusEnum.AccountLeadCompleted:
										{
											EnableFlowUI();

											break;
										}
									case StatusEnum.LoanLeadStarted:
										{
											WorkFlowViewModel.Message = "Customer filling chosen form...";

											if (newLeadCustomerForm == null)
												newLeadCustomerForm = new LeadCustomerForm();

											workflowFrame.Navigate(newLeadCustomerForm);

											break;
										}
									case StatusEnum.LoanLeadCompleted:
										{
											EnableFlowUI();

											break;
										}
									case StatusEnum.CreditCardLeadStarted:
										{
											WorkFlowViewModel.Message = "Customer filling chosen form...";

											if (newLeadCustomerForm == null)
												newLeadCustomerForm = new LeadCustomerForm();

											workflowFrame.Navigate(newLeadCustomerForm);

											break;
										}
									case StatusEnum.CreditCardLeadCompleted:
										{
											EnableFlowUI();

											break;
										}
									case StatusEnum.WealthLeadStarted:
										{
											WorkFlowViewModel.Message = "Customer filling chosen form...";

											if (newLeadCustomerForm == null)
												newLeadCustomerForm = new LeadCustomerForm();

											workflowFrame.Navigate(newLeadCustomerForm);

											break;
										}
									case StatusEnum.WealthLeadCompleted:
										{
											EnableFlowUI();

											break;
										}

									#endregion

									#region "CashWithdrawal"

									#region "CashWithdrawal Debit Card"

									case StatusEnum.CashWithdrawalWaitingForPin:
										{
											WorkFlowViewModel.Message = "Waiting for PIN...";

											break;
										}
									case StatusEnum.CashWithdrawalWaitingForAuthentication:
										{
											WorkFlowViewModel.Message = "Waiting for authentication...";

											break;
										}
									case StatusEnum.CashWithdrawalCardAuthenticated:
										{
											WorkFlowViewModel.Message = "Card and PIN Authenticated";

											break;
										}
									case StatusEnum.CashWithdrawalAccountVerbalAccount:
										{
											EnableFlowUI();
											if (cashWithdrawalSendAccountInfo == null)
												cashWithdrawalSendAccountInfo = new CashWithdrawalSendAccountInformation();

											workflowFrame.Navigate(cashWithdrawalSendAccountInfo);

											break;
										}
									case StatusEnum.CashWithdrawalManuallyEnteredAccount:
										{
											EnableFlowUI();

											WorkFlowViewModel.Title = "Account Selected";
											WorkFlowViewModel.SelectedAccountNumber = GetSelectedAccount(parsedMessage.Data);

											//if (cashWithdrawalSendAccountInfo == null)
											//    cashWithdrawalSendAccountInfo = new CashWithdrawalSendAccountInformation();

											//workflowFrame.Navigate(cashWithdrawalSendAccountInfo);

											break;
										}
									case StatusEnum.CashWithdrawalCardAccounts:
										{
											WorkFlowViewModel.Message = "Card and PIN Authenticated";
											WorkFlowViewModel.Title = "Linked Accounts";

											WorkFlowViewModel.LinkedAccounts = GetLinkedAccounts(parsedMessage.Data);

											break;
										}
									case StatusEnum.CashWithdrawalSelectedAccount:
										{
											WorkFlowViewModel.Message = "Card and PIN Authenticated";
											WorkFlowViewModel.Title = "Account Selected";
											WorkFlowViewModel.SelectedAccountNumber = GetSelectedAccount(parsedMessage.Data);

											break;
										}
									case StatusEnum.CashWithdrawalEnteredAmount:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "Card and PIN Authenticated";
											WorkFlowViewModel.Title = string.Empty;
											WorkFlowViewModel.Amount = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Amount");

											if (cashWithdrawal04 == null)
												cashWithdrawal04 = new CashWithdrawal04();

											workflowFrame.Navigate(cashWithdrawal04);

											break;
										}
									case StatusEnum.CashWithdrawalDispenseCashSuccess:
										{
											WorkFlowViewModel.Message = "Cash dispensed.";
											WorkFlowViewModel.TransactionNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "FTNumber");

											if (cashWithdrawal05 == null)
												cashWithdrawal05 = new CashWithdrawal05();

											workflowFrame.Navigate(cashWithdrawal05);
											EnableFlowUI();

											break;
										}

									#endregion

									#region "CashWithdrawal Emirates ID"

									case StatusEnum.CashWithdrawalUseEmiratesIdSelectedAccount:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "";
											WorkFlowViewModel.Title = "Entered Account #";
											WorkFlowViewModel.SelectedAccountNumber = GetSelectedAccount(parsedMessage.Data);

											break;
										}
									case StatusEnum.CashWithdrawalUseEmiratesIdEnteredOTP:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "";
											WorkFlowViewModel.Title = "Account Selected";

											WorkFlowViewModel.EnteredOTPNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "EnteredOTP");

											if (cashWithdrawal08 == null)
												cashWithdrawal08 = new CashWithdrawal08();

											cashWithdrawal08.btnEmiratesIDConfirmed.IsEnabled = false;

											workflowFrame.Navigate(cashWithdrawal08);

											break;
										}
									case StatusEnum.CashWithdrawalAuthenticateEmiratesIdCard:
										{
											EnableFlowUI();

											if (cashWithdrawal08 == null)
												cashWithdrawal08 = new CashWithdrawal08();

											cashWithdrawal08.btnEmiratesIDConfirmed.IsEnabled = true;

											break;
										}
									case StatusEnum.CashWithdrawalUseEmiratesIdAmountEntered:
										{
											EnableFlowUI();

											WorkFlowViewModel.Title = "Selected Account.";
											WorkFlowViewModel.Message = "Emirates ID Authenticated.";
											WorkFlowViewModel.Amount = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "AmountEntered");

											if (cashWithdrawal09 == null)
												cashWithdrawal09 = new CashWithdrawal09();

											workflowFrame.Navigate(cashWithdrawal09);

											break;
										}
									case StatusEnum.CashWithdrawalUseEmiratesIdCashDispensedSucess:
										{
											EnableFlowUI();

											WorkFlowViewModel.Title = "Selected Account.";
											WorkFlowViewModel.Message = "Cash Dispensed.";

											if (cashWithdrawal11 == null)
												cashWithdrawal11 = new CashWithdrawal11();

											workflowFrame.Navigate(cashWithdrawal11);

											break;
										}
									case StatusEnum.CashWithdrawalUseEmiratesIdReceiptPrintSuccess:
										{
											EnableFlowUI();

											WorkFlowViewModel.Title = "Selected Account.";
											WorkFlowViewModel.Message = "Receipt Printed.";

											break;
										}
									case StatusEnum.CashWithdrawalReceiptPrinted:
										{
											WorkFlowViewModel.Message = "Receipt printed.";

											if (cashWithdrawal06 == null)
												cashWithdrawal06 = new CashWithdrawal06();

											workflowFrame.Navigate(cashWithdrawal06);
											EnableFlowUI();

											FlowType.Text = "None";
											break;
										}

                                    #endregion

                                    #region "CashWithdrawal Credit Card"

                                    case StatusEnum.CashWithdrawalCreditCardNumberConfirm:
                                        {
                                            EnableFlowUI();

                                            WorkFlowViewModel.Title = string.Empty;
                                            WorkFlowViewModel.CardNumber = "Card Number: " + eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "CardNumber");
                                            
                                            break;
                                        }
                                    case StatusEnum.CashWithdrawalCreditCardEnteredAmountConfirmed:
                                        {
                                            WorkFlowViewModel.Amount = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Amount");
                                            WorkFlowViewModel.Message = "Withdrawal Amount Entered.";
                                            DisableFlowUI();
                                            break;
                                        }
                                    case StatusEnum.CashWithdrawalCreditCardDispenseCashSuccess:
                                        {
                                            WorkFlowViewModel.TransactionNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "FTNumber");
                                            WorkFlowViewModel.Message = "Transaction Success.";
                                            break;
                                        }
                                    case StatusEnum.CashWithdrawalCreditCardReceiptPrinted:
                                        {
                                            WorkFlowViewModel.Message = "Receipt Printed.";
                                            
                                            EnableFlowUI();

                                            FlowType.Text = "None";
                                            break;
                                        }

                                    #endregion

                                    #endregion

                                    #region "Cash Deposit Account"

                                    case StatusEnum.CashDepositAccountLinkedAccounts:
										{
											WorkFlowViewModel.Message = "Card and PIN Authenticated";
											WorkFlowViewModel.Title = "Linked Accounts";

											WorkFlowViewModel.LinkedAccounts = GetLinkedAccounts(parsedMessage.Data);

											break;
										}
									case StatusEnum.CashDepositAccountCardNumber:
										{
											var selectedAccount = GetSelectedAccount(parsedMessage.Data);

											WorkFlowViewModel.Message = "Selected Account Number";
											WorkFlowViewModel.Title = string.Empty;
											WorkFlowViewModel.AccountNumber = selectedAccount?.AccountNumber;
											WorkFlowViewModel.SelectedAccountNumber = GetSelectedAccount(parsedMessage.Data);

											break;
										}
									case StatusEnum.CashDepositAccountCardNumberConfirmed:
										{
											EnableFlowUI();

											if (cashDepositAccountActivateCashDeposit == null)
												cashDepositAccountActivateCashDeposit = new CashDepositAccountActivateCashDeposit();

											var selectedAccount = GetSelectedAccount(parsedMessage.Data);
											WorkFlowViewModel.CardNumber = selectedAccount?.AccountNumber;

											workflowFrame.Navigate(cashDepositAccountActivateCashDeposit);

											break;
										}
									case StatusEnum.CashDepositAccountAmountEntered:
										{
											WorkFlowViewModel.TenNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.TenNote));
											WorkFlowViewModel.TwentyNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.TwentyNote));
											WorkFlowViewModel.FiftyNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.FiftyNote));
											WorkFlowViewModel.OneHundredNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.OneHundredNote));
											WorkFlowViewModel.TwoHundredNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.TwoHundredNote));
											WorkFlowViewModel.FiveHundredNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.FiveHundredNote));
											WorkFlowViewModel.OneThousandNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.OneThousandNote));

											int noteVal = 0;
											double amount = 0;
											int.TryParse(WorkFlowViewModel.TenNote, out noteVal);
											amount += noteVal * 10;

											noteVal = 0;
											int.TryParse(WorkFlowViewModel.TwentyNote, out noteVal);
											amount += noteVal * 20;

											noteVal = 0;
											int.TryParse(WorkFlowViewModel.FiftyNote, out noteVal);
											amount += noteVal * 50;

											noteVal = 0;
											int.TryParse(WorkFlowViewModel.OneHundredNote, out noteVal);
											amount += noteVal * 100;

											noteVal = 0;
											int.TryParse(WorkFlowViewModel.TwoHundredNote, out noteVal);
											amount += noteVal * 200;

											noteVal = 0;
											int.TryParse(WorkFlowViewModel.FiveHundredNote, out noteVal);
											amount += noteVal * 500;

											noteVal = 0;
											int.TryParse(WorkFlowViewModel.OneThousandNote, out noteVal);
											amount += noteVal * 1000;

											WorkFlowViewModel.Amount = amount.ToString();

											EnableFlowUI();

											if (cashDepositAccountConfirmDeposit == null)
												cashDepositAccountConfirmDeposit = new CashDepositAccountConfirmDeposit();

											cashDepositAccountConfirmDeposit.btnConfirmDeposit.IsEnabled = false;
											cashDepositAccountConfirmDeposit.btnCancelDeposit.IsEnabled = false;

											workflowFrame.Navigate(cashDepositAccountConfirmDeposit);

											break;
										}
									case StatusEnum.CashDepositAccountLimitExceeded:
										{
											EnableFlowUI();

											if (cashDepositAccountExceedLimit == null)
												cashDepositAccountExceedLimit = new CashDepositAccountExceedLimit();

											cashDepositAccountExceedLimit.btnSourceOfFunds.IsEnabled = true;
											workflowFrame.Navigate(cashDepositAccountExceedLimit);

											break;
										}
									case StatusEnum.CashDepositAccountSourceOfFundConfirmed:
										{
											EnableFlowUI();

											if (cashDepositAccountExceedLimit == null)
												cashDepositAccountExceedLimit = new CashDepositAccountExceedLimit();

											cashDepositAccountExceedLimit.btnActivateEID.IsEnabled = true;
											break;
										}
									case StatusEnum.CashDepositAccountEIDScannerActivated:
										{
											if (cashDepositAccountExceedLimit == null)
												cashDepositAccountExceedLimit = new CashDepositAccountExceedLimit();

											cashDepositAccountExceedLimit.btnActivateEID.IsEnabled = false;
											workflowFrame.Navigate(cashDepositAccountExceedLimit);

											break;
										}
									case StatusEnum.CashDepositAccountConfirmEID:
										{
											EnableFlowUI();

											if (cashDepositAccountExceedLimit == null)
												cashDepositAccountExceedLimit = new CashDepositAccountExceedLimit();

											cashDepositAccountExceedLimit.btnConfirmEID.IsEnabled = true;
											workflowFrame.Navigate(cashDepositAccountExceedLimit);
											break;
										}
									case StatusEnum.CashDepositAccountConfirmed:
										{
											EnableFlowUI();

											if (cashDepositAccountConfirmDeposit == null)
												cashDepositAccountConfirmDeposit = new CashDepositAccountConfirmDeposit();

											cashDepositAccountConfirmDeposit.btnConfirmDeposit.IsEnabled = true;
											cashDepositAccountConfirmDeposit.btnCancelDeposit.IsEnabled = true;

											workflowFrame.Navigate(cashDepositAccountConfirmDeposit);

											break;
										}
									case StatusEnum.CashDepositAccountEmiratesIdTaken:
										{
											EnableFlowUI();

											if (cashDepositAccountConfirmDeposit == null)
												cashDepositAccountConfirmDeposit = new CashDepositAccountConfirmDeposit();

											cashDepositAccountConfirmDeposit.btnConfirmDeposit.IsEnabled = true;
											cashDepositAccountConfirmDeposit.btnCancelDeposit.IsEnabled = true;

											workflowFrame.Navigate(cashDepositAccountConfirmDeposit);

											break;
										}
									case StatusEnum.CashDepositAccountPaymentSuccess:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "Cash Payment Success";
											WorkFlowViewModel.Title = string.Empty;
											WorkFlowViewModel.TransactionNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "FTNumber");

											if (cashDepositAccountPrintReceipt == null)
												cashDepositAccountPrintReceipt = new CashDepositAccountPrintReceipt();

											workflowFrame.Navigate(cashDepositAccountPrintReceipt);

											break;
										}
									case StatusEnum.CashDepositAccountEIDUploaded:
										{
											WorkFlowViewModel.Message = "EID Uploaded";
											WorkFlowViewModel.Title = string.Empty;

											break;
										}
									case StatusEnum.CashDepositAccountReceiptPrinted:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "Receipt Printed";
											WorkFlowViewModel.Title = string.Empty;

											FlowType.Text = "None";

											break;
										}

									#endregion

									#region "Credit Card Payment With Cash"

									case StatusEnum.CreditCardPaymentWithCashCardNumber:
										{
											WorkFlowViewModel.Message = "Card Number";
											WorkFlowViewModel.Title = string.Empty;
											WorkFlowViewModel.CardNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "CardNumber");

											break;
										}
									case StatusEnum.CreditCardPaymentWithCashCardNumberConfirmed:
										{
											EnableFlowUI();

											if (creditCardPaymentWithCashActivateCashDeposit == null)
												creditCardPaymentWithCashActivateCashDeposit = new CreditCardPaymentWithCashActivateCashDeposit();

											workflowFrame.Navigate(creditCardPaymentWithCashActivateCashDeposit);

											break;
										}
									case StatusEnum.CreditCardPaymentWithCashAmountEntered:
										{
											WorkFlowViewModel.TenNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.TenNote));
											WorkFlowViewModel.TwentyNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.TwentyNote));
											WorkFlowViewModel.FiftyNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.FiftyNote));
											WorkFlowViewModel.OneHundredNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.OneHundredNote));
											WorkFlowViewModel.TwoHundredNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.TwoHundredNote));
											WorkFlowViewModel.FiveHundredNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.FiveHundredNote));
											WorkFlowViewModel.OneThousandNote = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, nameof(CurrenceyNoteType.OneThousandNote));

											int noteVal = 0;
											double amount = 0;
											int.TryParse(WorkFlowViewModel.TenNote, out noteVal);
											amount += noteVal * 10;

											noteVal = 0;
											int.TryParse(WorkFlowViewModel.TwentyNote, out noteVal);
											amount += noteVal * 20;

											noteVal = 0;
											int.TryParse(WorkFlowViewModel.FiftyNote, out noteVal);
											amount += noteVal * 50;

											noteVal = 0;
											int.TryParse(WorkFlowViewModel.OneHundredNote, out noteVal);
											amount += noteVal * 100;

											noteVal = 0;
											int.TryParse(WorkFlowViewModel.TwoHundredNote, out noteVal);
											amount += noteVal * 200;

											noteVal = 0;
											int.TryParse(WorkFlowViewModel.FiveHundredNote, out noteVal);
											amount += noteVal * 500;

											noteVal = 0;
											int.TryParse(WorkFlowViewModel.OneThousandNote, out noteVal);
											amount += noteVal * 1000;

											WorkFlowViewModel.Amount = amount.ToString();

											EnableFlowUI();

											if (creditCardPaymentWithCashConfirmDeposit == null)
												creditCardPaymentWithCashConfirmDeposit = new CreditCardPaymentWithCashConfirmDeposit();

											workflowFrame.Navigate(creditCardPaymentWithCashConfirmDeposit);

											break;
										}
									case StatusEnum.CreditCardPaymentWithCashLimitExceeded:
										{
											EnableFlowUI();

											if (creditCardPaymentWithCashExceedLimit == null)
												creditCardPaymentWithCashExceedLimit = new CreditCardPaymentWithCashExceedLimit();

											creditCardPaymentWithCashExceedLimit.btnSourceOfFunds.IsEnabled = true;
											workflowFrame.Navigate(creditCardPaymentWithCashExceedLimit);

											break;
										}
									case StatusEnum.CreditCardPaymentWithCashConfirmEID:
										{
											EnableFlowUI();

											if (creditCardPaymentWithCashExceedLimit == null)
												creditCardPaymentWithCashExceedLimit = new CreditCardPaymentWithCashExceedLimit();

											creditCardPaymentWithCashExceedLimit.btnConfirmEID.IsEnabled = true;
											workflowFrame.Navigate(creditCardPaymentWithCashExceedLimit);
											break;
										}
									case StatusEnum.CreditCardPaymentWithCashConfirmed:
										{
											EnableFlowUI();

											if (creditCardPaymentWithCashConfirmDeposit == null)
												creditCardPaymentWithCashConfirmDeposit = new CreditCardPaymentWithCashConfirmDeposit();

											creditCardPaymentWithCashConfirmDeposit.btnConfirmDeposit.IsEnabled = true;
											creditCardPaymentWithCashConfirmDeposit.btnCancelDeposit.IsEnabled = true;

											workflowFrame.Navigate(creditCardPaymentWithCashConfirmDeposit);

											break;
										}
									case StatusEnum.CreditCardPaymentWithCashSourceOfFundConfirmed:
										{
											EnableFlowUI();

											if (creditCardPaymentWithCashExceedLimit == null)
												creditCardPaymentWithCashExceedLimit = new CreditCardPaymentWithCashExceedLimit();

											creditCardPaymentWithCashExceedLimit.btnActivateEID.IsEnabled = true;

											break;
										}
									case StatusEnum.CreditCardPaymentWithCashEIDScannerActivated:
										{
											if (creditCardPaymentWithCashExceedLimit == null)
												creditCardPaymentWithCashExceedLimit = new CreditCardPaymentWithCashExceedLimit();

											creditCardPaymentWithCashExceedLimit.btnActivateEID.IsEnabled = false;
											workflowFrame.Navigate(creditCardPaymentWithCashExceedLimit);

											break;
										}
									case StatusEnum.CreditCardPaymentWithCashEmiratesIdTaken:
										{
											EnableFlowUI();

											if (creditCardPaymentWithCashConfirmDeposit == null)
												creditCardPaymentWithCashConfirmDeposit = new CreditCardPaymentWithCashConfirmDeposit();

											workflowFrame.Navigate(creditCardPaymentWithCashConfirmDeposit);

											break;
										}
									case StatusEnum.CreditCardPaymentWithCashPaymentSuccess:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "Cash Payment Success";
											WorkFlowViewModel.Title = string.Empty;
											WorkFlowViewModel.TransactionNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "FTNumber");

											if (creditCardPaymentWithCashPrintReceipt == null)
												creditCardPaymentWithCashPrintReceipt = new CreditCardPaymentWithCashPrintReceipt();

											workflowFrame.Navigate(creditCardPaymentWithCashPrintReceipt);

											break;
										}
									case StatusEnum.CreditCardPaymentWithCashEIDUploaded:
										{
											WorkFlowViewModel.Message = "EID Uploaded";
											WorkFlowViewModel.Title = string.Empty;

											break;
										}
									case StatusEnum.CreditCardPaymentWithCashReceiptPrinted:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "Receipt Printed";
											WorkFlowViewModel.Title = string.Empty;

											FlowType.Text = "None";
											break;
										}

									#endregion

									#region "ChequeDepositAccount"

									case StatusEnum.ChequeDepositAccountAccountNumber:
										{
											WorkFlowViewModel.Message = "Account Number";
											WorkFlowViewModel.Title = string.Empty;
											WorkFlowViewModel.CardNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "AccountNumber");
											Logger.Writer.Info("Account Number" + WorkFlowViewModel.CardNumber);
											break;
										}
									case StatusEnum.ChequeDepositAccountCardNumberConfirmed:
										{
											EnableFlowUI();

											if (chequeDepositAccountActivateChequeDeposit == null)
												chequeDepositAccountActivateChequeDeposit = new ChequeDepositAccountActivateChequeDeposit();

											chequeDepositAccountActivateChequeDeposit.btnActivateDeposit.IsEnabled = true;
											workflowFrame.Navigate(chequeDepositAccountActivateChequeDeposit);

											break;
										}
									//case StatusEnum.ChequeDepositAccountAmountEntered:
									//    {
									//        EnableFlowUI();

									//        WorkFlowViewModel.Amount = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Amount");

									//        if (chequeDepositAccountActivateChequeDeposit == null)
									//            chequeDepositAccountActivateChequeDeposit = new ChequeDepositAccountActivateChequeDeposit();

									//        workflowFrame.Navigate(chequeDepositAccountActivateChequeDeposit);

									//        break;
									//    }
									case StatusEnum.ChequeDepositAccountChequeInserted:
										{
											WorkFlowViewModel.Title = "Cheque inserted.";

											break;
										}
									case StatusEnum.ChequeDepositAccountChequeScanning:
										{
											WorkFlowViewModel.Title = "Cheque scanning.";

											break;
										}
									case StatusEnum.ChequeDepositAccountChequeScanningDone:
										{
											WorkFlowViewModel.Title = "Cheque scanning done.";

											break;
										}
									case StatusEnum.ChequeDepositAccountConfirmDeposit:
										{
											EnableFlowUI();

											WorkFlowViewModel.Amount = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Amount");
											WorkFlowViewModel.Date = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Date");
											if (chequeDepositAccountConfirmDeposit == null)
												chequeDepositAccountConfirmDeposit = new ChequeDepositAccountConfirmDeposit();
											workflowFrame.Navigate(chequeDepositAccountConfirmDeposit);

											break;
										}
									case StatusEnum.ChequeDepositAccountDepositSuccess:
										{
											EnableFlowUI();

											if (chequeDepositAccountPrintReceipt == null)
												chequeDepositAccountPrintReceipt = new ChequeDepositAccountPrintReceipt();

											workflowFrame.Navigate(chequeDepositAccountPrintReceipt);

											break;
										}
									case StatusEnum.ChequeDepositEncashChequeSuccess:
										{
											EnableFlowUI();

											if (chequeDepositAccountPrintReceipt == null)
												chequeDepositAccountPrintReceipt = new ChequeDepositAccountPrintReceipt();

											workflowFrame.Navigate(chequeDepositAccountPrintReceipt);

											break;
										}
									case StatusEnum.ChequeDepositAccountReceiptPrinted:
										{
											EnableFlowUI();
											if (chequeDepositAccountPrintReceipt == null)
												chequeDepositAccountPrintReceipt = new ChequeDepositAccountPrintReceipt();
											chequeDepositAccountPrintReceipt.btnPrintReceipt.IsEnabled = false;
											workflowFrame.Navigate(chequeDepositAccountPrintReceipt);

											FlowType.Text = "None";
											break;
										}

									#endregion

									#region "ChequeDepositCreditCard"

									case StatusEnum.ChequeDepositCreditCardCardNumber:
										{
											WorkFlowViewModel.Message = "Card Number";
											WorkFlowViewModel.Title = string.Empty;
											WorkFlowViewModel.CardNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "CardNumber");

											break;
										}
									case StatusEnum.ChequeDepositCreditCardCardNumberConfirmed:
										{
											EnableFlowUI();

											if (chequeDepositCreditCardActivateChequeDeposit == null)
												chequeDepositCreditCardActivateChequeDeposit = new ChequeDepositCreditCardActivateChequeDeposit();

											chequeDepositCreditCardActivateChequeDeposit.btnActivateDeposit.IsEnabled = true;
											workflowFrame.Navigate(chequeDepositCreditCardActivateChequeDeposit);
											break;
										}
									//case StatusEnum.ChequeDepositCreditCardAmountEntered:
									//    {
									//        EnableFlowUI();

									//        WorkFlowViewModel.Amount = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Amount");

									//        if (chequeDepositCreditCardActivateChequeDeposit == null)
									//            chequeDepositCreditCardActivateChequeDeposit = new ChequeDepositCreditCardActivateChequeDeposit();

									//        workflowFrame.Navigate(chequeDepositCreditCardActivateChequeDeposit);

									//        break;
									//    }
									case StatusEnum.ChequeDepositCreditCardChequeInserted:
										{
											WorkFlowViewModel.Title = "Cheque inserted.";

											break;
										}
									case StatusEnum.ChequeDepositCreditCardChequeScanning:
										{
											WorkFlowViewModel.Title = "Cheque scanning.";

											break;
										}
									case StatusEnum.ChequeDepositCreditCardChequeScanningDone:
										{
											WorkFlowViewModel.Title = "Cheque scanning done.";

											break;
										}
									case StatusEnum.ChequeDepositCreditCardConfirmDeposit:
										{
											EnableFlowUI();

											WorkFlowViewModel.Amount = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Amount");

											if (chequeDepositCreditCardConfirmDeposit == null)
												chequeDepositCreditCardConfirmDeposit = new ChequeDepositCreditCardConfirmDeposit();

											workflowFrame.Navigate(chequeDepositCreditCardConfirmDeposit);

											break;
										}
									case StatusEnum.ChequeDepositCreditCardDepositSuccess:
										{
											EnableFlowUI();

											if (chequeDepositCreditCardPrintReceipt == null)
												chequeDepositCreditCardPrintReceipt = new ChequeDepositCreditCardPrintReceipt();
											workflowFrame.Navigate(chequeDepositCreditCardPrintReceipt);

											break;
										}
									case StatusEnum.ChequeDepositCreditCardReceiptPrinted:
										{
											EnableFlowUI();

											break;
										}

									#endregion

									#region "CreditCardPaymentWithAccount"

									case StatusEnum.CreditCardPaymentWithAccountCardNumber:
										{
											WorkFlowViewModel.CardNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "CardNumber");

											break;
										}
									case StatusEnum.CreditCardPaymentWithAccountSelectedAccountNumber:
										{
											WorkFlowViewModel.SelectedAccountNumber = GetSelectedAccount(parsedMessage.Data);

											break;
										}
									case StatusEnum.CreditCardPaymentWithAccountPaymentSuccess:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "";
											WorkFlowViewModel.Title = "Successful Payment.";
											WorkFlowViewModel.TransactionNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "TransactionNumber");
											WorkFlowViewModel.Amount = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Amount");
											WorkFlowViewModel.AccountCurrency = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Currency");

											if (creditCardPaymentWithAccountPrintReceipt == null)
												creditCardPaymentWithAccountPrintReceipt = new CreditCardPaymentWithAccountPrintReceipt();
											workflowFrame.Navigate(creditCardPaymentWithAccountPrintReceipt);

											break;
										}
									case StatusEnum.CreditCardPaymentWithAccountPrinted:
										{
											WorkFlowViewModel.Message = "Receipt printed.";
											EnableFlowUI();

											break;
										}

									#endregion

									#region "AccountInquiry"

									#region "AccountInquiry Debit Card"

									case StatusEnum.AccountInquiryWaitingForPin:
										{
											if (accountInquiry02 == null)
												accountInquiry02 = new AccountInquiry02();

											WorkFlowViewModel.Message = "Waiting for PIN...";

											break;
										}
									case StatusEnum.AccountInquiryWaitingForAuthentication:
										{
											if (accountInquiry02 == null)
												accountInquiry02 = new AccountInquiry02();

											WorkFlowViewModel.Message = "Waiting for authentication...";

											break;
										}
									case StatusEnum.AccountInquiryCardAuthenticated:
										{
											EnableFlowUI();

											if (accountInquiry02 == null)
												accountInquiry02 = new AccountInquiry02();

											WorkFlowViewModel.Message = "Card and PIN Authenticated";

											break;
										}
									case StatusEnum.AccountInquiryCardAccounts:
										{
											EnableFlowUI();

											WorkFlowViewModel.Title = "Card and PIN authenticated";
											WorkFlowViewModel.Message = "LINKED ACCOUNTS";
											WorkFlowViewModel.LinkedAccounts = GetLinkedAccounts(parsedMessage.Data);

											if (accountInquiry02 == null)
												accountInquiry02 = new AccountInquiry02();

											accountInquiry02.grdAccounts.Visibility = Visibility.Visible;
											accountInquiry02.grdMessage.Visibility = Visibility.Collapsed;

											break;
										}
									case StatusEnum.AccountInquiryCompleted:
										{
											EnableFlowUI();

											break;
										}

									#endregion

									#region "AccountInquiry Emirates ID"

									case StatusEnum.AccountInquiryUseEmiratesIdEnteredOTP:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "";
											WorkFlowViewModel.Title = "Account Selected";

											WorkFlowViewModel.EnteredOTPNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "EnteredOTP");

											if (accountInquiry04 == null)
												accountInquiry04 = new AccountInquiry04();

											accountInquiry04.btnEmiratesIDConfirmed.IsEnabled = false;
											workflowFrame.Navigate(accountInquiry04);

											break;
										}
									case StatusEnum.AccountInquiryAuthenticateEmiratesIdCard:
										{
											EnableFlowUI();

											if (accountInquiry04 == null)
												accountInquiry04 = new AccountInquiry04();

											accountInquiry04.btnEmiratesIDConfirmed.IsEnabled = true;

											break;
										}
									case StatusEnum.AccountInquiryUseEmiratesIdSelectedAccount:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "";
											WorkFlowViewModel.Title = "Account Selected";

											WorkFlowViewModel.SelectedAccountNumber = GetSelectedAccount(parsedMessage.Data);

											break;
										}
									case StatusEnum.AccountInquiryEmiratesIdTaken:
										{
											EnableFlowUI();

											break;
										}

									#endregion

									#endregion

									#region "FundsTransferOwnAccount"

									case StatusEnum.FundsTransferOwnAccountUseDebitCardCardAuthenticated:
										{
											if (fundsTransfer02 == null)
												fundsTransfer02 = new FundsTransferOwnAccountLinkedAccounts();
											workflowFrame.Navigate(fundsTransfer02);

											break;
										}
									case StatusEnum.FundsTransferOwnAccountFundsTransferAccounts:
										{
											WorkFlowViewModel.Message = "Card and PIN Authenticated";
											WorkFlowViewModel.Title = "Fund Transfer Accounts";
											WorkFlowViewModel.LinkedAccounts = GetFundsTransferAccounts(parsedMessage.Data);

											break;
										}
									case StatusEnum.FundsTransferOwnAccountTransactionSuccess:
										{
											EnableFlowUI();

											WorkFlowViewModel.Amount = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Amount");
											WorkFlowViewModel.AccountCurrency = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "AccountCurrency");

											if (fundsTransfer03 == null)
												fundsTransfer03 = new FundsTransferOwnAccountPrintReceipt();
											workflowFrame.Navigate(fundsTransfer03);

											break;
										}
									case StatusEnum.FundsTransferOwnAccountReceiptPrinted:
										{
											EnableFlowUI();

											if (fundsTransfer03 == null)
												fundsTransfer03 = new FundsTransferOwnAccountPrintReceipt();
											fundsTransfer03.btnPrintReceipt.IsEnabled = false;

											break;
										}

									#endregion

									#region "FundsTransferOtherAccount"

									case StatusEnum.FundsTransferOtherAccountUseDebitCardCardAuthenticated:
										{
											if (fundsTransferOther02 == null)
												fundsTransferOther02 = new FundsTransferOtherAccountLinkedAccounts();
											workflowFrame.Navigate(fundsTransferOther02);

											break;
										}
									case StatusEnum.FundsTransferOtherAccountFundsTransferAccounts:
										{
											WorkFlowViewModel.Message = "Card and PIN Authenticated";
											WorkFlowViewModel.Title = "Fund Transfer Accounts";
											WorkFlowViewModel.LinkedAccounts = GetFundsTransferAccounts(parsedMessage.Data);

											break;
										}
									case StatusEnum.FundsTransferOtherAccountTransactionSuccess:
										{
											EnableFlowUI();

											WorkFlowViewModel.Amount = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Amount");
											WorkFlowViewModel.AccountCurrency = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "AccountCurrency");

											if (fundsTransferOther03 == null)
												fundsTransferOther03 = new FundsTransferOtherAccountPrintReceipt();

											workflowFrame.Navigate(fundsTransferOther03);

											break;
										}
									case StatusEnum.FundsTransferOtherAccountReceiptPrinted:
										{
											EnableFlowUI();

											if (fundsTransferOther03 == null)
												fundsTransferOther03 = new FundsTransferOtherAccountPrintReceipt();
											fundsTransferOther03.btnPrintReceipt.IsEnabled = false;

											break;
										}

									#endregion

									#region "CreateBeneficiary"

									case StatusEnum.CreateBeneficiaryUseDebitCardCardAuthenticated:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "Card and PIN Authenticated";
											WorkFlowViewModel.Title = "Fund Transfer Accounts";

											if (createBeneficiaryLinkedAccounts == null)
												createBeneficiaryLinkedAccounts = new CreateBeneficiaryLinkedAccounts();
											workflowFrame.Navigate(createBeneficiaryLinkedAccounts);

											break;
										}
									case StatusEnum.CreateBeneficiaryUseDebitCardLinkedAccounts:
										{
											EnableFlowUI();

											WorkFlowViewModel.Title = "Card and PIN authenticated";
											WorkFlowViewModel.Message = "LINKED ACCOUNTS";
											WorkFlowViewModel.LinkedAccounts = GetLinkedAccounts(parsedMessage.Data);

											break;
										}

									#endregion

									#region "AdditionalAccountOpening"

									case StatusEnum.AdditionalAccountOpeningRequestByRTCardNumber:
										{
											WorkFlowViewModel.Message = "Account Number";
											WorkFlowViewModel.AccountNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "AccountNumber");

											break;
										}
									case StatusEnum.AdditionalAccountOpeningRequestByRTAccountDetailsEditing:
										{
											if (additionalAccountOpeningSelectedAccount == null)
												additionalAccountOpeningSelectedAccount = new AdditionalAccountOpeningSelectedAccount();
											workflowFrame.Navigate(additionalAccountOpeningSelectedAccount);

											DisableFlowUI();

											break;
										}
									case StatusEnum.AdditionalAccountOpeningRequestByRTAccountTypeSelected:
										{
											WorkFlowViewModel.Message = "Account Type";
											WorkFlowViewModel.AccountType = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "AccountType");

											break;
										}
									case StatusEnum.AdditionalAccountOpeningRequestByRTCurrencySelected:
										{
											WorkFlowViewModel.Message = "Currency";
											WorkFlowViewModel.AccountCurrency = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "AccountCurrency");

											break;
										}
									case StatusEnum.AdditionalAccountOpeningRequestByRTAccountDetailSpecified:
										{
											EnableFlowUI();

											if (additionalAccountOpeningCaptureSignature == null)
												additionalAccountOpeningCaptureSignature = new AdditionalAccountOpeningCaptureSignature();
											workflowFrame.Navigate(additionalAccountOpeningCaptureSignature);

											break;
										}
									case StatusEnum.AdditionalAccountOpeningRequestByRTSignature1Captured:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "Signature 1 Captured";
											break;
										}
									case StatusEnum.AdditionalAccountOpeningRequestByRTSignature2Captured:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "Signature 2 Captured";
											break;
										}
									case StatusEnum.AdditionalAccountOpeningRequestByRTVerifyFormByRT:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "Verify Form";

											if (additionalAccountOpeningInitiateOnScreenForm == null)
												additionalAccountOpeningInitiateOnScreenForm = new AdditionalAccountOpeningInitiateOnScreenForm();
											workflowFrame.Navigate(additionalAccountOpeningInitiateOnScreenForm);

											break;
										}
									case StatusEnum.AdditionalAccountOpeningRequestByRTFormConfirmedByCustomer:
										{
											EnableFlowUI();

											if (additionalAccountOpeningEIDScan == null)
												additionalAccountOpeningEIDScan = new AdditionalAccountOpeningEIDScan();
											additionalAccountOpeningEIDScan.btnActivateEID.IsEnabled = true;
											additionalAccountOpeningEIDScan.btnConfirmEID.IsEnabled = false;

											workflowFrame.Navigate(additionalAccountOpeningEIDScan);
											break;
										}
									case StatusEnum.AdditionalAccountOpeningRequestByRTFormUploaded:
										{
											WorkFlowViewModel.Message = "Form Uploaded";

											if (additionalAccountOpeningEndSession == null)
												additionalAccountOpeningEndSession = new AdditionalAccountOpeningEndSession();
											workflowFrame.Navigate(additionalAccountOpeningEndSession);

											break;
										}
									case StatusEnum.AdditionalAccountOpeningRequestByRTEmiratesIdUploaded:
										{
											WorkFlowViewModel.Message = "Emirates Id Uploaded";

											if (additionalAccountOpeningEndSession == null)
												additionalAccountOpeningEndSession = new AdditionalAccountOpeningEndSession();
											workflowFrame.Navigate(additionalAccountOpeningEndSession);

											break;
										}
									case StatusEnum.AdditionalAccountOpeningRequestByRTEmailSent:
										{
											EnableFlowUI();
											WorkFlowViewModel.Message = "Email sent";

											if (additionalAccountOpeningEndSession == null)
												additionalAccountOpeningEndSession = new AdditionalAccountOpeningEndSession();
											workflowFrame.Navigate(additionalAccountOpeningEndSession);

											break;
										}
									case StatusEnum.AdditionalAccountOpeningRequestByRTFgbError:
										{
											WorkFlowViewModel.Message = "Error while sending request to AHB";

											if (additionalAccountOpeningEndSession == null)
												additionalAccountOpeningEndSession = new AdditionalAccountOpeningEndSession();
											workflowFrame.Navigate(additionalAccountOpeningEndSession);

											break;
										}
									case StatusEnum.ScanEmiratesIdStepVerifyEmiratesId:
										{
											EnableFlowUI();

											if (additionalAccountOpeningEIDScan == null)
												additionalAccountOpeningEIDScan = new AdditionalAccountOpeningEIDScan();

											additionalAccountOpeningEIDScan.btnActivateEID.IsEnabled = false;
											additionalAccountOpeningEIDScan.btnConfirmEID.IsEnabled = true;
											workflowFrame.Navigate(additionalAccountOpeningEIDScan);
											break;
										}

									#endregion

									#region "OTPSent"

									case StatusEnum.OTPInitializing:
										{
											WorkFlowViewModel.OTPResendCount = 0;
											WorkFlowViewModel.OTPSentCount = 0;

											if (authenticationFlowActivateOTP == null)
												authenticationFlowActivateOTP = new AuthenticationFlowActivateOTP();

											workflowFrame.Navigate(authenticationFlowActivateOTP);

											break;
										}
									case StatusEnum.OTPSent:
										{
											WorkFlowViewModel.OTPResendCount = eSpaceMediaOcx.FindIntValueFromJson(parsedMessage.Data, "OTPResendCount");
											WorkFlowViewModel.OTPSentCount = eSpaceMediaOcx.FindIntValueFromJson(parsedMessage.Data, "OTPSentCount");

											break;
										}

									#endregion

									#region "ChequePrinting"

									case StatusEnum.ChequePrintingLinkedAccounts:
										{
											WorkFlowViewModel.Message = "LINKED ACCOUNTS";
											WorkFlowViewModel.Title = string.Empty;
											WorkFlowViewModel.LinkedAccounts = GetLinkedAccounts(parsedMessage.Data);

											break;
										}
									case StatusEnum.ChequePrintingSelectedAccount:
										{
											WorkFlowViewModel.LinkedAccounts = new ObservableCollection<Core.Model.LinkedAccount>();

											chequePrinting01?.LinkedAccounts?.ResetItemSource();

											WorkFlowViewModel.Message = "ACCOUNT SELECTED";
											WorkFlowViewModel.Title = string.Empty;
											WorkFlowViewModel.SelectedAccountNumber = GetSelectedAccount(parsedMessage.Data);

											break;
										}
									case StatusEnum.ChequePrintingCharges:
										{
											WorkFlowViewModel.Message = "CHARGES";
											WorkFlowViewModel.Title = string.Empty;
											WorkFlowViewModel.Charges = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Charges");

											break;
										}
									case StatusEnum.ChequePrintingPrintingLeaf:
										{
											EnableFlowUI();

											if (chequePrinting02 == null)
												chequePrinting02 = new ChequePrinting02();

											workflowFrame.Navigate(chequePrinting02);

											break;
										}

									#endregion

									#region "End"

									case StatusEnum.UpdateCustomerDetailsOTPValidated:
										{
											EnableFlowUI();

											if (updateCustomerDetailsEIDScan == null)
												updateCustomerDetailsEIDScan = new UpdateCustomerDetailsEIDScan();
											updateCustomerDetailsEIDScan.btnActivateEID.IsEnabled = true;
											updateCustomerDetailsEIDScan.btnConfirmEID.IsEnabled = false;

											workflowFrame.Navigate(updateCustomerDetailsEIDScan);
											break;
										}
									case StatusEnum.UpdateCustomerDetailsScanEmiratesIdToVerify:
										{
											EnableFlowUI();

											if (updateCustomerDetailsEIDScan == null)
												updateCustomerDetailsEIDScan = new UpdateCustomerDetailsEIDScan();

											updateCustomerDetailsEIDScan.btnActivateEID.IsEnabled = false;
											updateCustomerDetailsEIDScan.btnConfirmEID.IsEnabled = true;
											workflowFrame.Navigate(updateCustomerDetailsEIDScan);
											break;
										}

									#endregion

									#region "BillPaymentWithCreditCard"

									case StatusEnum.BillPaymentWithCreditCardCardNumber:
										{
											WorkFlowViewModel.CardNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "CardNumber");

											break;
										}
									case StatusEnum.BillPaymentWithCreditCardConsumerNumber:
										{
											WorkFlowViewModel.SelectedAccountNumber = GetSelectedAccount(parsedMessage.Data);

											break;
										}
									case StatusEnum.BillPaymentWithCreditCardPaymentSuccess:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "";
											WorkFlowViewModel.Title = "Successful Payment.";
											WorkFlowViewModel.TransactionNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "TransactionNumber");
											WorkFlowViewModel.Amount = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Amount");
											WorkFlowViewModel.AccountCurrency = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Currency");

											if (billPaymentWithCreditCardPrintReceipt == null)
												billPaymentWithCreditCardPrintReceipt = new BillPaymentWithCreditCardPrintReceipt();

											billPaymentWithCreditCardPrintReceipt.btnPrintReceipt.IsEnabled = true;

											workflowFrame.Navigate(billPaymentWithCreditCardPrintReceipt);

											break;
										}
									case StatusEnum.BillPaymentWithCreditCardPrinted:
										{
											WorkFlowViewModel.Message = "Receipt printed.";
											EnableFlowUI();

											break;
										}

									#endregion

									#region "BillPaymentWithAccount"

									case StatusEnum.BillPaymentWithAccountCardNumber:
										{
											WorkFlowViewModel.CardNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "CardNumber");

											break;
										}
									case StatusEnum.BillPaymentWithAccountConsumerNumber:
										{
											WorkFlowViewModel.SelectedAccountNumber = GetSelectedAccount(parsedMessage.Data);

											break;
										}
									case StatusEnum.BillPaymentWithAccountPaymentSuccess:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "";
											WorkFlowViewModel.Title = "Successful Payment.";
											WorkFlowViewModel.TransactionNumber = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "TransactionNumber");
											WorkFlowViewModel.Amount = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Amount");
											WorkFlowViewModel.AccountCurrency = eSpaceMediaOcx.FindValueFromJson(parsedMessage.Data, "Currency");

											if (billPaymentWithAccountPrintReceipt == null)
												billPaymentWithAccountPrintReceipt = new BillPaymentWithAccountPrintReceipt();

											billPaymentWithAccountPrintReceipt.btnPrintReceipt.IsEnabled = true;

											workflowFrame.Navigate(billPaymentWithAccountPrintReceipt);

											break;
										}
									case StatusEnum.BillPaymentWithAccountPrinted:
										{
											WorkFlowViewModel.Message = "Receipt printed.";
											EnableFlowUI();

											break;
										}

									#endregion

									#region "TransactionFailed"

									case StatusEnum.RollbackStarted:
										{
											DisableFlowUI();

											WorkFlowViewModel.Message = "Rollback Started";
											WorkFlowViewModel.Title = string.Empty;
											break;
										}
									case StatusEnum.TransactionFailed:
										{
											EnableFlowUI();

											WorkFlowViewModel.Message = "";
											WorkFlowViewModel.Title = "Transaction declined.";

											if (transactionDeclinedPrintReceipt == null)
												transactionDeclinedPrintReceipt = new TransactionDeclinedPrintReceipt();

											transactionDeclinedPrintReceipt.btnPrintReceipt.IsEnabled = true;

											workflowFrame.Navigate(transactionDeclinedPrintReceipt);

											break;
										}
									case StatusEnum.TransactionDeclinedPrinted:
										{
											EnableFlowUI();
											WorkFlowViewModel.Message = "Receipt printed.";
											break;
										}

									#endregion

									#region "End"

									case StatusEnum.CardInsertSuccess:
										{
											EnableFlowUI();
											break;
										}
									case StatusEnum.PINAuthenticationEndCurrentSession:
										{
											WorkFlowViewModel.Message = "PIN authentication .";
											ReInitializeWorkflow();
											EnableFlowUI();
											break;
										}
									case StatusEnum.AuthenticationFailedEndCurrentSession:
										{
											GlobalInfo.IsAuthenticated = false;

											if (mainWF == null)
												mainWF = new Main();

											workflowFrame.Navigate(mainWF);
											ReInitializeWorkflow();
											EnableFlowUI();
											break;
										}
									case StatusEnum.EndCurrentSession:
										{
											if (mainWF == null)
												mainWF = new Main();

											workflowFrame.Navigate(mainWF);
											ReInitializeWorkflow();
											EnableFlowUI();
											break;
										}

									#endregion

									case StatusEnum.StartSession:
										{
											EnableUI();
											break;
										}
									case StatusEnum.SendSessionLanguage:
										{
											var language = eSpaceMediaOcx.GetSelectedLanguage(parsedMessage.Data);
											WpfMessageBox.Show($"Customer selected language: {language}", "Info", WpfMessageBoxButton.OK);
											break;
										}
								}

								break;
							}
						case MessageType.Device:
							{
								#region "Update Device Status"

								var devType = eSpaceMediaOcx.GetDeviceType(parsedMessage.Data);
								var devStatus = eSpaceMediaOcx.GetDeviceStatus(parsedMessage.Data);

								switch (devType)
								{
									case DeviceType.CashDispenser:
										{
											DevicesControl.ViewModel.CashDispenser.DeviceName = DeviceType.CashDispenser.ToString();
											DevicesControl.ViewModel.CashDispenser.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.CardReader:
										{
											DevicesControl.ViewModel.CardReader.DeviceName = DeviceType.CardReader.ToString();
											DevicesControl.ViewModel.CardReader.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.Scanner:
										{
											DevicesControl.ViewModel.Scanner.DeviceName = DeviceType.Scanner.ToString();
											DevicesControl.ViewModel.Scanner.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.ReceiptPrinter:
										{
											DevicesControl.ViewModel.ReceiptPrinter.DeviceName = DeviceType.ReceiptPrinter.ToString();
											DevicesControl.ViewModel.ReceiptPrinter.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.StatementPrinter:
										{
											DevicesControl.ViewModel.StatementPrinter.DeviceName = DeviceType.StatementPrinter.ToString();
											DevicesControl.ViewModel.StatementPrinter.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.PinPad:
										{
											DevicesControl.ViewModel.PinPad.DeviceName = DeviceType.PinPad.ToString();
											DevicesControl.ViewModel.PinPad.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.Doors:
										{
											DevicesControl.ViewModel.Doors.DeviceName = DeviceType.Doors.ToString();
											DevicesControl.ViewModel.Doors.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.Sensors:
										{
											DevicesControl.ViewModel.Sensors.DeviceName = DeviceType.Sensors.ToString();
											DevicesControl.ViewModel.Sensors.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.Camera:
										{
											DevicesControl.ViewModel.Camera.DeviceName = DeviceType.Camera.ToString();
											DevicesControl.ViewModel.Camera.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.RFIDReader:
										{
											DevicesControl.ViewModel.RFIDReader.DeviceName = DeviceType.RFIDReader.ToString();
											DevicesControl.ViewModel.RFIDReader.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.Auxiliaries:
										{
											DevicesControl.ViewModel.Auxiliaries.DeviceName = DeviceType.Auxiliaries.ToString();
											DevicesControl.ViewModel.Auxiliaries.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.SignPad:
										{
											DevicesControl.ViewModel.SignPad.DeviceName = DeviceType.SignPad.ToString();
											DevicesControl.ViewModel.SignPad.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.Indicators:
										{
											DevicesControl.ViewModel.Indicators.DeviceName = DeviceType.Indicators.ToString();
											DevicesControl.ViewModel.Indicators.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.DVCSignal:
										{
											DevicesControl.ViewModel.DVCSignal.DeviceName = DeviceType.DVCSignal.ToString();
											DevicesControl.ViewModel.DVCSignal.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.VDM:
										{
											DevicesControl.ViewModel.VDM.DeviceName = DeviceType.VDM.ToString();
											DevicesControl.ViewModel.VDM.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.VFD:
										{
											DevicesControl.ViewModel.VFD.DeviceName = DeviceType.VFD.ToString();
											DevicesControl.ViewModel.VFD.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.TMD:
										{
											DevicesControl.ViewModel.TMD.DeviceName = DeviceType.TMD.ToString();
											DevicesControl.ViewModel.TMD.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.ChequeScanner:
										{
											DevicesControl.ViewModel.ChequeScanner.DeviceName = DeviceType.ChequeScanner.ToString();
											DevicesControl.ViewModel.ChequeScanner.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.JournalPrinter:
										{
											DevicesControl.ViewModel.JournalPrinter.DeviceName = DeviceType.JournalPrinter.ToString();
											DevicesControl.ViewModel.JournalPrinter.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.CashAcceptor:
										{
											DevicesControl.ViewModel.CashAcceptor.DeviceName = DeviceType.CashAcceptor.ToString();
											DevicesControl.ViewModel.CashAcceptor.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.IDScanner:
										{
											DevicesControl.ViewModel.IDScanner.DeviceName = DeviceType.IDScanner.ToString();
											DevicesControl.ViewModel.IDScanner.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.A4Printer:
										{
											DevicesControl.ViewModel.A4Printer.DeviceName = DeviceType.A4Printer.ToString();
											DevicesControl.ViewModel.A4Printer.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.A4Scanner:
										{
											DevicesControl.ViewModel.A4Scanner.DeviceName = DeviceType.A4Scanner.ToString();
											DevicesControl.ViewModel.A4Scanner.DeviceStatus = devStatus.ToString();
											break;
										}
									case DeviceType.CassetteStatus:
										{
											CreateCassettesGrid(GetCashCassettes(parsedMessage.Data));
											break;
										}
									default:
										break;
								}

								#endregion

								break;
							}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private void LoadCustomerDashboard()
		{
			try
			{
				if (!string.IsNullOrEmpty(WorkFlowViewModel.CustomerId))
				{
					GetCustomerCards();
					GetCustomerLoans();
					GetCustomerAccountsAndDeposits();
					GetCustomerInfo();

					btnCustomerInfo.IsEnabled = true;
				}

				InitiateWorkflow();
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		#endregion

		#region "Create Cash Cassetts"

		public void CreateCassettesGrid(ObservableCollection<Cassette> cas)
		{
			try
			{
				DevicesControl.lstCashCassettes.ItemsSource = cas;
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		#endregion

		#region "Customer Tab"

		private async void GetCustomerCards()
		{
			try
			{
				grdCustomerCardsLoader.Visibility = Visibility.Visible;
				var creditCards = await TransactionService.GetCreditCardsAsync(WorkFlowViewModel.CustomerId);
				CardViewModel = creditCards?.ConvertAll(ToCard);
				CustomerCards.lstBoxCustomerCards.ItemsSource = CardViewModel;
			}
			catch (Exception ex)
			{
				CustomerCards.lstBoxCustomerCards.ItemsSource = null;
				Logger.Writer.Exception(ex);
			}
			finally
			{
				grdCustomerCardsLoader.Visibility = Visibility.Collapsed;
			}
		}

		private static Card ToCard(CreditCardResult creditCard) => new Card
		{
			CardType = creditCard.Type,
			CardNumber = creditCard.Number,
			Currency = creditCard.Currency,
			StatementMinimumDue = creditCard.StatementMinimumDue,
			AvailableCardLimit = creditCard.CardLimit?.ToString()
		};

		private async void GetCustomerLoans()
		{
			try
			{
				grdCustomerLoansLoader.Visibility = Visibility.Visible;
				var loans = await AuthenticationService.GetAccounts(WorkFlowViewModel.CustomerId, AccountCriterion.Loan);
				LoansViewModel = loans?.ConvertAll(ToLoan);
				CustomerLoans.lstBoxCustomerLoans.ItemsSource = LoansViewModel;
			}
			catch (Exception ex)
			{
				CustomerLoans.lstBoxCustomerLoans.ItemsSource = null;
				Logger.Writer.Exception(ex);
			}
			finally
			{
				grdCustomerLoansLoader.Visibility = Visibility.Collapsed;
			}
		}

		private Loan ToLoan(Account loan) => new Loan
		{
			LoanAccountNumber = loan.Number,
			OutstandingBalance = loan.AvailableBalance?.ToString(),
			LoanType = loan.Type,
			Currency = loan.Currency
		};

		private async void GetCustomerAccountsAndDeposits()
		{
			grdCustomerAccountsAndDepositsLoader.Visibility = Visibility.Visible;

			List<Account> accounts;
			List<Account> deposits;

			try
			{
				accounts = await AuthenticationService.GetAccounts(WorkFlowViewModel.CustomerId, AccountCriterion.Casa);
			}
			catch (Exception ex)
			{
				accounts = null;
				Logger.Writer.Exception(ex);
			}
			finally
			{
				grdCustomerAccountsAndDepositsLoader.Visibility = Visibility.Collapsed;
			}

			try
			{
				deposits = await AuthenticationService.GetAccounts(WorkFlowViewModel.CustomerId, AccountCriterion.Deposit);
			}
			catch (Exception ex)
			{
				deposits = null;
				Logger.Writer.Exception(ex);
			}
			finally
			{
				grdCustomerAccountsAndDepositsLoader.Visibility = Visibility.Collapsed;
			}

			AccountsAndDepositsViewModel = (accounts?.Select(ToAccountsAndDeposits) ?? Enumerable.Empty<AccountsAndDeposits>())
				.Union(deposits?.Select(ToAccountsAndDeposits) ?? Enumerable.Empty<AccountsAndDeposits>())
				.ToList();

			CustomerAccountsAndDeposits.lstBoxAccountsAndDeposits.ItemsSource = AccountsAndDepositsViewModel;
		}

		private AccountsAndDeposits ToAccountsAndDeposits(Account account) => new AccountsAndDeposits
		{
			AccountNumber = account.Number,
			AccountBalance = account.AvailableBalance?.ToString(),
			AccountType = account.Type,
			Currency = account.Currency
		};

		private AccountsAndDeposits ToAccountsAndDeposits(DepositAccountResult deposit) => new AccountsAndDeposits
		{
			AccountNumber = deposit.Number,
			AccountBalance = deposit.Balance?.ToString(),
			AccountType = deposit.Type,
			Currency = deposit.Currency
		};

		private async void GetCustomerInfo()
		{
			grdCustomerInfoLoader.Visibility = Visibility.Visible;
			CustomerDetail customerDetails;

			try
			{
				customerDetails = await CustomerService.GetCustomerDetail(WorkFlowViewModel.CustomerId);
			}
			catch (Exception ex)
			{
				customerDetails = null;
				Logger.Writer.Exception(ex);
			}
			finally
			{
				grdCustomerInfoLoader.Visibility = Visibility.Collapsed;
			}

			FillCustomerInfo(CustomerViewModel, WorkFlowViewModel.CustomerId, customerDetails);
			CustomerInfo.DataContext = CustomerViewModel;
			accountHolder.DataContext = CustomerViewModel;
		}

		private void FillCustomerInfo(Customer customer, string customerId, CustomerDetail customerDetails)
		{
			customer.CustomerId = customerId;
			customer.Name = customerDetails?.FullName;
			customer.Nationality = customerDetails?.Nationality;
			customer.Address1 = customerDetails?.Address1;
			customer.Address2 = customerDetails?.Address2;
			customer.Country = customerDetails?.Country;
			customer.PassportNumber = customerDetails?.PassportNumber;
			customer.VisaNumber = customerDetails?.VisaRefNumber;
			customer.VisaExpiry = customerDetails?.VisaExpiryDate?.ToString("yyyy/MM/dd");
			customer.PassportExpiry = customerDetails?.PassportExpiryDate?.ToString("yyyy/MM/dd");
			customer.EmiratesId = customerDetails?.EmiratesId;
			//customer.EmiratesIdExpiry = customerDetails?.EmiratesIdExpiryDate?.ToString("dd/MM/yyyy");
			//customer.Language = customerDetails?.Language;
			//customer.Staff = customerDetails?.IsStaff == null ? "N/A" : customerDetails.IsStaff.Value ? "YES" : "NO";
			customer.RegisteredMobile = customerDetails?.MobileNumber;
			customer.RegisteredEmailAccount = customerDetails?.Email;
			customer.Salary = customerDetails?.Salary;
			//customer.RegisteredEmailCreditCard = customerDetails?.CreditCardEmail;
		}

		#endregion

		#region "Workflow"

		private void StartAuthenticationFlowWorkflow()
		{
			try
			{
				if (authenticationFlow == null)
					authenticationFlow = new AuthenticationFlow();

				workflowFrame.Navigate(authenticationFlow);
				EnableFlowUI();
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private void InitiateWorkflow()
		{
			try
			{
				if (mainWF == null)
					mainWF = new Main();

				workflowFrame.Navigate(mainWF);
				EnableFlowUI();
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		#region "Workflow Pages"

		Main mainWF = null;

		#region "Authentication Flow"

		AuthenticationFlow authenticationFlow = null;
		AuthenticationFlowDebitCreditCard authenticationFlowDebitCreditCard = null;
		AuthenticationFlowActivateOTP authenticationFlowActivateOTP = null;

		#endregion

		#region "CashDepositCreditCard"

		CashDepositCreditCard02 cashDepositCreditCard02 = null;
		CashDepositCreditCard03 cashDepositCreditCard03 = null;
		CashDepositCreditCard04 cashDepositCreditCard04 = null;

		#endregion

		#region "CreditCardPaymentWithCash"

		CreditCardPaymentWithCashSelectedCard creditCardPaymentWithCashSelectedCard = null;
		CreditCardPaymentWithCashActivateCashDeposit creditCardPaymentWithCashActivateCashDeposit = null;
		CreditCardPaymentWithCashConfirmDeposit creditCardPaymentWithCashConfirmDeposit = null;
		CreditCardPaymentWithCashPrintReceipt creditCardPaymentWithCashPrintReceipt = null;
		CreditCardPaymentWithCashExceedLimit creditCardPaymentWithCashExceedLimit = null;

		#endregion

		#region "CashDepositAccount"

		CashDepositAccountSelectedCard cashDepositAccountSelectedCard = null;
		CashDepositAccountActivateCashDeposit cashDepositAccountActivateCashDeposit = null;
		CashDepositAccountConfirmDeposit cashDepositAccountConfirmDeposit = null;
		CashDepositAccountPrintReceipt cashDepositAccountPrintReceipt = null;
		CashDepositAccountExceedLimit cashDepositAccountExceedLimit = null;

		#endregion

		#region "CashWithdrawal"

		CashWithdrawal01 cashWithdrawal01 = null;
		CashWithdrawal02 cashWithdrawal02 = null;
		CashWithdrawal03 cashWithdrawal03 = null;
		CashWithdrawal04 cashWithdrawal04 = null;
		CashWithdrawal05 cashWithdrawal05 = null;
		CashWithdrawal06 cashWithdrawal06 = null;
		CashWithdrawal07 cashWithdrawal07 = null;
		CashWithdrawal08 cashWithdrawal08 = null;
		CashWithdrawal09 cashWithdrawal09 = null;
		CashWithdrawal10 cashWithdrawal10 = null;
		CashWithdrawal11 cashWithdrawal11 = null;
        CashWithdrawal12 cashWithdrawal12 = null;

        CashWithdrawalSendAccountInformation cashWithdrawalSendAccountInfo = null;
		#endregion

		#region "CreditCardPaymentWithAccount"

		CreditCardPaymentWithAccountPayment creditCardPaymentWithAccountPayment = null;
		CreditCardPaymentWithAccountPrintReceipt creditCardPaymentWithAccountPrintReceipt = null;

		#endregion

		#region "ChequeDepositCreditCard"

		ChequeDepositCreditCardSelectedCard chequeDepositCreditCardSelectedCard = null;
		ChequeDepositCreditCardActivateChequeDeposit chequeDepositCreditCardActivateChequeDeposit = null;
		ChequeDepositCreditCardConfirmDeposit chequeDepositCreditCardConfirmDeposit = null;
		ChequeDepositCreditCardPrintReceipt chequeDepositCreditCardPrintReceipt = null;

		#endregion

		#region "ChequeDepositAccount"

		ChequeDepositAccountSelectedAccount chequeDepositAccountSelectedAccount = null;
		ChequeDepositAccountActivateChequeDeposit chequeDepositAccountActivateChequeDeposit = null;
		ChequeDepositAccountConfirmDeposit chequeDepositAccountConfirmDeposit = null;
		ChequeDepositAccountPrintReceipt chequeDepositAccountPrintReceipt = null;

		#endregion

		#region "FundsTransferOwnAccount"

		FundsTransferOwnAccountAuthentication fundsTransfer01 = null;
		FundsTransferOwnAccountLinkedAccounts fundsTransfer02 = null;
		FundsTransferOwnAccountPrintReceipt fundsTransfer03 = null;

		#endregion

		#region "FundsTransferOtherAccount"

		FundsTransferOtherAccountAuthentication fundsTransferOther01 = null;
		FundsTransferOtherAccountLinkedAccounts fundsTransferOther02 = null;
		FundsTransferOtherAccountPrintReceipt fundsTransferOther03 = null;

		#endregion

		#region "CreateBeneficiary"

		CreateBeneficiaryAuthentication createBeneficiaryAuthentication = null;
		CreateBeneficiaryLinkedAccounts createBeneficiaryLinkedAccounts = null;

		#endregion

		#region "AccountInquiry"

		AccountInquiry01 accountInquiry01 = null;
		AccountInquiry02 accountInquiry02 = null;
		AccountInquiry03 accountInquiry03 = null;
		AccountInquiry04 accountInquiry04 = null;

		#endregion

		#region "ChequePrinting"

		ChequePrinting01 chequePrinting01 = null;
		ChequePrinting02 chequePrinting02 = null;

		#endregion

		#region "NewLead"

		LeadCustomerForm newLeadCustomerForm = null;

		#endregion

		#region "AdditionalAccountOpening"

		AdditionalAccountOpeningSelectedAccount additionalAccountOpeningSelectedAccount = null;
		AdditionalAccountOpeningCaptureSignature additionalAccountOpeningCaptureSignature = null;
		AdditionalAccountOpeningInitiateOnScreenForm additionalAccountOpeningInitiateOnScreenForm = null;
		AdditionalAccountOpeningEIDScan additionalAccountOpeningEIDScan = null;
		AdditionalAccountOpeningEndSession additionalAccountOpeningEndSession = null;

		#endregion

		#region "UpdateCustomerDetails"

		UpdateCustomerDetailsEIDScan updateCustomerDetailsEIDScan;
		UpdateCustomerDetailsWithOTP updateCustomerDetailsWithOTP;

		#endregion

		#region "BillPaymentWithCreditCard"

		BillPaymentWithCreditCardPayment billPaymentWithCreditCardPayment = null;
		BillPaymentWithCreditCardPrintReceipt billPaymentWithCreditCardPrintReceipt = null;

		#endregion

		#region "BillPaymentWithAccount"

		BillPaymentWithAccountPayment billPaymentWithAccountPayment = null;
		BillPaymentWithAccountPrintReceipt billPaymentWithAccountPrintReceipt = null;

		#endregion

		#region "TransactionDeclined"

		TransactionDeclinedPrintReceipt transactionDeclinedPrintReceipt = null;

		#endregion

		#endregion

		private void ReInitializeWorkflow()
		{
			FlowType.Text = "None";

			#region "Authentication Flow"

			authenticationFlow = null;
			authenticationFlowDebitCreditCard = null;
			authenticationFlowActivateOTP = null;

			#endregion

			#region "CashDepositCreditCard"

			cashDepositCreditCard02 = null;
			cashDepositCreditCard03 = null;
			cashDepositCreditCard04 = null;

			#endregion

			#region "CashDepositAccount"

			cashDepositAccountSelectedCard = null;
			cashDepositAccountActivateCashDeposit = null;
			cashDepositAccountConfirmDeposit = null;
			cashDepositAccountPrintReceipt = null;
			cashDepositAccountExceedLimit = null;

			#endregion

			#region "CashWithdrawal"

			cashWithdrawal01 = null;
			cashWithdrawal02 = null;
			cashWithdrawal03 = null;
			cashWithdrawal04 = null;
			cashWithdrawal05 = null;
			cashWithdrawal06 = null;
			cashWithdrawal07 = null;
			cashWithdrawal08 = null;
			cashWithdrawal09 = null;
			cashWithdrawal10 = null;
			cashWithdrawal11 = null;
            cashWithdrawal12 = null;
			cashWithdrawalSendAccountInfo = null;

			#endregion

			#region "ChequeDepositCreditCard"

			chequeDepositCreditCardSelectedCard = null;
			chequeDepositCreditCardActivateChequeDeposit = null;
			chequeDepositCreditCardConfirmDeposit = null;
			chequeDepositCreditCardPrintReceipt = null;

			#endregion

			#region "ChequeDepositAccount"

			chequeDepositAccountSelectedAccount = null;
			chequeDepositAccountActivateChequeDeposit = null;
			chequeDepositAccountConfirmDeposit = null;
			chequeDepositAccountPrintReceipt = null;

			#endregion

			#region "CreditCardPaymentWithAccount"

			creditCardPaymentWithAccountPayment = null;
			creditCardPaymentWithAccountPrintReceipt = null;

			#endregion

			#region "CreditCardPaymentWithCash"

			creditCardPaymentWithCashSelectedCard = null;
			creditCardPaymentWithCashActivateCashDeposit = null;
			creditCardPaymentWithCashConfirmDeposit = null;
			creditCardPaymentWithCashPrintReceipt = null;
			creditCardPaymentWithCashExceedLimit = null;

			#endregion

			#region "FundsTransferOwnAccount"

			fundsTransfer01 = null;
			fundsTransfer02 = null;
			fundsTransfer03 = null;

			#endregion

			#region "FundsTransferOtherAccount"

			fundsTransferOther01 = null;
			fundsTransferOther02 = null;
			fundsTransferOther03 = null;

			#endregion

			#region "CreateBeneficiary"

			createBeneficiaryAuthentication = null;
			createBeneficiaryLinkedAccounts = null;

			#endregion

			#region "AccountInquiry"

			accountInquiry01 = null;
			accountInquiry02 = null;
			accountInquiry03 = null;
			accountInquiry04 = null;

			#endregion

			#region "ChequePrinting"

			chequePrinting01 = null;
			chequePrinting02 = null;

			#endregion

			#region "NewLead"

			newLeadCustomerForm = null;

			#endregion

			#region "AdditionalAccountOpening"

			additionalAccountOpeningSelectedAccount = null;
			additionalAccountOpeningCaptureSignature = null;
			additionalAccountOpeningInitiateOnScreenForm = null;
			additionalAccountOpeningEIDScan = null;
			additionalAccountOpeningEndSession = null;

			#endregion

			#region "UpdateCustomerDetails"

			updateCustomerDetailsEIDScan = null;
			updateCustomerDetailsWithOTP = null;

			#endregion

			WorkFlowViewModel = null;
			CustomerViewModel = null;
			LoansViewModel = null;
			CardViewModel = null;
			AccountsAndDepositsViewModel = null;

			WorkflowTracking.Instance().EndOfWorkflow();

			if (mainWF == null)
				mainWF = new Main();

			mainWF.EndCurrentSession();

			if (GlobalInfo.IsAuthenticated)
			{
				workflowFrame.Navigate(mainWF);
			}
			else
			{
				if (authenticationFlow == null)
					authenticationFlow = new AuthenticationFlow();
				workflowFrame.Navigate(authenticationFlow);
			}
		}

		private void workflowFrame_Click(object sender, RoutedEventArgs e)
		{
			Timer?.Start();

			try
			{
				if (count == 0)
				{
					if (e.OriginalSource is Button)
					{
						var btn = (Button)e.OriginalSource;
						if (btn == null)
							return;

						var cmdParam = btn.CommandParameter.ToString();
						var enumType = StatusEnum.ParseException;

						Enum.TryParse(cmdParam, out enumType);
						WorkflowTracking.Instance().ManageWorkflowStepTracking(enumType);

						NavigateWorkflow(enumType);
						count = 1;
					}
				}
				else
				{
					WpfMessageBox.Show("Invoked already, please don't invoke twice!");
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			count = 0;
			Timer?.Stop();
		}

		#endregion

		#region "Button Click Events"

		private void UnDoTransactionButton_Click(object sender, RoutedEventArgs e)
		{
			var mbxResult = WpfMessageBox.Show("Are you sure you want to cancel current transaction?", "Warning",
				WpfMessageBoxButton.YesNo, WpfMessageBoxImage.Warning);

			if (mbxResult == WpfMessageBoxResult.Yes)
			{
				SendStatuCodeMessage(((int)StatusEnum.EndCurrentSession).ToString());

				if (mainWF == null)
					mainWF = new Main();

				workflowFrame.Navigate(mainWF);
				ReInitializeWorkflow();
				EnableFlowUI();
			}
		}

		private void Image_MouseDown(object sender, MouseButtonEventArgs e)
		{
			var about = new About();
			about.ShowDialog();
		}

		public void btnRightGridColumn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (mainGrid.ColumnDefinitions[2].Width == new GridLength(10, GridUnitType.Pixel))
				{
					mainGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
					btnRightGridColumn.Content = Char.ConvertFromUtf32(0xE013);
					btnRightGridColumn.ToolTip = "Hide Right Panel";
				}
				else
				{
					mainGrid.ColumnDefinitions[2].Width = new GridLength(10, GridUnitType.Pixel);
					btnRightGridColumn.Content = Char.ConvertFromUtf32(0xE0E2);
					btnRightGridColumn.ToolTip = "Show Right Panel";
				}

				eSpaceMediaOcxApi.SetDesktopSharingDisplaySize(screenShare.Width.ToString(), screenShare.Height.ToString());
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private void btnLeftGridColumn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (mainGrid.ColumnDefinitions[0].Width == new GridLength(10, GridUnitType.Pixel))
				{
					mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
					btnLeftGridColumn.Content = Char.ConvertFromUtf32(0xE0E2);
					btnLeftGridColumn.ToolTip = "Hide Left Panel";
				}
				else
				{
					mainGrid.ColumnDefinitions[0].Width = new GridLength(10, GridUnitType.Pixel);
					btnLeftGridColumn.Content = Char.ConvertFromUtf32(0xE013);
					btnLeftGridColumn.ToolTip = "Show Left Panel";
				}

				eSpaceMediaOcxApi.SetDesktopSharingDisplaySize(screenShare.Width.ToString(), screenShare.Height.ToString());
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private void btnLogout_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Logout();
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private void btnRelease_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SendStatuCodeMessage(((int)StatusEnum.EndCall).ToString());
				System.Threading.Thread.Sleep(300);
				ReleaseCall();
				localVideo.Refresh();
				customerVideo.Refresh();
				screenShare.Refresh();
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		private async void btnMute_Click(object sender, RoutedEventArgs e)
		{
			await AudioSwitcherWrapper.MuteAll();
			//eSpaceMediaOcxApi.SetMute();
		}

		private async void btnUnMute_Click(object sender, RoutedEventArgs e)
		{
			await AudioSwitcherWrapper.UnMuteAll();
			//eSpaceMediaOcxApi.SetUnMute();
		}

		private void btnHoldCall_Click(object sender, RoutedEventArgs e)
		{
			if (btnHoldCall.Content.ToString() == char.ConvertFromUtf32(0xE102))
			{
				//var result = eSpaceMediaOcxApi.UnHoldCall();  // Hold call does not work and Mute does the same thing :D
				var result = eSpaceMediaOcxApi.SetUnMute();

				if (result == 0)
				{
					SendStatuCodeMessage(((int)StatusEnum.UnholdCall).ToString());
					btnHoldCall.Content = char.ConvertFromUtf32(0xE769);
				}
			}
			else
			{
				//var result = eSpaceMediaOcxApi.HoldCall();
				var result = eSpaceMediaOcxApi.SetMute();

				if (result == 0)
				{
					SendStatuCodeMessage(((int)StatusEnum.HoldCall).ToString());
					btnHoldCall.Content = char.ConvertFromUtf32(0xE102);
				}
			}
		}

		private void btnTellerState_Click(object sender, RoutedEventArgs e)
		{
			TellerState.Visibility = Visibility.Visible;
			TellerState.IsOpen = true;
		}

		private void mnuBusy_Click(object sender, RoutedEventArgs e)
		{
			var result = eSpaceMediaOcxApi.SetTellerBusy();
		}

		private void mnuIdle_Click(object sender, RoutedEventArgs e)
		{
			var result = eSpaceMediaOcxApi.SetTellerIdle();
		}

		private void mnuRest_Click(object sender, RoutedEventArgs e)
		{
			var result = eSpaceMediaOcxApi.SetTellerRest();
		}

		private void MainHead_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void DplMainHead_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		#endregion

		#region "Public Methods"

		public int NavigateWorkflow(StatusEnum enumType)
		{
			var msgRetCode = -1;

			try
			{
				switch (enumType)
				{
					case StatusEnum.Main:
						{
							if (mainWF == null)
								mainWF = new Main();

							workflowFrame.Navigate(mainWF);
							break;
						}

					#region "Authentication Flow"

					case StatusEnum.AuthenticateDebitCreditCard:
						{
							FlowType.Text = "Authentication";

							WorkFlowViewModel.Message = "Waiting for card...";

							if (authenticationFlowDebitCreditCard == null)
								authenticationFlowDebitCreditCard = new AuthenticationFlowDebitCreditCard();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AuthenticateDebitCreditCard).ToString());
							workflowFrame.Navigate(authenticationFlowDebitCreditCard);

							DisableFlowUI();
							break;
						}
					case StatusEnum.AuthenticateWithEmiratesId:
						{
							FlowType.Text = "Authentication EID";

							WorkFlowViewModel.OTPResendCount = 0;
							WorkFlowViewModel.OTPSentCount = 0;

							if (authenticationFlowActivateOTP == null)
								authenticationFlowActivateOTP = new AuthenticationFlowActivateOTP();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AuthenticateWithEmiratesId).ToString());
							workflowFrame.Navigate(authenticationFlowActivateOTP);

							DisableFlowUI();
							break;
						}
					case StatusEnum.AuthenticateActivateOTP:
						{
							WorkFlowViewModel.OTPResendCount = 0;
							WorkFlowViewModel.OTPSentCount = 0;

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AuthenticateActivateOTP).ToString());
							DisableFlowUI();

							break;
						}
					case StatusEnum.AuthenticateCif:
						{
							FlowType.Text = "Authentication CIF";

							WorkFlowViewModel.Message = "Waiting for CIF...";

							if (authenticationFlowDebitCreditCard == null)
								authenticationFlowDebitCreditCard = new AuthenticationFlowDebitCreditCard();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AuthenticateCif).ToString());
							workflowFrame.Navigate(authenticationFlowDebitCreditCard);

							DisableFlowUI();
							break;
						}

					#endregion

					#region "CashWithdrawal"

					case StatusEnum.CashWithdrawal:
						{
							FlowType.Text = "Cash Withdrawal";

							WorkFlowViewModel.Message = "Card and PIN Authenticated";
							WorkFlowViewModel.Title = "Linked Accounts";

							if (cashWithdrawal02 == null)
								cashWithdrawal02 = new CashWithdrawal02();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashWithdrawal).ToString());
							workflowFrame.Navigate(cashWithdrawal02);
							DisableFlowUI();
							break;
						}

					case StatusEnum.CashWithdrawalCancelAuthentication:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashWithdrawalCancelAuthentication).ToString());

							if (cashWithdrawal01 == null)
								cashWithdrawal01 = new CashWithdrawal01();

							workflowFrame.Navigate(cashWithdrawal01);

							break;
						}
					case StatusEnum.CashWithdrawalSendAccountInformation:
						{
							msgRetCode = SendStatuCodeMessageWithInformation($"{{MessageType: 5, StatusEnum:{((int)StatusEnum.CashWithdrawalSendAccountInformation).ToString()}, AccountHolderName:'{WorkFlowViewModel.AccountHolderName}', SelectedAccountNumber: '{WorkFlowViewModel.SelectedAccountNumber.AccountNumber}'}}");
							DisableFlowUI();

							break;
						}
					case StatusEnum.CashWithdrawalSendTransactionInformation:
						{
							if (cashWithdrawal04 == null)
								cashWithdrawal04 = new CashWithdrawal04();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashWithdrawalSendTransactionInformation).ToString());
							workflowFrame.Navigate(cashWithdrawal04);

							break;
						}
					case StatusEnum.CashWithdrawalDispenseCash:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashWithdrawalDispenseCash).ToString());
							WorkFlowViewModel.Message = "Preparing cash...";
							DisableFlowUI();

							break;
						}
					case StatusEnum.CashWithdrawalReceiptPrinting:
						{
							if (cashWithdrawal06 == null)
								cashWithdrawal06 = new CashWithdrawal06();

							msgRetCode = SendStatuCodeMessageWithInformation($"{{MessageType: 5, StatusEnum:'{((int)StatusEnum.CashWithdrawalReceiptPrinting).ToString()}', AccountHolderName:'{WorkFlowViewModel.AccountHolderName}', TransactionNumber: '{WorkFlowViewModel.TransactionNumber}'}}");
							workflowFrame.Navigate(cashWithdrawal06);
							DisableFlowUI();

							break;
						}
					case StatusEnum.CashWithdrawalUseEmiratesId:
						{
							if (cashWithdrawal07 == null)
								cashWithdrawal07 = new CashWithdrawal07();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashWithdrawalUseEmiratesId).ToString());
							WorkFlowViewModel.Message = "Waiting for account number...";
							workflowFrame.Navigate(cashWithdrawal07);
							DisableFlowUI();

							break;
						}
					case StatusEnum.CashWithdrawalUseEmiratesIdActivateOTPInput:
						{

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashWithdrawalUseEmiratesIdActivateOTPInput).ToString());
							DisableFlowUI();

							break;
						}
					case StatusEnum.CashWithdrawalUseEmiratesIdOTPConfirmed:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashWithdrawalUseEmiratesIdOTPConfirmed).ToString());

							if (cashWithdrawal08 == null)
								cashWithdrawal08 = new CashWithdrawal08();

							cashWithdrawal08.btnOTPConfirmed.IsEnabled = false;
							DisableFlowUI();

							break;
						}
					case StatusEnum.CashWithdrawalUseEmiratesIdAuthenticated:
						{
							if (cashWithdrawalSendAccountInfo == null)
								cashWithdrawalSendAccountInfo = new CashWithdrawalSendAccountInformation();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashWithdrawalUseEmiratesIdAuthenticated).ToString());
							workflowFrame.Navigate(cashWithdrawalSendAccountInfo);

							break;
						}
					case StatusEnum.CashWithdrawalUseEmiratesIdInitiateCashDispense:
						{
							if (!string.IsNullOrEmpty(WorkFlowViewModel.TransactionNumber))
							{
								msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashWithdrawalUseEmiratesIdInitiateCashDispense).ToString());
								DisableFlowUI();
							}

							break;
						}
					case StatusEnum.CashWithdrawalUseEmiratesIdPrintReceipt:
						{
							if (cashWithdrawal11 == null)
								cashWithdrawal11 = new CashWithdrawal11();

							if (!string.IsNullOrEmpty(WorkFlowViewModel.TransactionNumber))
							{
								msgRetCode = SendStatuCodeMessageWithInformation($"{{MessageType: 5, StatusEnum:'{((int)StatusEnum.CashWithdrawalUseEmiratesIdPrintReceipt).ToString()}', AccountHolderName:'{WorkFlowViewModel.AccountHolderName}', TransactionNumber: '{WorkFlowViewModel.TransactionNumber}'}}");
								cashWithdrawal11.btnPrintReceipt.Visibility = Visibility.Collapsed;

								DisableFlowUI();
							}

							break;
						}
					case StatusEnum.CashWithdrawalUseEmiratesIdSendTransactionInformation:
						{
							if (cashWithdrawal04 == null)
								cashWithdrawal04 = new CashWithdrawal04();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashWithdrawalUseEmiratesIdSendTransactionInformation).ToString());
							workflowFrame.Navigate(cashWithdrawal04);
							DisableFlowUI();
							break;
						}
                    case StatusEnum.CashWithdrawalCreditCard:
                        {
                            FlowType.Text = "Cash Withdrawal using Credit Card";

                            WorkFlowViewModel.Message = "Card and PIN Authenticated";

                            if (cashWithdrawal12 == null)
                                cashWithdrawal12 = new CashWithdrawal12();

                            msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashWithdrawalCreditCard).ToString());
                            workflowFrame.Navigate(cashWithdrawal02);
                            DisableFlowUI();
                            break;
                        }

                    #endregion

                    #region "CashDepositAccount"

                    case StatusEnum.CashDepositAccount:
						{
							FlowType.Text = "Cash Deposit Account";

							if (cashDepositAccountSelectedCard == null)
								cashDepositAccountSelectedCard = new CashDepositAccountSelectedCard();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositAccount).ToString());
							workflowFrame.Navigate(cashDepositAccountSelectedCard);

							DisableFlowUI();
							break;
						}
					case StatusEnum.CashDepositAccountActivateCashDeposit:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositAccountActivateCashDeposit).ToString());

							DisableFlowUI();
							break;
						}
					case StatusEnum.CashDepositAccountAddMore:
						{
							if (cashDepositAccountConfirmDeposit == null)
								cashDepositAccountConfirmDeposit = new CashDepositAccountConfirmDeposit();

							cashDepositAccountConfirmDeposit.btnAddMore.IsEnabled = false;
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositAccountAddMore).ToString());

							DisableFlowUI();
							break;
						}
					case StatusEnum.CashDepositAccountSourceOfFund:
						{
							if (cashDepositAccountExceedLimit == null)
								cashDepositAccountExceedLimit = new CashDepositAccountExceedLimit();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositAccountSourceOfFund).ToString());
							cashDepositAccountExceedLimit.btnSourceOfFunds.IsEnabled = false;
							cashDepositAccountExceedLimit.btnActivateEID.IsEnabled = true;

							DisableFlowUI();
							break;
						}
					case StatusEnum.CashDepositAccountActivateEIDScanner:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositAccountActivateEIDScanner).ToString());

							DisableFlowUI();
							break;
						}
					case StatusEnum.CashDepositAccountEIDConfirmed:
						{
							if (cashDepositAccountConfirmDeposit == null)
								cashDepositAccountConfirmDeposit = new CashDepositAccountConfirmDeposit();

							cashDepositAccountConfirmDeposit.btnCancelDeposit.IsEnabled = true;
							cashDepositAccountConfirmDeposit.btnConfirmDeposit.IsEnabled = true;
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositAccountEIDConfirmed).ToString());
							workflowFrame.Navigate(cashDepositAccountConfirmDeposit);

							DisableFlowUI();
							break;
						}
					case StatusEnum.CashDepositAccountConfirmCashDeposit:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositAccountConfirmCashDeposit).ToString());

							DisableFlowUI();
							break;
						}
					case StatusEnum.CashDepositAccountCancelCashDeposit:
						{
							if (cashDepositAccountPrintReceipt == null)
								cashDepositAccountPrintReceipt = new CashDepositAccountPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositAccountCancelCashDeposit).ToString());
							workflowFrame.Navigate(cashDepositAccountPrintReceipt);
							cashDepositAccountPrintReceipt.btnPrintReceipt.IsEnabled = false;

							break;
						}
					case StatusEnum.CashDepositAccountPrintReceipt:
						{
							if (cashDepositAccountPrintReceipt == null)
								cashDepositAccountPrintReceipt = new CashDepositAccountPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositAccountPrintReceipt).ToString());
							workflowFrame.Navigate(cashDepositAccountPrintReceipt);
							cashDepositAccountPrintReceipt.btnPrintReceipt.IsEnabled = false;

							DisableFlowUI();
							break;
						}

					#endregion

					#region "CashDepositCreditCard"

					case StatusEnum.CashDepositCreditCard:
						{
							if (cashDepositCreditCard03 == null)
								cashDepositCreditCard03 = new CashDepositCreditCard03();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositCreditCard).ToString());
							workflowFrame.Navigate(cashDepositCreditCard03);
							break;
						}
					case StatusEnum.CashDepositCreditCardUseCreditCard:
						{
							if (cashDepositCreditCard02 == null)
								cashDepositCreditCard02 = new CashDepositCreditCard02();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositCreditCardUseCreditCard).ToString());

							workflowFrame.Navigate(cashDepositCreditCard02);
							break;
						}
					case StatusEnum.CashDepositCreditCardAuthenticateCard:
						{
							if (cashDepositCreditCard03 == null)
								cashDepositCreditCard03 = new CashDepositCreditCard03();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositCreditCardAuthenticateCard).ToString());
							workflowFrame.Navigate(cashDepositCreditCard03);
							break;
						}
					case StatusEnum.CashDepositCreditCardActivateCashDeposit:
						{
							if (cashDepositCreditCard04 == null)
								cashDepositCreditCard04 = new CashDepositCreditCard04();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositCreditCardActivateCashDeposit).ToString());
							workflowFrame.Navigate(cashDepositCreditCard04);
							break;
						}
					case StatusEnum.CashDepositCreditCardSendAccountInformation:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CashDepositCreditCardSendAccountInformation).ToString());
							break;
						}

					#endregion

					#region "ChequeDepositAccount"

					case StatusEnum.ChequeDepositAccount:
						{
							FlowType.Text = "Cheque Encashment";

							if (chequeDepositAccountSelectedAccount == null)
								chequeDepositAccountSelectedAccount = new ChequeDepositAccountSelectedAccount();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositAccount).ToString());
							workflowFrame.Navigate(chequeDepositAccountSelectedAccount);

							DisableFlowUI();

							break;
						}
					case StatusEnum.ChequeDepositAccountActivateAmountEntry:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositAccountActivateAmountEntry).ToString());
							DisableFlowUI();

							break;
						}
					case StatusEnum.ChequeDepositAccountActivateDeposit:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositAccountActivateDeposit).ToString());

							// This enum value is NOT used, because we are using ChequeDepositCreditCardActivateDeposit both for deposit to Account and to Credit Card, but just in case we put logic to disable RT button (https://jira.network.ae/jira/browse/VTM-2510)
							// We do not disable whole UI, because End Current Session button still should be available (https://jira.network.ae/jira/browse/VTM-2061)
							if (chequeDepositAccountActivateChequeDeposit != null)
							{
								chequeDepositAccountActivateChequeDeposit.btnActivateDeposit.IsEnabled = false;
							}
							break;
						}
					case StatusEnum.ChequeDepositAccountDepositConfirmed:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositAccountDepositConfirmed).ToString());
							DisableFlowUI();

							break;
						}
					case StatusEnum.ChequeDepositAccountCancelDeposit:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositAccountCancelDeposit).ToString());
							break;
						}
					case StatusEnum.ChequeDepositAccountPrintReceipt:
						{
							WorkFlowViewModel.Message = "Receipt printing...";

							if (chequeDepositAccountPrintReceipt == null)
								chequeDepositAccountPrintReceipt = new ChequeDepositAccountPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositAccountPrintReceipt).ToString());
							chequeDepositAccountPrintReceipt.btnPrintReceipt.IsEnabled = false;
							DisableFlowUI();

							break;
						}
					case StatusEnum.ChequeDepositAccountVerifySignature:
						{
							WorkFlowViewModel.Message = "Verifying Signature...";
							VerifySignature verifySignature = new VerifySignature();
							var URL = ConfigurationManager.AppSettings["SigCapURL"];
							Logger.Writer.Info("Account Number: " + WorkFlowViewModel.CardNumber);
							verifySignature.wbSignature.Navigate(string.Format(URL, WorkFlowViewModel.CardNumber));
							verifySignature.Show();
							break;
						}
					case StatusEnum.ChequeDepositAccountVerifySecurityImage:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositAccountVerifySecurityImage).ToString());
							break;
						}

					#endregion

					#region "ChequeDepositCreditCard"

					case StatusEnum.ChequeDepositCreditCard:
						{
							if (chequeDepositCreditCardSelectedCard == null)
								chequeDepositCreditCardSelectedCard = new ChequeDepositCreditCardSelectedCard();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositCreditCard).ToString());
							workflowFrame.Navigate(chequeDepositCreditCardSelectedCard);

							// Fix https://jira.network.ae/jira/browse/VTM-2062
							//DisableFlowUI();
							break;
						}
					//case StatusEnum.ChequeDepositCreditCardActivateAmountEntry:
					//    {
					//        msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositCreditCardActivateAmountEntry).ToString());
					//        DisableFlowUI();

					//        break;
					//    }
					case StatusEnum.ChequeDepositCreditCardActivateDeposit:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositCreditCardActivateDeposit).ToString());

							// This enum value is used both for deposit to Account and to Credit Card, so we disable buttons for both workflows (https://jira.network.ae/jira/browse/VTM-2510)
							// We do not disable whole UI, because End Current Session button still should be available (https://jira.network.ae/jira/browse/VTM-2061)
							if (chequeDepositAccountActivateChequeDeposit != null)
							{
								chequeDepositAccountActivateChequeDeposit.btnActivateDeposit.IsEnabled = false;
							}
							if (chequeDepositCreditCardActivateChequeDeposit != null)
							{
								chequeDepositCreditCardActivateChequeDeposit.btnActivateDeposit.IsEnabled = false;
							}
							break;
						}
					case StatusEnum.ChequeDepositCreditCardDepositConfirmed:
						{

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositCreditCardDepositConfirmed).ToString());
							DisableFlowUI();

							break;
						}
					case StatusEnum.ChequeDepositCreditCardCancelDeposit:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositCreditCardCancelDeposit).ToString());
							DisableFlowUI();
							break;
						}
					case StatusEnum.ChequeDepositCreditCardPrintReceipt:
						{
							WorkFlowViewModel.Message = "Receipt printing...";

							if (chequeDepositCreditCardPrintReceipt == null)
								chequeDepositCreditCardPrintReceipt = new ChequeDepositCreditCardPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositCreditCardPrintReceipt).ToString());
							chequeDepositCreditCardPrintReceipt.btnPrintReceipt.IsEnabled = false;
							DisableFlowUI();

							break;
						}

					case StatusEnum.ChequeDepositEncashCheque:
						{
							chequeDepositAccountPrintReceipt.btnEncashCheque.IsEnabled = false;
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequeDepositEncashCheque).ToString());
							DisableFlowUI();
							break;
						}

					#endregion

					#region "CreditCardPaymentWithAccount"

					case StatusEnum.CreditCardPaymentWithAccount:
						{
							WorkFlowViewModel.Message = "Card and PIN Authenticated";
							WorkFlowViewModel.Title = "CARD INFORMATION";

							if (creditCardPaymentWithAccountPayment == null)
								creditCardPaymentWithAccountPayment = new CreditCardPaymentWithAccountPayment();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreditCardPaymentWithAccount).ToString());
							workflowFrame.Navigate(creditCardPaymentWithAccountPayment);
							DisableFlowUI();

							break;
						}
					case StatusEnum.CreditCardPaymentWithAccountPrintReceipt:
						{
							WorkFlowViewModel.Message = "Receipt printing...";

							if (creditCardPaymentWithAccountPrintReceipt == null)
								creditCardPaymentWithAccountPrintReceipt = new CreditCardPaymentWithAccountPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreditCardPaymentWithAccountPrintReceipt).ToString());
							creditCardPaymentWithAccountPrintReceipt.btnPrintReceipt.IsEnabled = false;
							DisableFlowUI();

							break;
						}


					#endregion

					#region "CreditCardPaymentWithCash"

					case StatusEnum.CreditCardPaymentWithCash:
						{
							FlowType.Text = "Credit Card Payment by Cash";

							if (creditCardPaymentWithCashSelectedCard == null)
								creditCardPaymentWithCashSelectedCard = new CreditCardPaymentWithCashSelectedCard();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreditCardPaymentWithCash).ToString());
							workflowFrame.Navigate(creditCardPaymentWithCashSelectedCard);

							DisableFlowUI();
							break;
						}
					case StatusEnum.CreditCardPaymentWithCashActivateCashDeposit:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreditCardPaymentWithCashActivateCashDeposit).ToString());

							DisableFlowUI();
							break;
						}
					case StatusEnum.CreditCardPaymentWithCashAddMore:
						{
							if (creditCardPaymentWithCashConfirmDeposit == null)
								creditCardPaymentWithCashConfirmDeposit = new CreditCardPaymentWithCashConfirmDeposit();

							creditCardPaymentWithCashConfirmDeposit.btnAddMore.IsEnabled = false;
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreditCardPaymentWithCashAddMore).ToString());

							DisableFlowUI();
							break;
						}
					case StatusEnum.CreditCardPaymentWithCashSourceOfFund:
						{
							if (creditCardPaymentWithCashExceedLimit == null)
								creditCardPaymentWithCashExceedLimit = new CreditCardPaymentWithCashExceedLimit();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreditCardPaymentWithCashSourceOfFund).ToString());
							creditCardPaymentWithCashExceedLimit.btnSourceOfFunds.IsEnabled = false;
							creditCardPaymentWithCashExceedLimit.btnActivateEID.IsEnabled = true;
							creditCardPaymentWithCashExceedLimit.btnCancelDeposit.IsEnabled = false;

							DisableFlowUI();
							break;
						}
					case StatusEnum.CreditCardPaymentWithCashActivateEIDScanner:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreditCardPaymentWithCashActivateEIDScanner).ToString());

							DisableFlowUI();
							break;
						}
					case StatusEnum.CreditCardPaymentWithCashEIDConfirmed:
						{
							if (creditCardPaymentWithCashConfirmDeposit == null)
								creditCardPaymentWithCashConfirmDeposit = new CreditCardPaymentWithCashConfirmDeposit();

							creditCardPaymentWithCashConfirmDeposit.btnCancelDeposit.IsEnabled = true;
							creditCardPaymentWithCashConfirmDeposit.btnConfirmDeposit.IsEnabled = true;
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreditCardPaymentWithCashEIDConfirmed).ToString());
							workflowFrame.Navigate(creditCardPaymentWithCashConfirmDeposit);

							DisableFlowUI();
							break;
						}
					case StatusEnum.CreditCardPaymentWithCashConfirmCashDeposit:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreditCardPaymentWithCashConfirmCashDeposit).ToString());

							DisableFlowUI();
							break;
						}
					case StatusEnum.CreditCardPaymentWithCashCancelCashDeposit:
						{
							if (creditCardPaymentWithCashPrintReceipt == null)
								creditCardPaymentWithCashPrintReceipt = new CreditCardPaymentWithCashPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreditCardPaymentWithCashCancelCashDeposit).ToString());
							workflowFrame.Navigate(creditCardPaymentWithCashPrintReceipt);
							creditCardPaymentWithCashPrintReceipt.btnPrintReceipt.IsEnabled = false;

							break;
						}
					case StatusEnum.CreditCardPaymentWithCashPrintReceipt:
						{
							if (creditCardPaymentWithCashPrintReceipt == null)
								creditCardPaymentWithCashPrintReceipt = new CreditCardPaymentWithCashPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreditCardPaymentWithCashPrintReceipt).ToString());
							workflowFrame.Navigate(creditCardPaymentWithCashPrintReceipt);
							creditCardPaymentWithCashPrintReceipt.btnPrintReceipt.IsEnabled = false;

							DisableFlowUI();
							break;
						}

					#endregion

					#region "FundsTransferOwnAccount"

					case StatusEnum.FundsTransferOwnAccount:
						{
							if (fundsTransfer01 == null)
								fundsTransfer01 = new FundsTransferOwnAccountAuthentication();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.FundsTransferOwnAccount).ToString());
							workflowFrame.Navigate(fundsTransfer01);
							break;
						}
					case StatusEnum.FundsTransferOwnAccountUseDebitCard:
						{
							if (fundsTransfer02 == null)
								fundsTransfer02 = new FundsTransferOwnAccountLinkedAccounts();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.FundsTransferOwnAccountUseDebitCard).ToString());
							workflowFrame.Navigate(fundsTransfer02);
							DisableFlowUI();

							break;
						}
					case StatusEnum.FundsTransferOwnAccountUseDebitCardAuthenticateCard:
						{
							if (fundsTransfer03 == null)
								fundsTransfer03 = new FundsTransferOwnAccountPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.FundsTransferOwnAccountUseDebitCardAuthenticateCard).ToString());
							workflowFrame.Navigate(fundsTransfer03);
							DisableFlowUI();

							break;
						}
					case StatusEnum.FundsTransferOwnAccountUseEID:
						{
							if (fundsTransfer03 == null)
								fundsTransfer03 = new FundsTransferOwnAccountPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.FundsTransferOwnAccountUseEID).ToString());
							workflowFrame.Navigate(fundsTransfer03);
							DisableFlowUI();
							break;
						}
					case StatusEnum.FundsTransferOwnAccountPrintReceipt:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.FundsTransferOwnAccountPrintReceipt).ToString());
							DisableFlowUI();

							break;
						}

					#endregion

					#region "FundsTransferOtherAccount"

					case StatusEnum.FundsTransferOtherAccount:
						{
							if (fundsTransferOther01 == null)
								fundsTransferOther01 = new FundsTransferOtherAccountAuthentication();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.FundsTransferOtherAccount).ToString());
							workflowFrame.Navigate(fundsTransferOther01);
							break;
						}
					case StatusEnum.FundsTransferOtherAccountUseDebitCard:
						{
							if (fundsTransferOther02 == null)
								fundsTransferOther02 = new FundsTransferOtherAccountLinkedAccounts();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.FundsTransferOtherAccountUseDebitCard).ToString());
							workflowFrame.Navigate(fundsTransferOther02);
							DisableFlowUI();

							break;
						}
					case StatusEnum.FundsTransferOtherAccountUseDebitCardAuthenticateCard:
						{
							if (fundsTransfer03 == null)
								fundsTransfer03 = new FundsTransferOwnAccountPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.FundsTransferOtherAccountUseDebitCardAuthenticateCard).ToString());
							workflowFrame.Navigate(fundsTransfer03);
							DisableFlowUI();

							break;
						}
					case StatusEnum.FundsTransferOtherAccountUseEID:
						{
							if (fundsTransfer03 == null)
								fundsTransfer03 = new FundsTransferOwnAccountPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.FundsTransferOtherAccountUseEID).ToString());
							workflowFrame.Navigate(fundsTransfer03);
							DisableFlowUI();
							break;
						}
					case StatusEnum.FundsTransferOtherAccountPrintReceipt:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.FundsTransferOtherAccountPrintReceipt).ToString());
							DisableFlowUI();

							break;
						}

					#endregion

					#region "AccountInquiry"

					case StatusEnum.AccountInquiry:
						{
							if (accountInquiry01 == null)
								accountInquiry01 = new AccountInquiry01();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AccountInquiry).ToString());
							workflowFrame.Navigate(accountInquiry01);
							break;
						}
					case StatusEnum.AccountInquiryAuthenticateCard:
						{
							FlowType.Text = "Account Inquiry";

							if (accountInquiry02 == null)
								accountInquiry02 = new AccountInquiry02();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AccountInquiryAuthenticateCard).ToString());
							workflowFrame.Navigate(accountInquiry02);
							DisableFlowUI();

							break;
						}
					case StatusEnum.AccountInquiryCancelAuthentication:
						{
							if (accountInquiry01 == null)
								accountInquiry01 = new AccountInquiry01();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AccountInquiryCancelAuthentication).ToString());
							workflowFrame.Navigate(accountInquiry01);
							DisableFlowUI();

							break;
						}
					case StatusEnum.AccountInquiryUseManualEntry:
						{
							if (accountInquiry03 == null)
								accountInquiry03 = new AccountInquiry03();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AccountInquiryUseManualEntry).ToString());
							workflowFrame.Navigate(accountInquiry03);
							DisableFlowUI();

							break;
						}
					case StatusEnum.AccountInquiryUseEmiratesIdActivateOTPInput:
						{
							if (accountInquiry04 == null)
								accountInquiry04 = new AccountInquiry04();
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AccountInquiryUseEmiratesIdActivateOTPInput).ToString());
							workflowFrame.Navigate(accountInquiry04);
							DisableFlowUI();

							break;
						}
					case StatusEnum.AccountInquiryUseEmiratesIdOTPConfirmed:
						{
							if (accountInquiry04 == null)
								accountInquiry04 = new AccountInquiry04();

							accountInquiry04.btnOTPConfirmed.IsEnabled = false;
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AccountInquiryUseEmiratesIdOTPConfirmed).ToString());
							DisableFlowUI();

							break;
						}
					case StatusEnum.AccountInquiryUseEmiratesIdAuthenticated:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AccountInquiryUseEmiratesIdAuthenticated).ToString());
							accountInquiry04.btnEmiratesIDConfirmed.IsEnabled = false;

							break;
						}

					#endregion

					#region "Documents"

					case StatusEnum.Documents:
						{
							FlowType.Text = "Certificates/Letter Requests";

							if (accountInquiry02 == null)
								accountInquiry02 = new AccountInquiry02();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.Documents).ToString());
							workflowFrame.Navigate(accountInquiry02);
							DisableFlowUI();

							break;
						}

					#endregion

					#region "ChequePrinting"

					case StatusEnum.ChequePrintingEligibility:
						{
							FlowType.Text = "Cheque Printing";

							if (chequePrinting01 == null)
								chequePrinting01 = new ChequePrinting01();

							workflowFrame.Navigate(chequePrinting01);
							break;
						}
					case StatusEnum.ChequePrintingStart:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequePrintingStart).ToString());
							DisableFlowUI();

							break;
						}
					case StatusEnum.ChequePrintingPrintingPrint:
						{
							DisableFlowUI();

							WorkFlowViewModel.Message = "PRINTING CHEQUE LEAVES";
							WorkFlowViewModel.Title = string.Empty;

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ChequePrintingPrintingPrint).ToString());

							break;
						}
					#endregion


					#region "CreateBeneficiary"

					case StatusEnum.CreateBeneficiary:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreateBeneficiary).ToString());

							break;
						}
					case StatusEnum.CreateBeneficiaryUseDebitCardAuthenticateCard:
						{
							if (createBeneficiaryAuthentication == null)
								createBeneficiaryAuthentication = new CreateBeneficiaryAuthentication();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreateBeneficiaryUseDebitCardAuthenticateCard).ToString());
							workflowFrame.Navigate(createBeneficiaryAuthentication);
							DisableFlowUI();

							break;
						}
					case StatusEnum.CreateBeneficiaryAddNewTransaction:
						{
							if (createBeneficiaryLinkedAccounts == null)
								createBeneficiaryLinkedAccounts = new CreateBeneficiaryLinkedAccounts();

							createBeneficiaryLinkedAccounts.btnAddNewTransaction.IsEnabled = false;

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreateBeneficiaryAddNewTransaction).ToString());
							DisableFlowUI();

							break;
						}

					#endregion

					#region "Leads"

					case StatusEnum.AccountLeadStarting:
						{
							WorkFlowViewModel.Message = "Opening chosen form...";
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AccountLeadStarting).ToString());
							DisableFlowUI();
							break;
						}
					case StatusEnum.LoanLeadStarting:
						{
							WorkFlowViewModel.Message = "Opening chosen form...";
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.LoanLeadStarting).ToString());
							DisableFlowUI();
							break;
						}
					case StatusEnum.CreditCardLeadStarting:
						{
							WorkFlowViewModel.Message = "Opening chosen form...";
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.CreditCardLeadStarting).ToString());
							DisableFlowUI();
							break;
						}
					case StatusEnum.WealthLeadStarting:
						{
							WorkFlowViewModel.Message = "Opening chosen form...";
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.WealthLeadStarting).ToString());
							DisableFlowUI();
							break;
						}

					#endregion

					#region "UpdateCustomerDetails"

					case StatusEnum.UpdateCustomerDetailsStarting:
						{
							WorkFlowViewModel.OTPResendCount = 0;
							WorkFlowViewModel.OTPSentCount = 0;

							if (updateCustomerDetailsWithOTP == null)
								updateCustomerDetailsWithOTP = new UpdateCustomerDetailsWithOTP();
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.UpdateCustomerDetailsStarting).ToString());
							workflowFrame.Navigate(updateCustomerDetailsWithOTP);

							DisableFlowUI();
							break;
						}
					case StatusEnum.UpdateCustomerDetailsScanEmiratesIdActivate:
						{
							msgRetCode = SendStatuCodeMessage(StatusEnum.UpdateCustomerDetailsScanEmiratesIdActivate);
							DisableFlowUI();
							break;
						}
					case StatusEnum.UpdateCustomerDetailsScanEmiratesIdVerified:
						{
							msgRetCode = SendStatuCodeMessage(StatusEnum.UpdateCustomerDetailsScanEmiratesIdVerified);
							DisableFlowUI();
							break;
						}

					#endregion

					#region "AdditionalAccountOpening"

					case StatusEnum.AdditionalAccountOpeningRequestByRT:
						{
							WorkFlowViewModel.Message = "Card and PIN Authenticated";

							if (additionalAccountOpeningSelectedAccount == null)
								additionalAccountOpeningSelectedAccount = new AdditionalAccountOpeningSelectedAccount();
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AdditionalAccountOpeningRequestByRT).ToString());
							workflowFrame.Navigate(additionalAccountOpeningSelectedAccount);

							DisableFlowUI();

							break;
						}
					case StatusEnum.AdditionalAccountOpeningRequestByRTCaptureSignature1:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AdditionalAccountOpeningRequestByRTCaptureSignature1).ToString());
							DisableFlowUI();

							break;
						}
					case StatusEnum.AdditionalAccountOpeningRequestByRTCaptureSignature2:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AdditionalAccountOpeningRequestByRTCaptureSignature2).ToString());
							DisableFlowUI();

							break;
						}
					case StatusEnum.AdditionalAccountOpeningRequestByRTSignaturesVerified:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.AdditionalAccountOpeningRequestByRTSignaturesVerified).ToString());
							DisableFlowUI();

							break;
						}
					case StatusEnum.AdditionalAccountOpeningRequestByRTFormVerifiedByRT:
						{
							msgRetCode = SendStatuCodeMessageWithInformation($"{{MessageType: {(int)MessageType.Command}, StatusEnum:'{((int)StatusEnum.AdditionalAccountOpeningRequestByRTFormVerifiedByRT)}', VerifiedById:'{GlobalInfo.TellerId}', VerifiedByName:'{GlobalInfo.TellerName}'}}");
							DisableFlowUI();

							break;
						}
					case StatusEnum.ScanEmiratesIdStepActivateScanner:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ScanEmiratesIdStepActivateScanner).ToString());
							DisableFlowUI();

							break;
						}
					case StatusEnum.ScanEmiratesIdStepEmiratesIdVerified:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.ScanEmiratesIdStepEmiratesIdVerified).ToString());
							DisableFlowUI();

							break;
						}
					#endregion

					#region "BillPaymentWithCreditCard"

					case StatusEnum.BillPaymentWithCreditCard:
						{
							WorkFlowViewModel.Message = "Card and PIN Authenticated";
							WorkFlowViewModel.Title = "CARD INFORMATION";

							if (billPaymentWithCreditCardPayment == null)
								billPaymentWithCreditCardPayment = new BillPaymentWithCreditCardPayment();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.BillPaymentWithCreditCard).ToString());
							workflowFrame.Navigate(billPaymentWithCreditCardPayment);
							DisableFlowUI();

							break;
						}
					case StatusEnum.BillPaymentWithCreditCardPrintReceipt:
						{
							WorkFlowViewModel.Message = "Receipt printing...";

							if (billPaymentWithCreditCardPrintReceipt == null)
								billPaymentWithCreditCardPrintReceipt = new BillPaymentWithCreditCardPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.BillPaymentWithCreditCardPrintReceipt).ToString());
							billPaymentWithCreditCardPrintReceipt.btnPrintReceipt.IsEnabled = false;
							DisableFlowUI();

							break;
						}

					#endregion

					#region "BillPaymentWithAccount"

					case StatusEnum.BillPaymentWithAccount:
						{
							WorkFlowViewModel.Message = "Card and PIN Authenticated";
							WorkFlowViewModel.Title = "CARD INFORMATION";

							if (billPaymentWithAccountPayment == null)
								billPaymentWithAccountPayment = new BillPaymentWithAccountPayment();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.BillPaymentWithAccount).ToString());
							workflowFrame.Navigate(billPaymentWithAccountPayment);
							DisableFlowUI();

							break;
						}
					case StatusEnum.BillPaymentWithAccountPrintReceipt:
						{
							WorkFlowViewModel.Message = "Receipt printing...";

							if (billPaymentWithAccountPrintReceipt == null)
								billPaymentWithAccountPrintReceipt = new BillPaymentWithAccountPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.BillPaymentWithAccountPrintReceipt).ToString());
							billPaymentWithAccountPrintReceipt.btnPrintReceipt.IsEnabled = false;
							DisableFlowUI();

							break;
						}

					#endregion

					#region "TransactionDeclined"

					case StatusEnum.PrintTransactionFailureReceipt:
						{
							WorkFlowViewModel.Message = "Receipt printing...";

							if (transactionDeclinedPrintReceipt == null)
								transactionDeclinedPrintReceipt = new TransactionDeclinedPrintReceipt();

							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.PrintTransactionFailureReceipt).ToString());
							transactionDeclinedPrintReceipt.btnPrintReceipt.IsEnabled = false;
							DisableFlowUI();

							break;
						}

					#endregion

					#region "End"

					case StatusEnum.Back:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.Back).ToString());
							ReInitializeWorkflow();
							EnableFlowUI();
							break;
						}
					case StatusEnum.EndCurrentSession:
						{
							msgRetCode = SendStatuCodeMessage(((int)StatusEnum.EndCurrentSession).ToString());
							ReInitializeWorkflow();
							EnableFlowUI();

							break;
						}
					default:
						{
							break;
						}

						#endregion
				}
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}

			return msgRetCode;
		}

		public void EnableUI()
		{
			workflowFrame.IsEnabled = true;
			txtRecording.Visibility = Visibility.Visible;
			CallButtons.IsEnabled = true;
		}

		public void DisableUI()
		{
			workflowFrame.IsEnabled = false;
			txtRecording.Visibility = Visibility.Collapsed;
			CallButtons.IsEnabled = false;
		}

		public void ReleaseCall()
		{
			eSpaceMediaOcxApi.ReleaseCall();
			accountHolder.DataContext = null;
			btnHoldCall.Content = char.ConvertFromUtf32(0xE769);
			localVideo.Refresh();
			customerVideo.Refresh();
			screenShare.Refresh();
		}

		public void AnswerIncomingCall()
		{
			eSpaceMediaOcxApi.AnswerCall();

			btnTellerState.IsEnabled = false;
			CallTimeObserver.CallTimerEvent += CallTimeObserver_CallTimerEvent;
			CallTimeObserver.ObserverTimer.Start();
			CallTimeObserver.StartTime = DateTime.Now;

			//remoteVid.Show();
			btnRelease.Background = System.Windows.Media.Brushes.Red;
		}

		private void CallTimeObserver_CallTimerEvent(object sender, EventArgs e)
		{
			var send = sender as CallTimeObserver;

			if (send != null)
			{
				DevicesControl.ViewModel.CallDuration.Duration = DateTime.Now - send.StartTime; ;
			}
		}

		#endregion

		#region "Private Methods"

		private StatusEnum ParseMessageCode(string msg)
		{
			var enumType = StatusEnum.ParseException;

			try
			{
				Enum.TryParse(msg, out enumType);
			}
			catch (Exception)
			{

			}

			return enumType;
		}

		private short SendStatuCodeMessage(StatusEnum statusCode)
		{
			return SendStatuCodeMessage(statusCode.ToString());
		}

		private short SendStatuCodeMessage(string statusCode)
		{
			short msg = -1;

			try
			{
				var jsonObject = new JObject();
				jsonObject.Add(nameof(MessageType), ((int)MessageType.Command).ToString());
				jsonObject.Add(nameof(StatusEnum), statusCode);

				msg = eSpaceMediaOcxApi.SendMessage(jsonObject.ToString());
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}

			return msg;
		}

		private short SendStatuCodeMessageWithInformation(string message)
		{
			short msg = -1;

			try
			{
				msg = eSpaceMediaOcxApi.SendMessage(message);
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}

			return msg;
		}

		private short Logout()
		{
			short ret = -1;
			var state = eSpaceMediaOcxApi.GetTellerStatus(GlobalInfo.TellerId);

			switch (state)
			{
				case VTA.TellerState.Unknown:
					{
						ReleaseCall();
						DevicesControl.StopNetworObserver();
						Logger.Writer.Info(GetType() + " Logout() - Unknown.");
						Application.Current.Shutdown();
						break;
					}
				case VTA.TellerState.Talking:
					{
						if (WpfMessageBox.Show("Are you sure, you want to logout in an active call ?", "Confirmation", WpfMessageBoxButton.YesNo) == WpfMessageBoxResult.Yes)
						{
							ReleaseCall();
							DevicesControl.StopNetworObserver();
							Dispatcher.Invoke(DispatcherPriority.Send, (Action)(() =>
							{
								_shutDownOnLogOut = true;
								ret = eSpaceMediaOcxApi.Logout();
							}));

							Logger.Writer.Info(GetType() + " Logout() - Talking.");
						}

						break;
					}
				default:
					{
						ReleaseCall();
						DevicesControl.StopNetworObserver();

						Dispatcher.Invoke(DispatcherPriority.Send, (Action)(() =>
						{
							_shutDownOnLogOut = true;
							ret = eSpaceMediaOcxApi.Logout();
						}));

						Logger.Writer.Info(GetType() + " Logout() - default.");
						break;
					}
			}

			return ret;
		}

		private void EnableFlowUI()
		{
			grdFlowLoader.Visibility = Visibility.Collapsed;
		}

		private void DisableFlowUI()
		{
			grdFlowLoader.Visibility = Visibility.Visible;
		}

		private ObservableCollection<Core.Model.LinkedAccount> GetLinkedAccounts(string data)
		{
			WorkFlowViewModel.LinkedAccounts.Clear();
			var accounts = eSpaceMediaOcx.FindValueFromJson(data, "LinkedAccount");
			var accountsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(accounts);

			foreach (var item in accountsDictionary)
			{
				WorkFlowViewModel.LinkedAccounts.Add(new Core.Model.LinkedAccount { AccountType = item.Key, AccountNumber = item.Value });
			}

			return WorkFlowViewModel.LinkedAccounts;
		}

		private ObservableCollection<Core.Model.LinkedAccount> GetFundsTransferAccounts(string data)
		{
			WorkFlowViewModel.LinkedAccounts.Clear();
			var accounts = eSpaceMediaOcx.FindValueFromJson(data, "FundsTransferAccounts");
			var accountsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(accounts);

			foreach (var item in accountsDictionary)
			{
				WorkFlowViewModel.LinkedAccounts.Add(new Core.Model.LinkedAccount { AccountType = item.Key, AccountNumber = item.Value });
			}

			return WorkFlowViewModel.LinkedAccounts;
		}

		public Core.Model.LinkedAccount GetSelectedAccount(string data)
		{
			var accounts = eSpaceMediaOcx.FindValueFromJson(data, "SelectedAccountNumber");
			var accountsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(accounts);

			foreach (var item in accountsDictionary)
			{
				WorkFlowViewModel.SelectedAccountNumber.AccountType = item.Key;
				WorkFlowViewModel.SelectedAccountNumber.AccountNumber = item.Value;
			}

			return WorkFlowViewModel.SelectedAccountNumber;
		}

		private ObservableCollection<Cassette> GetCashCassettes(string data)
		{
			DevicesControl.ViewModel.CashCassettes.Clear();
			var accounts = eSpaceMediaOcx.FindValueFromJson(data, "CashCassettes");
			var accountsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(accounts);

			foreach (var item in accountsDictionary)
			{
				var cas = new Cassette();
				cas.CurrenceyNoteName = item.Key;

				if (!string.IsNullOrEmpty(item.Value))
				{
					var notePercentage = 0.0;
					double.TryParse(item.Value, out notePercentage);
					cas.CurrencyNotePercentage = (notePercentage / 2800 * 10).ToString();
					cas.CurrencyNoteQuantity = item.Value;
					int CasKey = 0;
					int.TryParse(item.Key, out CasKey);

					if (CasKey > 0)
						cas.CurrenceyAmount = (CasKey * int.Parse(item.Value)).ToString();
				}
				else
				{
					cas.CurrencyNotePercentage = "0.0";
					cas.CurrencyNoteQuantity = "0";
				}

				DevicesControl.ViewModel.CashCassettes.Add(cas);
			}

			return DevicesControl.ViewModel.CashCassettes;
		}

		#endregion

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var mbxResult = WpfMessageBox.Show("Are you sure you want to cancel current Session?", "Warning",
				WpfMessageBoxButton.YesNo, WpfMessageBoxImage.Warning);

			if (mbxResult == WpfMessageBoxResult.Yes)
			{
				GlobalInfo.IsAuthenticated = false;

				EnableFlowUI();
				workflowFrame.IsEnabled = CallButtons.IsEnabled;

				InitiateWorkflow();
				StartAuthenticationFlowWorkflow();

				if (CallButtons.IsEnabled)
				{
					SendStatuCodeMessage(((int)StatusEnum.ResetCall).ToString());
				}
			}
		}

		private void btnDashboard_Click(object sender, RoutedEventArgs e)
		{
			pnlDashboard.Visibility = Visibility.Visible;
			btnDashboard.IsChecked = true;

			btnCustomerInfo.IsChecked = false;
			pnlCustomerInfo.Visibility = Visibility.Collapsed;
		}

		private void btnCustomerInfo_Click(object sender, RoutedEventArgs e)
		{
			pnlDashboard.Visibility = Visibility.Collapsed;
			btnCustomerInfo.IsChecked = true;

			btnDashboard.IsChecked = false;
			pnlCustomerInfo.Visibility = Visibility.Visible;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Logger.Writer.Info(txtTempCode.Text);
			SendStatuCodeMessage((int.Parse(txtTempCode.Text)).ToString());
		}
	}
}