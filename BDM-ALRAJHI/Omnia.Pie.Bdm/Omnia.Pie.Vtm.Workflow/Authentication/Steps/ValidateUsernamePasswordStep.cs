namespace Omnia.Pie.Vtm.Workflow.Authentication
{
    using Omnia.Pie.Client.Journal.Interface.Extension;
    using Omnia.Pie.Supervisor.Shell.Service;
    using Omnia.Pie.Vtm.DataAccess.Interface;
    using Omnia.Pie.Vtm.Framework.Exceptions;
    using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using System.Threading.Tasks;
    using Microsoft.Practices.Unity;
    using System.Web.Script.Serialization;
    using Omnia.Pie.Vtm.Services.Interface.Entities;
    using System.Configuration;

    internal class ValidateUsernamePasswordStep : WorkflowStep
	{
        private readonly IUsersStore _userStore = ServiceLocator.Instance.Resolve<IUsersStore>();

        public ValidateUsernamePasswordStep(IResolver container) : base(container)
		{

		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Validate Username and Password");

            SetCurrentStep($"{nameof(ValidateUsernamePasswordStep)}");

            LoadWaitScreen();
            try
            {
                var ctx = Context.Get<IAuthDataContext>();
                ctx.Authenticated = false;
                //var _authService = _container.Resolve<IAuthenticationService>();
                var _userAuthenticationService = _container.Resolve<IUserService>();
                Context.Get<IAuthDataContext>().isOnlineTran = _networkStatus.isConnected();
                UserInfo _userAuth; 
            //_container.Resolve<ISessionContext>().CustomerIdentifier = Context.Get<IAuthDataContext>().CustomerId = "5555555";
                _container.Resolve<ISessionContext>().CustomerIdentifier = Context.Get<IAuthDataContext>().CustomerId = ConfigurationManager.AppSettings["CustomerId"].ToString();// Hardcoded values

                if (Context.Get<IAuthDataContext>().isOnlineTran)
                {
                    _journal.Write("Online user authentication");
                    //Context.Get<IAuthDataContext>().isOnlineTran = true;
                    _userAuth = await _userAuthenticationService.ValidatePasswordAsync(Context.Get<IAuthDataContext>().Username, Context.Get<IAuthDataContext>().Password);
                    _userAuth.Username = Context.Get<IAuthDataContext>().Username;
                    _userAuth.Password = Context.Get<IAuthDataContext>().Password;
                    _journal.Write($"Username: {_userAuth.Username} online authenticated");
                }
                else
                {
                    //Context.Get<IAuthDataContext>().isOnlineTran = false;
                    _journal.Write("Offline user authentication");
                    _userAuth = ValidateUserOffline(Context.Get<IAuthDataContext>().Username);

                    if (_userAuth.ResponseCode.Length > 0)
                    {
                        _userAuth.Username = Context.Get<IAuthDataContext>().Username;
                        _userAuth.Password = Context.Get<IAuthDataContext>().Password;
                        _journal.Write($"Username: {_userAuth.Username} offline authenticated");
                    }
                    /*else
                    {
                        _journal.Write("Offline authentication failed");
                    }*/

                }
                

            Context.Get<IAuthDataContext>().loggedInUserInfo = _userAuth;
                MonitorDeviceStatus();

                //TODO: modify with correct logic based on authentication result
                if (ctx.loggedInUserInfo.Username.Length > 0 && ctx.loggedInUserInfo.Password.Length > 0)
            {

                ctx.Authenticated = true;
            }
            else
            {
                    _journal.Write("user authentication failed");
                    throw new System.Exception("Validation failed.");
            }
            }
            catch (System.Exception ex)
            {
                
                _journal.Write("user authentication failed");
                _journal.TransactionFailed(ex.GetBaseException().Message);
                _logger.Exception(ex);

                throw new InvalidOtpException();
                /*await LoadErrorScreenAsync(ErrorType.InvalidLogin);
                //SendNotification(Services.Interface.TransactionType.SSCashDepositAccountCardless, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Account entry failed");
                _completion.TrySetResult(false);*/
                //return;

            }

        }

        private UserInfo ValidateUserOffline(string username) {

           var userInfo = _userStore.ValidateUser(username);

            return userInfo;
        }


		public override void Dispose()
		{

		}
	}
}