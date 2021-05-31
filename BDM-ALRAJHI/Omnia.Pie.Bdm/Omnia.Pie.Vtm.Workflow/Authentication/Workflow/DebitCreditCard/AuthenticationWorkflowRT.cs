namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Communication.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Workflow;
	using Omnia.Pie.Vtm.Workflow.Authentication.Context;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
    using Omnia.Pie.Vtm.Workflow.Common.Steps;
    using System;
	using System.Net.Http;
	using System.Threading.Tasks;

	public class AuthenticationWorkflowRT : Workflow
	{
		private readonly ICardReader _cardReader;
		private readonly IPinPad _pinPad;
		private readonly IESpaceTerminalCommunication _communicator;

		private readonly ReadCardStep _readCardStep;
		private readonly EnterPinStep _enterPinStep;
		private readonly ReturnCardStep _returnCardStep;
		private readonly ValidatePinNdcStep _validatePinStep;
		private readonly ValidatePinEidNdcStep _validatePinEidNdcStep;
		private readonly GetCardsStep _getCardsStep;
		private readonly SelectCardStep _selectCardStep;
		private readonly SendSelectedCardStep _sendSelectedCardStep;
        private readonly CheckCardReaderStep _checkCardReaderStep;

        public AuthenticationWorkflowRT(IResolver container) : base(container)
		{
			_cardReader = _container.Resolve<ICardReader>();
			_pinPad = _container.Resolve<IPinPad>();
			_communicator = _container.Resolve<IESpaceTerminalCommunication>();

			//_communicator.RTMessageReceived += _communicator_RTMessageReceived;

			_readCardStep = _container.Resolve<ReadCardStep>();
			_enterPinStep = _container.Resolve<EnterPinStep>();
			_validatePinStep = _container.Resolve<ValidatePinNdcStep>();
			_validatePinEidNdcStep = _container.Resolve<ValidatePinEidNdcStep>();
			_returnCardStep = _container.Resolve<ReturnCardStep>();
			_getCardsStep = _container.Resolve<GetCardsStep>();
			_selectCardStep = _container.Resolve<SelectCardStep>();
			_sendSelectedCardStep = _container.Resolve<SendSelectedCardStep>();
            _checkCardReaderStep = _container.Resolve<CheckCardReaderStep>();


            Context = _readCardStep.Context = _enterPinStep.Context = _validatePinStep.Context =
					_getCardsStep.Context = _selectCardStep.Context = _validatePinEidNdcStep.Context =
				_sendSelectedCardStep.Context = CreateContext(typeof(AuthDataContext));

			AddSteps($"{Properties.Resources.StepReadCard},{Properties.Resources.StepEnterPin},{Properties.Resources.StepPinValidation}");

			_enterPinStep.CancelAction = async () =>
			{
				LoadWaitScreen();
				await _pinPad.StopPinReading();

				_journal.TransactionCanceled();
				await _returnCardStep.ReturnCard();
				_communicator.SendStatus(StatusEnum.EndCurrentSession);

				LoadStandByRT();
			};

			_selectCardStep.CancelAction = async () =>
			{
				_journal.TransactionCanceled();
				await _returnCardStep.ReturnCard();
				_communicator.SendStatus(StatusEnum.EndCurrentSession);

				LoadStandByRT();
			};
		}

		public async void Execute()
		{
            _logger?.Info($"Execute Workflow: Card Authentication - RT Assisted");

            try
			{

                _logger?.Info($"Checking CardReader Availability");
                await _checkCardReaderStep.CheckCardReaderAsync();

                _navigator.RequestNavigationTo<IAnimationViewModel>((vm) =>
				{
					vm.Type(AnimationType.InsertCard);
				});

				await _readCardStep.ReadCardAsync();

				var attempts = 3;
				int.TryParse(SystemParametersConfiguration.GetElementValue("MaxPinAttemptLimit"), out attempts);

				if (Context.Get<IAuthDataContext>()?.Card != null)
				{
					switch (Context.Get<IAuthDataContext>().Card.CardType)
					{
						case CardType.CreditCard:
						case CardType.DebitCard:
							{
								_communicator.SendStatus(StatusEnum.AuthenticateWaitingForPin);

								for (int i = 0; i < attempts; i++)
								{
									await _pinPad.StartPinReading();
									await _enterPinStep.ExecuteAsync();
									LoadWaitScreen();
									var ct = _container.Resolve<ISessionContext>();
									Context.Get<IAuthDataContext>().Pin = await _pinPad.BuildPinBlockDieboldAsync(Context.Get<IAuthDataContext>().Card);
									//Context.Get<IAuthDataContext>().Pin = "<=:11>0402;=:155";
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
									_communicator.SendStatus(StatusEnum.AuthenticateWaitingForPin);
									await _pinPad.StartPinReading();
									await _enterPinStep.ExecuteAsync();
									LoadWaitScreen();
									Context.Get<IAuthDataContext>().Pin = await _pinPad.BuildPinBlockDieboldAsync(ct.CardUsed.EmiratesId?.Id);
									//Context.Get<IAuthDataContext>().Pin = "<=:11>0402;=:155";
									await _pinPad.StopPinReading();

									_communicator.SendStatus(StatusEnum.AuthenticateWaitingForAuthentication);

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
								_communicator.SendStatus(StatusEnum.AuthenticateOffUsCard);
								_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);

								break;
							}
					}

					if (Context.Get<IAuthDataContext>().Authenticated && !string.IsNullOrEmpty(_container.Resolve<ISessionContext>().CustomerIdentifier))
					{
						_journal.AuthenticationSucceeded();

						_communicator.SendStatus(StatusEnum.AuthenticateTerminalId,
												new { TerminalId = TerminalConfiguration.Section.Id });
						_communicator.SendStatus(StatusEnum.AuthenticateBranchId,
											new { BranchId = TerminalConfiguration.Section.Id });

						if (Context.Get<IAuthDataContext>().Card.CardType == CardType.EmiratesIdCard)
						{
							_communicator.SendStatus(StatusEnum.AuthenticateEmiratesIdAuthenticated,
												new { CustomerId = Context.Get<IAuthDataContext>().CustomerId });
						}
						else
						{
							_communicator.SendStatus(StatusEnum.AuthenticateCardAuthenticated,
												new { CustomerId = Context.Get<IAuthDataContext>().CustomerId });
						}

						LoadStandByRT();
					}
					else
					{
						_journal.AuthenticationFailed();
						_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);
						await LoadErrorScreenAsync(ErrorType.AuthenticationFailed, () =>
						{
							_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);
							LoadStandByRT();
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
					_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);
					await _returnCardStep.ReturnCard();
					LoadStandByRT();
				});
			}
			catch (ExceededPinException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.ExceededPin, async () =>
				{
					_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);
					await _returnCardStep.ReturnCard();
					LoadStandByRT();
				});
			}
			catch (ExpiredCardException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.ExpiredCard, async () =>
				{
					_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);
					await _returnCardStep.ReturnCard();
					LoadStandByRT();
				});
			}
			catch (BlockedCardException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.BlockedCard, async () =>
				{
					_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);
					await _returnCardStep.ReturnCard();
					LoadStandByRT();
				});
			}
			catch (InvalidPinException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.InvalidPin, async () =>
				{
					_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);
					await _returnCardStep.ReturnCard();
					LoadStandByRT();
				});
			}
			catch (HttpRequestException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, async () =>
				{
					_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);
					await _returnCardStep.ReturnCard();
					LoadStandByRT();
				});
			}
			catch (DeviceTimeoutException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);
				await _returnCardStep.ReturnCard();
				LoadStandByRT();
			}
			catch (EIDCardsNotFoundException ex)
			{
				_logger.Exception(ex);
				_journal.AuthenticationFailed();

				await LoadErrorScreenAsync(ErrorType.CardsNotLinked, async () =>
				{
					_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);
					await _returnCardStep.ReturnCard();
					LoadStandByRT();
				});
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				await LoadErrorScreenAsync(ErrorType.GenericUnavailableService, async () =>
				{
					_communicator.SendStatus(StatusEnum.AuthenticationFailedEndCurrentSession);
					await _returnCardStep.ReturnCard();
					LoadStandByRT();
				});
			}
			finally
			{
				DisposeSteps();
			}
		}

		//private void _communicator_RTMessageReceived(object sender, RTMessageEventArgs e)
		//{
		//	if (e.MessageCode.Code == (int)StatusEnum.AuthenticateDebitCreditCard)
		//	{

		//	}
		//}

		public override void Dispose()
		{
			//_communicator.RTMessageReceived -= _communicator_RTMessageReceived;

			//TODO need to test with some memory profiler
			GC.SuppressFinalize(this);
		}
	}
}