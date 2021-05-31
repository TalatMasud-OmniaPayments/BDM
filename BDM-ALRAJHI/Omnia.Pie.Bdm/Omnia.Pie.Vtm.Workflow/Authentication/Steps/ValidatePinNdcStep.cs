namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.ServicesNdc.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using System.Threading.Tasks;
	using System.Web.Script.Serialization;

	internal class ValidatePinNdcStep : WorkflowStep
	{
		public ValidatePinNdcStep(IResolver container) : base(container)
		{

		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Validate PIN");

            SetCurrentStep($"{Properties.Resources.StepPinValidation}");

			LoadWaitScreen();

			var ctx = Context.Get<IAuthDataContext>();
			var _authService = _container.Resolve<INdcService>();
            
			ctx.Pin = ReplaceCharacters(ctx?.Pin);

			var _pinVerificationResult = await _authService.ValidatePinAsync(ctx?.Card?.Track2, ctx?.Pin);

			_logger?.Info("Is PIN Valid? " + new JavaScriptSerializer().Serialize(_pinVerificationResult));

			if (_pinVerificationResult)
			{
				Context.Get<IAuthDataContext>().Authenticated = true;

				var _authenticationService = _container.Resolve<ICustomerService>();
				var result = await _authenticationService.GetCustomerIdentifierAsync(ctx?.Card?.CardNumber, CardType.CreditCardDebitCard);

				_logger?.Info("CIF No: " + new JavaScriptSerializer().Serialize(result));

				if (result == null || string.IsNullOrEmpty(result.CustomerId))
					throw new System.Exception("Validation failed.");

				_container.Resolve<ISessionContext>().CustomerIdentifier = Context.Get<IAuthDataContext>().CustomerId = result?.CustomerId;
				_container.Resolve<ISessionContext>().Pin = ctx?.Pin;
				_journal.CIF(_container.Resolve<ISessionContext>()?.CustomerIdentifier);
			}
			else
				throw new System.Exception("Validation failed.");
		}

		private string ReplaceCharacters(string pin)
		{
			pin = pin.Replace('A', ':');
			pin = pin.Replace('B', ';');
			pin = pin.Replace('C', '<');
			pin = pin.Replace('D', '=');
			pin = pin.Replace('E', '>');
			pin = pin.Replace('F', '?');

			return pin;
		}

		public override void Dispose()
		{

		}
	}
}