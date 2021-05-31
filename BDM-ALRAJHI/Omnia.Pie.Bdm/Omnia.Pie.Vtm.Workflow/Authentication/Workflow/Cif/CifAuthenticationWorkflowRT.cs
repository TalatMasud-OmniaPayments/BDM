namespace Omnia.Pie.Vtm.Workflow.Authentication.Cif
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Communication.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.Authentication.Context;
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;

	public class CifAuthenticationWorkflowRT : Workflow
	{
		private readonly IPinPad _pinPad;
		private readonly IESpaceTerminalCommunication _communicator;

		private readonly EnterCifStep _enterCifStep;
		private readonly ValidateCustomerStep _validateCustomerStep;
		private readonly SendSmsOtpStep _sendSmsOtpStep;
		private readonly EnterOtpStep _enterOtpStep;
		private readonly ValidateOtpStep _validateOtpStep;
		private readonly GetCardsStep _getCardsStep;
		private readonly SelectCardStep _selectCardStep;
		private readonly SendSelectedCardStep _sendSelectedCardStep;
		private readonly EnterPinStep _enterPinStep;
		private readonly ValidatePinEidNdcStep _validatePinEidNdcStep;

		public CifAuthenticationWorkflowRT(IResolver container) : base(container)
		{
			_pinPad = _container.Resolve<IPinPad>();
			_communicator = _container.Resolve<IESpaceTerminalCommunication>();

			_enterCifStep = _container.Resolve<EnterCifStep>();
			_validateCustomerStep = _container.Resolve<ValidateCustomerStep>();
			_sendSmsOtpStep = _container.Resolve<SendSmsOtpStep>();
			_enterOtpStep = _container.Resolve<EnterOtpStep>();
			_validateOtpStep = _container.Resolve<ValidateOtpStep>();
			_getCardsStep = _container.Resolve<GetCardsStep>();
			_selectCardStep = _container.Resolve<SelectCardStep>();
			_sendSelectedCardStep = _container.Resolve<SendSelectedCardStep>();
			_enterPinStep = _container.Resolve<EnterPinStep>();
			_validatePinEidNdcStep = _container.Resolve<ValidatePinEidNdcStep>();

			Context = _enterCifStep.Context = _validateCustomerStep.Context = _sendSmsOtpStep.Context =
				_enterOtpStep.Context = _validateOtpStep.Context = _getCardsStep.Context =
				_selectCardStep.Context = _sendSelectedCardStep.Context = _enterPinStep.Context =
				_validatePinEidNdcStep.Context = CreateContext(typeof(AuthDataContext));

			AddSteps($"{nameof(EnterCifStep)},{nameof(ValidateCustomerStep)}, {nameof(SendSmsOtpStep)},{nameof(EnterOtpStep)},{nameof(ValidateOtpStep)}");

			_enterPinStep.CancelAction = async () =>
			{
				LoadWaitScreen();
				await _pinPad.StopPinReading();

				_journal.TransactionCanceled();
				_communicator.SendStatus(StatusEnum.EndCurrentSession);
				LoadStandByRT();
			};

			_enterCifStep.CancelAction = _selectCardStep.CancelAction = _enterOtpStep.CancelAction = () =>
			 {
				 _journal.TransactionCanceled();
				 _communicator.SendStatus(StatusEnum.EndCurrentSession);
				 LoadStandByRT();
			 };

			_communicator.CurrentSessionEnded += _communicator_CurrentSessionEnded;
			_communicator.CallEnded += _communicator_CallEnded;
		}

		private void _communicator_CallEnded(object sender, CallEventArgs e)
		{
			_communicator.SendStatus(StatusEnum.EndCurrentSession);
			LoadStandByRT();
		}

		private void _communicator_CurrentSessionEnded(object sender, EventArgs e)
		{
			_communicator.SendStatus(StatusEnum.EndCurrentSession);
			LoadStandByRT();
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Workflow: CIF Authentication - RT Assisted");

            try
			{
				await _enterCifStep.ExecuteAsync();
				LoadWaitScreen();
				await _validateCustomerStep.ExecuteAsync(); // Validate Cif & Get EID 

				var attempts = 3;
				int.TryParse(SystemParametersConfiguration.GetElementValue("MaxOTPAttemptLimit"), out attempts);

				for (int i = 0; i < attempts; i++)
				{
					await _sendSmsOtpStep.ExecuteAsync();
					//_communicator.SendStatus(StatusEnum.AuthenticateWaitingForOtp);
					await _enterOtpStep.ExecuteAsync();
					//_communicator.SendStatus(StatusEnum.AuthenticateValidatingOtp);

					try
					{
						await _validateOtpStep.ExecuteAsync();
					}
					catch (InvalidOtpException)
					{
						await LoadErrorScreenAsync(ErrorType.InvalidOtp, () => { }, false);
					}

					if (Context.Get<IAuthDataContext>().OtpMatched)
						break;
				}

				if (Context.Get<IAuthDataContext>().OtpMatched)
				{
					await _getCardsStep.ExecuteAsync();
					await _selectCardStep.ExecuteAsync();
					await _sendSelectedCardStep.ExecuteAsync();

					for (int i = 0; i < attempts; i++)
					{
						await _pinPad.StartPinReading();
						await _enterPinStep.ExecuteAsync();
						LoadWaitScreen();

						Context.Get<IAuthDataContext>().Pin = await _pinPad.BuildPinBlockDieboldAsync(Context.Get<IAuthDataContext>().EIdNumber);
						//Context.Get<IAuthDataContext>().Pin = "<=:11>0402;=:155";
						await _pinPad.StopPinReading();

						try
						{
							await _validatePinEidNdcStep.ExecuteAsync();
						}
						catch (InvalidPinException ex)
						{
							_logger.Exception(ex);
							_journal.AuthenticationFailed();
							await LoadErrorScreenAsync(ErrorType.InvalidPin, () => { });
						}

						if (Context.Get<IAuthDataContext>().Authenticated)
							break;
					}

					if (Context.Get<IAuthDataContext>().Authenticated && !string.IsNullOrEmpty(Context?.Get<IAuthDataContext>()?.CustomerId))
					{
						_journal.AuthenticationSucceeded();

						_communicator.SendStatus(StatusEnum.AuthenticateTerminalId,
												new { TerminalId = TerminalConfiguration.Section.Id });
						_communicator.SendStatus(StatusEnum.AuthenticateBranchId,
											new { BranchId = TerminalConfiguration.Section.Id });

						_communicator.SendStatus(StatusEnum.AuthenticateEmiratesIdAuthenticated,
											new { CustomerId = Context.Get<IAuthDataContext>().CustomerId });

						LoadStandByRT();
					}
					else
					{
						_journal.AuthenticationFailed();
						_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);

						await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, () =>
						{
							_communicator.SendStatus(StatusEnum.EndCurrentSession);
							LoadStandByRT();
						});
					}
				}
				else
				{
					_journal.AuthenticationFailed();
					_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);

					await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, () =>
					{
						_communicator.SendStatus(StatusEnum.EndCurrentSession);
						LoadStandByRT();
					});
				}
			}
			catch (HttpRequestException ex)
			{
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, () =>
				{
					_communicator.SendStatus(StatusEnum.EndCurrentSession);
					LoadStandByRT();
				});
			}
			catch (EIDCardsNotFoundException ex)
			{
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.CardsNotLinked, () =>
				{
					_communicator.SendStatus(StatusEnum.EndCurrentSession);
					LoadStandByRT();
				});
			}
			catch (ServiceException ex)
			{
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, () =>
				{
					_communicator.SendStatus(StatusEnum.EndCurrentSession);
					LoadStandByRT();
				});
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, () =>
				{
					_communicator.SendStatus(StatusEnum.EndCurrentSession);
					LoadStandByRT();
				});
			}
			finally
			{
				DisposeSteps();
			}
		}

		public override void Dispose()
		{

		}
	}
}