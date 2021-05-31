using Omnia.Pie.Client.Journal.Interface.Extension;
using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.Configurations;
using Omnia.Pie.Vtm.Framework.Exceptions;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Framework.Interface.Receipts;
using Omnia.Pie.Vtm.Services.Interface;
using Omnia.Pie.Vtm.Workflow.Common.Context;
using Omnia.Pie.Vtm.Workflow.Common.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.Authentication
{
    internal class RegisterNewUserWorkflow : Workflow
    {
        //private readonly FingerprintScanRegisterStep _fingerprintScanRegisterStep;
        private readonly ChangePasswordStep _changePasswordStep;
        private readonly IFingerPrintScanner _fingerPrintScanner;
        private readonly FingerprintScanningStep _fingerprintScanningStep;
        private readonly RegisterNewUserSuccessStep _registerNewUserSuccessStep;
        private readonly IReceiptFormatter _receiptFormatter;
        private readonly PrintReceiptStep _printReceiptStep;
        private string fingerprint = "";

        public RegisterNewUserWorkflow(IResolver container, IReceiptFormatter receiptFormatter) : base(container)
        {

            _fingerPrintScanner = _container.Resolve<IFingerPrintScanner>();
            _changePasswordStep = _container.Resolve<ChangePasswordStep>();
            _fingerprintScanningStep = _container.Resolve<FingerprintScanningStep>();
            _registerNewUserSuccessStep = _container.Resolve<RegisterNewUserSuccessStep>();
            AddSteps($"{Properties.Resources.StepUpdateInfoSuccess}");
            _receiptFormatter = receiptFormatter;
            _printReceiptStep = _container.Resolve<PrintReceiptStep>();

            _registerNewUserSuccessStep.CancelAction = _changePasswordStep.CancelAction = _fingerprintScanningStep.CancelAction = _registerNewUserSuccessStep.CancelAction = () =>
            {
                //ExecuteAsync(_registerNewUserSuccessStep.Context);
                LoadMainScreen();
            };
        }

        public async void ExecuteAsync(IDataContext _context)
        {
            _logger?.Info($"Execute Step: Scan Fingerprint");
            Context = _changePasswordStep.Context = _registerNewUserSuccessStep.Context = _fingerprintScanningStep.Context = _context;
            //SetCurrentStep($"{nameof(FingerprintScanRegisterStep)}");
            //var cancellationToken = new CancellationTokenSource();
            try
            {

                bool isPasswordUpdated = false;
                //_changePasswordStep.ExecuteAsync();

                if (Context.Get<IAuthDataContext>().loggedInUserInfo.ResponseCode == "107")
                {
                    isPasswordUpdated = true;
                }
                else
                {
                    isPasswordUpdated = await _changePasswordStep.ExecuteAsync();
                }
                

                if (isPasswordUpdated)
                {
                    //_fingerPrintScanner.InitializeFingerScanner();

                    if (await _fingerprintScanningStep.ExecuteAsync())
                    {
                        // scanned successfully

                        if (await _registerNewUserSuccessStep.ExecuteAsync())
                        {
                            try
                            {
                                LoadWaitScreen();

                                var receiptData = await _receiptFormatter.FormatAsync(new RegisterFingerprintReceipt
                                {
                                    userName = Context.Get<IAuthDataContext>().loggedInUserInfo.Username,
                                    name = Context.Get<IAuthDataContext>().loggedInUserInfo.Name,
                                    mobile = Context.Get<IAuthDataContext>().loggedInUserInfo.Mobile,
                                    email = Context.Get<IAuthDataContext>().loggedInUserInfo.Email,
                                });

                                _journal.PrintingReceipt(receiptData);
                                await _printReceiptStep.PrintReceipt(true, receiptData);
                                //cancellationToken?.Cancel();
                                //cancellationToken = null;
                                LoadMainScreen();
                                //_registerNewUserSuccessStep.CancelAction();
                            }
                            catch (Exception ex)
                            {
                                _journal.TransactionFailed(ex.GetBaseException().Message);
                                _logger.Exception(ex);
                                await LoadErrorScreenAsync(ErrorType.UnablePrintReceipt);
                                //SendNotification(Services.Interface.TransactionType.SSCashDepositAccountCardless, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Receipt printing failed");
                                //_workflowCompletionTask.SetResult(true);
                            }

                        }
                    }

                    /*
                    var cancellationToken = new CancellationTokenSource();
                    try
                    {
                        _navigator.RequestNavigationTo<IAnimationViewModel>((viewModel) =>
                        {
                            viewModel.Type(AnimationType.FingerperintRegister);
                            viewModel.CancelVisibility = true;


                            viewModel.CancelAction = () =>
                            {
                                _fingerPrintScanner.StopFingerScanner();
                                cancellationToken?.Cancel();
                                cancellationToken = null;
                                LoadMainScreen();

                            };

                        });

                        if (await StartScanningAsync())
                        {
                            try
                            {
                                var _userInfoService = _container.Resolve<IUserService>();
                                var _userInfo = await _userInfoService.RegisterFingerprintAsync(Context.Get<IAuthDataContext>().loggedInUserInfo.Username, fingerprint);
                                if (_userInfo.ResponseCode == "000")
                                {
                                    if (await _registerNewUserSuccessStep.ExecuteAsync())
                                    {
                                        try
                                        {
                                            LoadWaitScreen();

                                            var receiptData = await _receiptFormatter.FormatAsync(new RegisterFingerprintReceipt
                                            {
                                                userName = Context.Get<IAuthDataContext>().loggedInUserInfo.Username,
                                                name = Context.Get<IAuthDataContext>().loggedInUserInfo.Name,
                                                mobile = Context.Get<IAuthDataContext>().loggedInUserInfo.Mobile,
                                                email = Context.Get<IAuthDataContext>().loggedInUserInfo.Email,
                                            });

                                            _journal.PrintingReceipt(receiptData);
                                            await _printReceiptStep.PrintReceipt(true, receiptData);
                                            cancellationToken?.Cancel();
                                            cancellationToken = null;
                                            LoadMainScreen();
                                            //_registerNewUserSuccessStep.CancelAction();
                                        }
                                        catch (Exception ex)
                                        {
                                            _journal.TransactionFailed(ex.GetBaseException().Message);
                                            _logger.Exception(ex);
                                            await LoadErrorScreenAsync(ErrorType.UnablePrintReceipt);
                                            //SendNotification(Services.Interface.TransactionType.SSCashDepositAccountCardless, "Self Service", _container.Resolve<ISessionContext>()?.CardUsed?.CardNumber, "", "", Context.Get<ICashDepositContext>()?.SelectedAccount?.Number, _container.Resolve<ISessionContext>()?.CustomerIdentifier, transactionStatus: Services.Interface.Enums.TransactionStatus.Failure, reason: "Receipt printing failed");
                                            //_workflowCompletionTask.SetResult(true);
                                        }

                                    }
                                }
                                else
                                {
                                    throw new System.Exception("Fingerprint Registration failed.");
                                }

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }

                        }
                        else
                        {
                            throw new System.Exception("Fingerprint Registration failed.");
                        }



                        //await _cardReader.RetainCardAsync();  // Finger scanning module
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    */

                }
                else
                {
                    throw new System.Exception("Fingerprint Registration failed.");
                }
            }
            catch (Exception ex)
            {
                _journal.TransactionFailed(ex.GetBaseException().Message);
                _logger.Exception(ex);
                await LoadErrorScreenAsync(ErrorType.FailUpdatePassword);

                LoadMainScreen();
            }


        }

        protected async Task<bool> StartScanningAsync()
        {
            _logger?.Info($"Execute Step: Scan Fingerprint Starting scan");
            fingerprint = await _fingerPrintScanner.CaptureFingerPrintAsync();


            //return _scanningTask.TrySetResult(fingerprint); ;
            //_completion.TrySetResult(true);
            return true;
        }
        public override void Dispose()
        {

        }
    }
}
