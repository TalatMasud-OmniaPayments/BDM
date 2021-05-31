namespace Omnia.Pie.Vtm.Workflow.Authentication
{
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using Omnia.Pie.Vtm.Devices.Interface;
    using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Services.Interface;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
	using System.Web.Script.Serialization;

    public class FingerprintScanningStep : WorkflowStep
	{
        private TaskCompletionSource<bool> _completion;
        private readonly IFingerPrintScanner _fingerPrintScanner;
        private string fingerprint = "";
        public FingerprintScanningStep(IResolver container) : base(container)
		{
            //_completion = new TaskCompletionSource<bool>();
            _fingerPrintScanner = _container.Resolve<IFingerPrintScanner>();
        }

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Scan Fingerprint");
            _completion = new TaskCompletionSource<bool>();
            SetCurrentStep($"{nameof(FingerprintScanningStep)}");
            //_fingerPrintScanner.InitializeFingerScanner();
            //await Task.Delay(500).ContinueWith(t => StartScanningAsync());
            if (_fingerPrintScanner.GetFingerPrintStatus() == DeviceStatus.Online)
            {
                var cancellationToken = new CancellationTokenSource();
                try
                {
                    //_fingerPrintScanner.InitializeFingerScanner();

                    _navigator.RequestNavigationTo<IFingerprintScanningViewModel>(async (vm) =>
                    {

                        vm.Type(AnimationType.FingerperintScan);
                        vm.BackVisibility = vm.DefaultVisibility = false;
                    //_fingerPrintScanner.InitializeFingerScanner();
                    vm.CancelVisibility = true;

                        vm.CancelAction = async () =>
                        {
                            _fingerPrintScanner.StopFingerScanner();
                            cancellationToken?.Cancel();
                            cancellationToken = null;

                            CancelAction?.Invoke();
                            _completion.TrySetResult(false);
                            return;

                        };

                        vm.DefaultAction = async () =>
                        {

                        };

                        vm.StartUserActivityTimer(cancellationToken.Token);
                        vm.ExpiredAction = () =>
                        {
                            _navigator.Push<IActiveConfirmationViewModel>((viewmodel) =>
                            {
                                viewmodel.StartTimer(new TimeSpan(0, 0, InactivityTimer));
                                viewmodel.YesAction = () =>
                                {
                                    vm.StartUserActivityTimer(cancellationToken.Token);
                                    _navigator.Pop();
                                };
                                viewmodel.NoAction = viewmodel.ExpiredAction = () =>
                                {
                                    _fingerPrintScanner.StopFingerScanner();
                                    cancellationToken?.Cancel();
                                    cancellationToken = null;
                                    CancelAction?.Invoke();
                                //_completion.SetResult(false);
                            };
                            });
                        };

                        if (await StartScanningAsync())
                        {
                            try
                            {

                                //return;
                                var _userInfoService = _container.Resolve<IUserService>();
                                var _userInfo = await _userInfoService.RegisterFingerprintAsync(Context.Get<IAuthDataContext>().loggedInUserInfo.Username, fingerprint, vm.SelectedFinger.Index);
                                if (_userInfo.ResponseCode == "000")
                                {
                                    _completion.SetResult(true);
                                    cancellationToken?.Cancel();
                                    cancellationToken = null;
                                // code to return success

                            }
                                else
                                {
                                    cancellationToken?.Cancel();
                                    cancellationToken = null;
                                    throw new System.Exception("Fingerprint Registration failed.");
                                }

                            }
                            catch (Exception ex)
                            {
                                cancellationToken?.Cancel();
                                cancellationToken = null;
                                throw ex;
                            }

                        }
                        else
                        {
                            throw new System.Exception("Fingerprint Registration failed.");

                        }



                    });

                    //await _cardReader.RetainCardAsync();  // Finger scanning module
                    return await _completion.Task;
                }
                catch (Exception ex)
                {
                    _completion.SetResult(false);
                    throw ex;
                }
            }
            else
            {
                
                await LoadErrorScreenAsync(ErrorType.FingerScannerNotAvailable, async () =>
                {
                    //Execute(Context);
                    _completion.SetResult(false);
                    LoadMainScreen();

                }, false);
                return await _completion.Task;
            }
        }


        /*protected async Task StartScanningAsync()
        {
            _logger?.Info($"Execute Step: Scan Fingerprint Starting scan");
            var fingerprint = await _fingerPrintScanner.CaptureFingerPrintAsync();
            Context.Get<IAuthDataContext>().Fingerprint = fingerprint;
            _completion.TrySetResult(true);
        }*/

            
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