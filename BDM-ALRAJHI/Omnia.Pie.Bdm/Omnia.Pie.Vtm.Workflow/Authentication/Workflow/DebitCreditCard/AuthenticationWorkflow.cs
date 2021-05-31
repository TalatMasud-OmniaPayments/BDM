namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.Framework.Extensions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow;
	using Omnia.Pie.Vtm.Workflow.Authentication.Cif;
	using Omnia.Pie.Vtm.Workflow.Authentication.Context;
	using Omnia.Pie.Vtm.Workflow.Common;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
    using Omnia.Pie.Vtm.Workflow.Common.Steps;
    using System;
    using System.Configuration;
    using System.Linq;
	using System.Net.Http;
	using System.Net.Sockets;
	using System.Threading.Tasks;
	using System.Web.Script.Serialization;

	internal class AuthenticationWorkflow : Workflow
	{
		private readonly ICardReader _cardReader;
		private readonly IPinPad _pinPad;

		private readonly ReadCardStep _readCardStep;
		private readonly EnterPinStep _enterPinStep;
		private readonly ValidatePinNdcStep _validatePinStep;
		private readonly ValidatePinEidNdcStep _validatePinEidNdcStep;
		private readonly ReturnCardStep _returnCardStep;
		private readonly GetCardsStep _getCardsStep;
		private readonly SelectCardStep _selectCardStep;
		private readonly SendSelectedCardStep _sendSelectedCardStep;
        private readonly CheckPinpadStep _checkPinpadStep;
        private readonly CheckCardReaderStep _checkCardReaderStep;

        public AuthenticationWorkflow(IResolver container) : base(container)
		{
			_cardReader = _container.Resolve<ICardReader>();
			_pinPad = _container.Resolve<IPinPad>();

			_readCardStep = _container.Resolve<ReadCardStep>();
			_enterPinStep = _container.Resolve<EnterPinStep>();
			_validatePinStep = _container.Resolve<ValidatePinNdcStep>();
			_validatePinEidNdcStep = _container.Resolve<ValidatePinEidNdcStep>();
			_returnCardStep = _container.Resolve<ReturnCardStep>();
			_getCardsStep = _container.Resolve<GetCardsStep>();
			_selectCardStep = _container.Resolve<SelectCardStep>();
			_sendSelectedCardStep = _container.Resolve<SendSelectedCardStep>();
            _checkPinpadStep = _container.Resolve<CheckPinpadStep>();
            _checkCardReaderStep = _container.Resolve<CheckCardReaderStep>();

            Context = _enterPinStep.Context = _validatePinStep.Context = _getCardsStep.Context =
				_validatePinEidNdcStep.Context = _readCardStep.Context = _selectCardStep.Context =
				_sendSelectedCardStep.Context = CreateContext(typeof(AuthDataContext));

			AddSteps($"{Properties.Resources.StepReadCard},{Properties.Resources.StepEnterPin},{Properties.Resources.StepPinValidation}");

			_enterPinStep.CancelAction = async () =>
			{
                //_logger?.Info($"Entering Cancel Action...Loading wait screen...");
                LoadWaitScreen();

                //_logger?.Info($"Stopping PIN Reading");
                await _pinPad.StopPinReading();
                //_logger?.Info($"Stopping PIN Reading....Done!");
                //_logger?.Info($"Writing to journal printer for transaction cancel");
                _journal.TransactionCanceled();
                //_logger?.Info($"Return card step");
                await _returnCardStep.ReturnCard();
				LoadMainScreen();
			};
			_selectCardStep.CancelAction = async () =>
			{
				_journal.TransactionCanceled();
				await _returnCardStep.ReturnCard();
				LoadMainScreen();
			};

			_navigator.RequestNavigationTo<IBankingServicesViewModel>((viewModel) =>
			{
				viewModel.CancelVisibility = true;
				viewModel.BackAction = viewModel.CancelAction = async () =>
				{
					_cardReader.CancelReadCard();
					await _returnCardStep.ReturnCard();
					LoadMainScreen();
				};
				viewModel.DefaultAction = async () =>
				{
					Context.Get<IAuthDataContext>().CifMod = true;
					_cardReader.CancelReadCard();
					await _returnCardStep.ReturnCard();

					using (var flow = _container.Resolve<CifAuthenticationWorkflow>())
					{
						flow.Execute();
					}
				};
			});

			Context.Get<IAuthDataContext>().SelfServiceMode = true;
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Workflow: Card Authentication - Self Service");

            try
			{

                _logger?.Info($"Checking Pinpad Availability");
                await _checkPinpadStep.CheckPinpadAsync();

                _logger?.Info($"Checking CardReader Availability");
                await _checkCardReaderStep.CheckCardReaderAsync();

                await _readCardStep.ReadCardAsync();

				var attempts = 3;
				int.TryParse(SystemParametersConfiguration.GetElementValue("MaxPinAttemptLimit"), out attempts);

				if (Context.Get<IAuthDataContext>()?.Card != null && !Context.Get<IAuthDataContext>().CifMod)
				{
                    _logger?.Info($"Card Type: {Context.Get<IAuthDataContext>()?.Card?.CardType}");

                    switch (Context.Get<IAuthDataContext>()?.Card?.CardType)
					{
						case CardType.CreditCard:
						case CardType.DebitCard:
							{
								for (int i = 0; i < attempts; i++)
								{
									await _pinPad.StartPinReading();
									bool result = await _enterPinStep.ExecuteAsync();
                                    _logger?.Info($"Checking PINREAD Result {result}");

                                    LoadWaitScreen();
									var ct = _container.Resolve<ISessionContext>();
									Context.Get<IAuthDataContext>().Pin = await _pinPad.BuildPinBlockDieboldAsync(ct.CardUsed);

									await _pinPad.StopPinReading();

									try
									{
										await _validatePinStep.ExecuteAsync();
									}
									catch (InvalidPinException ex)
									{
										_logger.Exception(ex);
										_journal.AuthenticationFailed();
										await LoadErrorScreenAsync(ErrorType.InvalidPin, () => { }, false);
									}

									if (Context.Get<IAuthDataContext>().Authenticated)
										break;
								}

								break;
							}
						case CardType.EmiratesIdCard:
							{
								var ct = _container.Resolve<ISessionContext>();
								Context.Get<IAuthDataContext>().EIdNumber = ct.CardUsed?.EmiratesId?.Id;

								await _getCardsStep.ExecuteAsync();
								await _selectCardStep.ExecuteAsync();
								await _sendSelectedCardStep.ExecuteAsync();

								for (int i = 0; i < attempts; i++)
								{
									await _pinPad.StartPinReading();
									await _enterPinStep.ExecuteAsync();
									LoadWaitScreen();

									Context.Get<IAuthDataContext>().Pin = await _pinPad.BuildPinBlockDieboldAsync(ct.CardUsed.EmiratesId?.Id);
									await _pinPad.StopPinReading();

									try
									{
										await _validatePinEidNdcStep.ExecuteAsync();
									}
									catch (InvalidPinException ex)
									{
										_logger.Exception(ex);
										_journal.AuthenticationFailed();
										await LoadErrorScreenAsync(ErrorType.InvalidPin, () => { }, false);
									}

									if (Context.Get<IAuthDataContext>().Authenticated)
										break;
								}

								break;
							}
						case CardType.Offus:
							{
                                _journal.OffUsCard();
                                _container.Resolve<AuthenticatedOffUsStandbyWorkflow>().Execute();
                                int errorScreenTimeout = Int32.Parse(ConfigurationManager.AppSettings["OffUsScreenTimeout"].ToString());
                                await _returnCardStep.ReturnCard();
                                await LoadTimedErrorScreenAsync(ErrorType.InvalidCardNumber, async () =>
								{
									LoadMainScreen();
									return;
								}, errorScreenTimeout);

								break;
							}
					}

					if (Context.Get<IAuthDataContext>().Authenticated && !string.IsNullOrEmpty(_container.Resolve<ISessionContext>().CustomerIdentifier))
					{
						_journal.CIF(_container.Resolve<ISessionContext>()?.CustomerIdentifier);
						_journal.AuthenticationSucceeded();
						await CheckNdcCardType();
						LoadSelfServiceMenu();
						//_cardReader.CancelReadCard();
					}
					else if (Context.Get<IAuthDataContext>()?.Card?.CardType != CardType.Offus)
					{
						_journal.AuthenticationFailed();
						await LoadErrorScreenAsync(ErrorType.AuthenticationFailed, async () =>
						{
							await _returnCardStep.ReturnCard();
							LoadMainScreen();
						});
					}
				}
			}
			catch (ArgumentNullException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, async () =>
				{
					await _returnCardStep.ReturnCard();
					LoadMainScreen();
				});
			}
			catch (ExceededPinException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.ExceededPin, async () =>
				{
					await _returnCardStep.ReturnCard();
					LoadMainScreen();
				});
			}
			catch (ExpiredCardException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.ExpiredCard, async () =>
				{
					await _returnCardStep.ReturnCard();
					LoadMainScreen();
				});
			}
			catch (BlockedCardException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.BlockedCard, async () =>
				{
					await _returnCardStep.ReturnCard();
					LoadMainScreen();
				});
			}
			catch (CardCaptureException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.CapturedCard, async () =>
				{
					var captureCardWorkflowStep = _container.Resolve<CaptureCardStep>();
					await captureCardWorkflowStep.CaptureCard();
					LoadMainScreen();
				});
			}
			catch (HttpRequestException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, async () =>
				{
					await _returnCardStep.ReturnCard();
					LoadMainScreen();
				});
			}
			catch (DeviceTimeoutException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await _returnCardStep.ReturnCard();
				LoadMainScreen();
			}
			catch (SocketException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, async () =>
				{
					await _returnCardStep.ReturnCard();
					LoadMainScreen();
				});
			}
			catch (EIDCardsNotFoundException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.CardsNotLinked, async () =>
				{
					await _returnCardStep.ReturnCard();
					LoadMainScreen();
				});
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, async () =>
				{
					await _returnCardStep.ReturnCard();
					LoadMainScreen();
				});
			}
		}

		private async Task CheckNdcCardType()
		{
            _logger?.Info($"Execute Task: Check Card Type");

            if (Context.Get<IAuthDataContext>()?.Card?.CardType == CardType.EmiratesIdCard)
			{
				var transactionService = _container.Resolve<Services.Interface.ITransactionService>();
				var creditCards = await transactionService.GetCreditCardsAsync(_container.Resolve<ISessionContext>().CustomerIdentifier);
				if (creditCards != null)
				{
					var cr = creditCards.Where(x => x.Number.GetLastCharacters(4) == Context.Get<IAuthDataContext>().SelectedCard?.CardNumber?.GetLastCharacters(4)).FirstOrDefault();
					if (cr != null)
					{
						if (_container.Resolve<ISessionContext>().CardUsed == null)
							_container.Resolve<ISessionContext>().CardUsed = new Devices.Interface.Entities.Card();

						_container.Resolve<ISessionContext>().CardUsed.CardType = CardType.CreditCard;
						_container.Resolve<ISessionContext>().CardUsed.CardNumber = cr.Number;
					}
				}
			}
		}

		public override void Dispose()
		{

		}
	}
}