namespace Omnia.Pie.Vtm.Workflow.Authentication
{
    using Omnia.Pie.Supervisor.Shell.Service;
    using Omnia.Pie.Vtm.DataAccess.Interface;
    using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using System.Threading.Tasks;
	using System.Web.Script.Serialization;

	internal class ValidateFingerprintStep : WorkflowStep
	{

        public ValidateFingerprintStep(IResolver container) : base(container)
		{

		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Validate Fingerprint");

            SetCurrentStep($"{nameof(ValidateFingerprintStep)}");

            LoadWaitScreen();

            var ctx = Context.Get<IAuthDataContext>();
            var _authService = _container.Resolve<IAuthenticationService>();


            var _fingerprintVerificationResult = true;
            //ctx.AuthenticatedByFingerprint = true;
            //TODO: modify with correct logic based on fingerprint authentication result
            //var _userInfoService = _container.Resolve<IUserService>();
            //var _userInfo = await _userInfoService.ValidateFingerprintAsync(Context.Get<IAuthDataContext>().loggedInUserInfo.Username, Context.Get<IAuthDataContext>().Fingerprint);


            //_fingerprintVerificationResult = true;
            if (_fingerprintVerificationResult)
            {
                var _userInfoService = _container.Resolve<IUserService>();
                ctx.allUserTypes = await _userInfoService.GetUserTypesAsync(Context.Get<IAuthDataContext>().loggedInUserInfo.Username);
                ctx.AuthenticatedByFingerprint = true;
            }
            else
                throw new System.Exception("Fingerprint Validation failed.");

        }

		public override void Dispose()
		{

		}
	}
}