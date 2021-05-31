namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
    using Omnia.Pie.Supervisor.Shell.Utilities;
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Services.Interface;
    using Omnia.Pie.Vtm.Workflow;
    using Omnia.Pie.Vtm.Workflow.Authentication.Steps;
    using Omnia.Pie.Vtm.Workflow.Common.Steps;
    using Omnia.Pie.Supervisor.Shell;
    using Omnia.Pie.Vtm.DataAccess.Interface;
    using Microsoft.Practices.Unity;
    using System;
    using Omnia.Pie.Supervisor.Shell.ViewModels.Pages;
    using Omnia.Pie.Vtm.Workflow.Common.Context;

    internal class AuthenticationLoginWorkflow : Workflow
	{

        private readonly EnterUsernamePasswordStep _enterUsrPwdStep;
        private readonly ValidateUsernamePasswordStep _validateUsrPwdStep;
        private readonly ValidateFingerprintStep _validateFingerprintStep;
        private readonly FingerprintScanningStep _fingerprintScanningStep;
        private readonly IFingerPrintScanner _fingerPrintScanner;
        //private readonly FingerprintScanRegisterStep _fingerprintScanRegisterStep;
        //UserRole
        //private readonly AdminUserMainMenuStep _adminUserMainMenuStep;
        //public ClearCashInViewModel ClearCashInViewModel;

        public AuthenticationLoginWorkflow(IResolver container) : base(container)
		{

            _enterUsrPwdStep = _container.Resolve<EnterUsernamePasswordStep>();
            _validateUsrPwdStep = _container.Resolve<ValidateUsernamePasswordStep>();
            _fingerprintScanningStep = _container.Resolve<FingerprintScanningStep>();
            //_fingerprintScanRegisterStep = _container.Resolve<FingerprintScanRegisterStep>();
            _validateFingerprintStep = _container.Resolve<ValidateFingerprintStep>();
            _fingerPrintScanner = _container.Resolve<IFingerPrintScanner>();
            //_adminUserMainMenuStep = _container.Resolve<AdminUserMainMenuStep>();
            //ClearCashInViewModel = new ClearCashInViewModel();

            Context = _enterUsrPwdStep.Context = _validateUsrPwdStep.Context = _fingerprintScanningStep.Context = _validateFingerprintStep.Context = CreateContext(typeof(AuthDataContext));

			AddSteps($"{Properties.Resources.StepEnterPin},{Properties.Resources.StepPinValidation},{Properties.Resources.StepFingerprintScan}");

            _enterUsrPwdStep.BackAction = async () =>
            {
                _journal.TransactionCanceled();
                LoadMainScreen();
            };

            _fingerprintScanningStep.CancelAction = async () =>
            {
                _journal.TransactionCanceled();
                LoadMainScreen();
            };
            
			Context.Get<IAuthDataContext>().SelfServiceMode = true;
            //Screens.OnLoggoutSupervisoryUser += Screens_OnLoggoutSupervisoryUser;
		}

        

        public async void Execute()
        {
            _logger?.Info($"Execute Workflow: Authentication Using Login Account");

            
            //TODO: create configuration entry for username and password attempts
            var attempts = 3;
            int.TryParse(SystemParametersConfiguration.GetElementValue("MaxOTPAttemptLimit"), out attempts);

            for (int i = 0; i < attempts; i++)
            {

                if (await _enterUsrPwdStep.ExecuteAsync()) { 
                    try
                        {
                            await _validateUsrPwdStep.ExecuteAsync();
                        }
                    catch (InvalidOtpException)
                        {
                        //TODO: Create Invalid username and password Error Type
                        if (i == 1) {
                            await LoadErrorScreenAsync(ErrorType.InvalidLoginLastTry, () => { }, false);
                        }
                        else { 
                            await LoadErrorScreenAsync(ErrorType.InvalidLogin, () => { }, false);
                        }
                    }
                }

                if (Context.Get<IAuthDataContext>().Authenticated)
                    break;

                if (!Context.Get<IAuthDataContext>().Authenticated && i == 2)
                {

                    LoadMainScreen();
                }
            }

            //Context.Get<IAuthDataContext>().loggedInUserInfo.ResponseCode = "107";
            if (Context.Get<IAuthDataContext>().Authenticated && (Context.Get<IAuthDataContext>().loggedInUserInfo.ResponseCode == "050" || Context.Get<IAuthDataContext>().loggedInUserInfo.ResponseCode == "107"))    // 050 user logged in first time. 107 user logged in he reset the password but did not done the fingerprint.
             {
                /*int.TryParse(SystemParametersConfiguration.GetElementValue("MaxOTPAttemptLimit"), out attempts);

                for (int i = 0; i < attempts; i++)
                {
                    try
                    {
                        await _fingerprintScanRegisterStep.ExecuteAsync();
                    }
                    catch (InvalidOtpException)
                    {
                        //TODO: Create Invalid username and password Error Type
                        await LoadErrorScreenAsync(ErrorType.InvalidOtp, () => { }, false);
                    }

                    if (Context.Get<IAuthDataContext>().AuthenticatedByFingerprint)
                        break;
                }*/
                if (_fingerPrintScanner.GetFingerPrintStatus() == DeviceStatus.Online)
                {
                    using (var flow = _container.Resolve<Authentication.RegisterNewUserWorkflow>())
                    {
                        flow.ExecuteAsync(Context);
                    }
                }
                else
                {
                    //await LoadErrorScreenAsync(ErrorType.InvalidLogin, () => { }, false);
                    await LoadErrorScreenAsync(ErrorType.FingerScannerNotAvailable, async () =>
                    {
                        //Execute(Context);
                        LoadMainScreen();
                    }, false);
                }
            }
           
            /*else if (Context.Get<IAuthDataContext>().Authenticated && Context.Get<IAuthDataContext>().loggedInUserInfo.ResponseCode == "000")
            {

                //TODO: create configuration entry for fingerprint scanning attempts
                int.TryParse(SystemParametersConfiguration.GetElementValue("MaxOTPAttemptLimit"), out attempts);

                for (int i = 0; i < attempts; i++)
                {
                    try
                    {
                        await _fingerprintScanningStep.ExecuteAsync();

                        try
                        {
                            await _validateFingerprintStep.ExecuteAsync();
                        }
                        catch (Exception e)
                        {
                            //TODO: Create Invalid username and password Error Type
                            await LoadErrorScreenAsync(ErrorType.InvalidFingerprint, () => { }, false);
                        }
                    }
                    catch (InvalidOtpException)
                    {
                        //TODO: Create Invalid username and password Error Type
                        await LoadErrorScreenAsync(ErrorType.InvalidOtp, () => { }, false);
                    }

                    if (Context.Get<IAuthDataContext>().AuthenticatedByFingerprint)
                        break;
                    if (!Context.Get<IAuthDataContext>().AuthenticatedByFingerprint && i == 2)
                    {

                        LoadMainScreen();
                    }
                }
            }*/

            //if (Context.Get<IAuthDataContext>().Authenticated && Context.Get<IAuthDataContext>().AuthenticatedByFingerprint)
            else if (Context.Get<IAuthDataContext>().Authenticated && Context.Get<IAuthDataContext>().loggedInUserInfo.ResponseCode == "000")
            {



                //var user = new IUsersStore();

                //Context.Get<IAuthDataContext>().loggedInUserInfo.UserType = "Supervisor";       // hardcoded


                if (Context.Get<IAuthDataContext>().loggedInUserInfo.UserType.ToUpper() == UserTypes.DEPOSIT)
                {
                    var doors = _container.Resolve<IDoors>();
                    if (doors.GetSafeDoorStatus() != DoorStatus.Open)
                    {
                        using (var flow = _container.Resolve<Authentication.BusinessUserMainMenuWorkFlow>())
                    {
                        flow.Context = Context;
                        flow.Execute();
                    }
                        }
                else
                {

                        await LoadErrorScreenAsync(ErrorType.OutOfService, () => { }, false);
                        LoadMainScreen();
                        //Screens.OutOfServiceViewsShow();
                        //_workflowCompletionTask.SetException(new DeviceMalfunctionException("CashDepositFailed"));
                    }
                }
                else if (Context.Get<IAuthDataContext>().loggedInUserInfo.UserType.ToUpper() == UserTypes.CIT)
                {
                   
                    
                    using (var flow = _container.Resolve<Authentication.CITWorkFlow>())
                    {
                        flow.Execute(Context);
                        //Screens.OutOfServiceViewsShow();
                        Screens.SetLogin(false, Context.Get<IAuthDataContext>().loggedInUserInfo.UserType);
                        //Screens.UpdateToMainApp(false);
                    }
                }
                else if (Context.Get<IAuthDataContext>().loggedInUserInfo.UserType.ToUpper() == UserTypes.SLM)
                {

                    using (var flow = _container.Resolve<Authentication.CITWorkFlow>())
                    {
                        flow.Execute(Context);
                        //Screens.OutOfServiceViewsShow();
                        Screens.SetLogin(false, Context.Get<IAuthDataContext>().loggedInUserInfo.UserType);
                        //Screens.UpdateToMainApp(false);
                    }
                }
                else if (Context.Get<IAuthDataContext>().loggedInUserInfo.UserType.ToUpper() == UserTypes.Supervisor)
                {

                    using (var flow = _container.Resolve<Authentication.CITWorkFlow>())
                    {
                        flow.Execute(Context);
                        //Screens.OutOfServiceViewsShow();
                        Screens.SetLogin(true,Context.Get<IAuthDataContext>().loggedInUserInfo.UserType);
                        //Screens.UpdateToMainApp(false);
                    }
                }
            }
            
        }

        

        public override void Dispose()
		{

            
        }
	}
}