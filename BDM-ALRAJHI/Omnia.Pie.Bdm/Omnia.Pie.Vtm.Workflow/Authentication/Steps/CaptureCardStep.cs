namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using System.Threading.Tasks;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;

	public class CaptureCardStep : WorkflowStep
	{
		private readonly ICardReader _cardReader;

		public CaptureCardStep(IResolver container) : base(container)
		{
			_cardReader = _container.Resolve<ICardReader>();
		}

		public async Task CaptureCard()
		{
            _logger?.Info($"Execute Step: Capturing Card");

            try
			{
				_navigator.RequestNavigationTo<IAnimationViewModel>((viewModel) =>
				{
					viewModel.Type(AnimationType.RetractingCard);
					viewModel.BackVisibility = viewModel.CancelVisibility = viewModel.DefaultVisibility = false;
				});

				await _cardReader.RetainCardAsync();
			}
			catch (DeviceMalfunctionException)
			{
				// Device Malfunction Trigger
			}
		}

		public override void Dispose()
		{
			
		}
	}
}