namespace Omnia.Pie.Vtm.Workflow.Authentication.Cif
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.Framework.Extensions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow.Authentication.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using System;
	using System.Linq;
	using System.Net.Http;
	using System.Threading.Tasks;

	public class CifAuthenticationWorkflow : Workflow
	{
		private readonly IPinPad _pinPad;

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

		public CifAuthenticationWorkflow(IResolver container) : base(container)
		{
			_pinPad = _container.Resolve<IPinPad>();

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

			AddSteps($"{Properties.Resources.StepEnterCif},{Properties.Resources.StepSendSMSOTP},{Properties.Resources.StepEnterOtp},{Properties.Resources.StepCardSelection},{Properties.Resources.StepEnterPin},{Properties.Resources.StepPinValidation}");

			_enterPinStep.CancelAction = async () =>
			{
				LoadWaitScreen();
				await _pinPad.StopPinReading();

				_journal.TransactionCanceled();
				LoadMainScreen();
			};
			_enterCifStep.CancelAction = _selectCardStep.CancelAction = _enterOtpStep.CancelAction = () =>
			{
				_journal.TransactionCanceled();
				LoadMainScreen();
			};
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Workflow: CIF Authentication - Self Service");

            try
			{
				_container.Resolve<ISessionContext>().CifAuth = true;

				await _enterCifStep.ExecuteAsync();
				LoadWaitScreen();
				await _validateCustomerStep.ExecuteAsync(); // Validate Cif & Get EID 

				var attempts = 3;
				int.TryParse(SystemParametersConfiguration.GetElementValue("MaxOTPAttemptLimit"), out attempts);

				for (int i = 0; i < attempts; i++)
				{
					await _sendSmsOtpStep.ExecuteAsync();
					await _enterOtpStep.ExecuteAsync();

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
						await CheckNdcCardType();
						LoadSelfServiceMenu();
					}
					else
					{
						_journal.AuthenticationFailed();
						await LoadErrorScreenAsync(ErrorType.AuthenticationFailed, () =>
						{
							LoadMainScreen();
						});
					}
				}
				else
				{
					_journal.AuthenticationFailed();
					await LoadErrorScreenAsync(ErrorType.AuthenticationFailed, () =>
					{
						LoadMainScreen();
					});
				}
			}
			catch (HttpRequestException ex)
			{
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, () =>
				{
					LoadMainScreen();
				});
			}
			catch (EIDCardsNotFoundException ex)
			{
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.CardsNotLinked, () =>
				{
					LoadMainScreen();
				});
			}
			catch (ServiceException ex)
			{
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, () =>
				{
					LoadMainScreen();
				});
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, () =>
				{
					LoadMainScreen();
				});
			}
			finally
			{
				DisposeSteps();
			}
		}

		private async Task CheckNdcCardType()
		{
            _logger?.Info($"Execute Task: Check Card Type");

            var TransactionService = _container.Resolve<Services.Interface.ITransactionService>();
			var creditCards = await TransactionService.GetCreditCardsAsync(_container.Resolve<ISessionContext>().CustomerIdentifier);
			if (creditCards != null)
			{
				var cr = creditCards.Where(x => x?.Number?.GetLastCharacters(4) == Context.Get<IAuthDataContext>()?.SelectedCard?.CardNumber?.GetLastCharacters(4))?.FirstOrDefault();
				if (cr != null)
				{
					if (_container.Resolve<ISessionContext>().CardUsed == null)
						_container.Resolve<ISessionContext>().CardUsed = new Devices.Interface.Entities.Card();

					_container.Resolve<ISessionContext>().CardUsed.CardType = CardType.CreditCard;
					_container.Resolve<ISessionContext>().CardUsed.CardNumber = cr.Number;
				}
			}
		}

		public override void Dispose()
		{

		}
	}
}