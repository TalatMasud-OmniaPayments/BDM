namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using System;
	using System.Threading.Tasks;

	internal class SendSmsOtpStep : WorkflowStep
	{
		private TaskCompletionSource<bool> _completion;

		public SendSmsOtpStep(IResolver container) : base(container)
		{

		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Send SMS OTP");

            SetCurrentStep($"{Properties.Resources.StepSendSMSOTP}");

			LoadWaitScreen();
			await Task.Delay(100);
			
			_completion = new TaskCompletionSource<bool>();

			var otpTimeLimit = 3;
			int.TryParse(SystemParametersConfiguration.GetElementValue("OTPTimeLimit"), out otpTimeLimit);

			try
			{
				var _authService = _container.Resolve<IAuthenticationService>();
				var resp = await _authService.SendSmsOtp(Context.Get<IAuthDataContext>().Cif, otpTimeLimit.ToString());

				Context.Get<IAuthDataContext>().Uuid = resp.Uuid;
			}
			catch (Exception ex)
			{
				_completion.TrySetException(ex);
				throw;
			}
		}

		public override void Dispose()
		{

		}
	}
}