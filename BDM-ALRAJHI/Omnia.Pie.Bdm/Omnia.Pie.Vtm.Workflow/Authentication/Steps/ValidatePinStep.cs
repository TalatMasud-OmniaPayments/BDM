namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using System.Threading.Tasks;
	using System.Web.Script.Serialization;

	internal class ValidatePinStep : WorkflowStep
	{
		public ValidatePinStep(IResolver container) : base(container)
		{

		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Validate PIN");

            SetCurrentStep($"{nameof(ValidatePinStep)}");

			LoadWaitScreen();

			var ctx = Context.Get<IAuthDataContext>();
			var _authService = _container.Resolve<IAuthenticationService>();
			var _pinVerificationResult = await _authService.VerifyPin(ctx?.Card?.Track2, ctx?.Pin, ctx?.Card?.EmvData?.IccData);

			_logger?.Info("Is PIN Valid? " + new JavaScriptSerializer().Serialize(_pinVerificationResult));

			if (_pinVerificationResult?.ResponseCode == "000" && !string.IsNullOrEmpty(_pinVerificationResult.CustomerIdentifier))
			{
				Context.Get<IAuthDataContext>().Authenticated = true;
				Context.Get<IAuthDataContext>().CustomerId = _pinVerificationResult.CustomerIdentifier;
				_container.Resolve<ISessionContext>().CustomerIdentifier = Context.Get<IAuthDataContext>().CustomerId;
				_container.Resolve<ISessionContext>().Pin = ctx?.Pin;
			}
			else
				throw new System.Exception("Validation failed.");
		}

		public override void Dispose()
		{

		}
	}
}