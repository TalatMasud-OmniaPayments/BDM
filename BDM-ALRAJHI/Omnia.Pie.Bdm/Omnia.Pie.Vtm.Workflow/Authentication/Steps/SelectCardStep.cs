namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.ServicesNdc.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public class SelectCardStep : WorkflowStep
	{
		private readonly TaskCompletionSource<bool> _completion;

		public SelectCardStep(IResolver container) : base(container)
		{
			_completion = new TaskCompletionSource<bool>();
		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Select Card");

            var cancellationToken = new CancellationTokenSource();
			SetCurrentStep($"{Properties.Resources.StepCardSelection}");

			_navigator.RequestNavigationTo<ICardImageListViewModel>((viewModel) =>
			{
				viewModel.Cards = Context.Get<IAuthDataContext>().Cards;
				viewModel.CancelVisibility = true;
				viewModel.DefaultVisibility = true;

				viewModel.DefaultAction = () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;

					try
					{
						Context.Get<IAuthDataContext>().SelectedCard = viewModel.SelectedCard;
						_container.Resolve<ISessionContext>().CardFdk = viewModel.SelectedCard.CardFDK;

						if (_container.Resolve<ISessionContext>().CardUsed == null)
							_container.Resolve<ISessionContext>().CardUsed = new Devices.Interface.Entities.Card() { CardNumber = viewModel.SelectedCard.CardNumber };
						else
						{
							_container.Resolve<ISessionContext>().CardUsed.CardNumber = viewModel.SelectedCard.CardNumber;
						}

						_logger?.Info($"Selected Card Number : {viewModel?.SelectedCard?.CardNumber} FDK {viewModel?.SelectedCard?.CardFDK}");

						_completion.TrySetResult(true);
					}
					catch (Exception ex)
					{
						throw ex;
					}
				};
				viewModel.CancelAction = async () =>
				{
					cancellationToken?.Cancel();
					cancellationToken = null;
					var _ndcService = _container.Resolve<INdcService>();
					await _ndcService.CardSelectedAsync(Context.Get<IAuthDataContext>().EIdNumber, "D", false);
					await _ndcService.GetReadyAsync();
					await _ndcService.ValidateEidPinAsync("11111111111111", "L", "111111111111", false);
					CancelAction?.Invoke();
				};

				if (Context.Get<IAuthDataContext>().SelfServiceMode)
				{
					viewModel.StartUserActivityTimer(cancellationToken.Token);
					viewModel.ExpiredAction = () =>
					{
						_navigator.Push<IActiveConfirmationViewModel>((vm) =>
						{
							vm.StartTimer(new TimeSpan(0, 0, InactivityTimer));
							vm.YesAction = () =>
							{
								viewModel.StartUserActivityTimer(cancellationToken.Token);
								_navigator.Pop();
							};
							vm.NoAction = vm.ExpiredAction = () =>
							{
								cancellationToken?.Cancel();
								cancellationToken = null;

								CancelAction?.Invoke();
							};
						});
					};
				}
			});

			return await _completion.Task;
		}

		public override void Dispose()
		{
			
		}
	}
}