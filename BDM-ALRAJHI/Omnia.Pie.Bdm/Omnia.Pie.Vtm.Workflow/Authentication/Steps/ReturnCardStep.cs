namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System.Threading.Tasks;

	internal class ReturnCardStep : WorkflowStep
	{
		private readonly ICardReader _cardReader;

		public ReturnCardStep(IResolver container) : base(container)
		{
			_cardReader = container.Resolve<ICardReader>();
		}

		public async Task ReturnCard()
		{
            _logger?.Info($"Execute Step: Return Card");

            SetCurrentStep($"{Properties.Resources.StepReadCard}");

			try
			{
				if (_cardReader.IsCardInside)
				{
					_navigator.RequestNavigationTo<IAnimationViewModel>((viewModel) =>
					{
						viewModel.Type(AnimationType.TakeCard);
						viewModel.BackVisibility = viewModel.CancelVisibility = viewModel.DefaultVisibility = false;
					});

					await _cardReader.EjectCardAndWaitTakenAsync();
				}
			}
			catch (DeviceMalfunctionException ex)
			{
				_cardReader.CancelReadCard();
				_logger.Exception(ex);
				//Device Malfunction 
			}
			catch (DeviceTimeoutException ex)
			{
				_logger.Exception(ex);
				//Device Timeout 
				var captureCardWorkflowStep = _container.Resolve<CaptureCardStep>();
				await captureCardWorkflowStep.CaptureCard();
			}
			finally
			{
				_cardReader.CancelReadCard();
			}
		}

		public override void Dispose()
		{

		}
	}
}