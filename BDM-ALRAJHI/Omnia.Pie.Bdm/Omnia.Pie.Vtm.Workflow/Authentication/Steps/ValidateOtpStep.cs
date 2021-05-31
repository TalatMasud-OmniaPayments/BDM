namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using System;
	using System.Threading.Tasks;

	internal class ValidateOtpStep : WorkflowStep
	{
		private TaskCompletionSource<bool> _completion;

		public ValidateOtpStep(IResolver container) : base(container)
		{

		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Validate SMS OTP");

            _completion = new TaskCompletionSource<bool>();
			SetCurrentStep($"{nameof(ValidateOtpStep)}");

			try
			{
				var ctx = Context.Get<IAuthDataContext>();
				if (!string.IsNullOrEmpty(ctx.Otp))
				{
					var _authService = _container.Resolve<IAuthenticationService>();
					var valid = await _authService.ValidateSmsOtp(ctx.Otp, ctx.Uuid);

					if (valid.OtpMatched)
					{
						Context.Get<IAuthDataContext>().OtpMatched = true;
						Context.Get<IAuthDataContext>().CustomerId = Context.Get<IAuthDataContext>().Cif;
					}
					else
					{
						Context.Get<IAuthDataContext>().OtpMatched = false;
						throw new InvalidOtpException();
					}
				}
			}
			catch (InvalidOtpException ex)
			{
				_logger.Exception(ex);
				throw ex;
			}
		}

		public override void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}