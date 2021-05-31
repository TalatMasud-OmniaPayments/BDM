namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using System;
	using System.Threading.Tasks;

	internal class EnterOtpStep : WorkflowStep
	{
		private TaskCompletionSource<bool> _completion;

		public EnterOtpStep(IResolver container) : base(container)
		{
		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Enter OTP");

            _completion = new TaskCompletionSource<bool>();
			SetCurrentStep($"{Properties.Resources.StepEnterOtp}");

			var otpTimeLimit = 3;
			int.TryParse(SystemParametersConfiguration.GetElementValue("OTPTimeLimit"), out otpTimeLimit);

			_navigator.RequestNavigationTo<IEnterOtpViewModel>((viewModel) =>
			{
				viewModel.DefaultVisibility = viewModel.CancelVisibility = true;
				viewModel.CancelAction = () =>
				{
					CancelAction?.Invoke();
				};
				viewModel.DefaultAction = () =>
				{
					Context.Get<IAuthDataContext>().Otp = viewModel.Otp;
					_completion.TrySetResult(true);
				};
				viewModel.ExpiredAction = async () =>
				{
					_logger.Error("OTP Timer has been expired");
					await LoadErrorScreenAsync(ErrorType.ExpiredOtp, () =>
					{
						_completion.TrySetResult(false);
					});
				};

				viewModel.StartTimer(new TimeSpan(0, 0, (otpTimeLimit * 60)));
			});

			return await _completion.Task;
		}

		public override void Dispose()
		{
		}
	}
}