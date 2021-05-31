using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Workflow.CashDeposit.Context;
using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.CashDeposit;
using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
using System.Threading;
using Omnia.Pie.Vtm.Framework.Configurations;

namespace Omnia.Pie.Vtm.Workflow.CashDeposit.Account.Steps
{
	class DepositConfirmationStep : WorkflowStep
	{
		private readonly TaskCompletionSource<bool> _taskSource;
		public Action _depositConfirmedAction;
		private readonly ICashAcceptor _cashAcceptor;
        private IDepositConfirmationViewModel vm;

        public DepositConfirmationStep(IResolver container) : base(container)
		{
			_taskSource = new TaskCompletionSource<bool>();
			_cashAcceptor = _container.Resolve<ICashAcceptor>();
            vm = _container.Resolve<IDepositConfirmationViewModel>();
        }

		private async Task<ICashDepositContext> ProcessCash(ICashDepositContext _context)
		{
            _logger?.Info($"Execute Step: Process Cash");

            var insertedCash = await _cashAcceptor.AcceptCashAsync();

            /*var cessetteStatus = _cashAcceptor.GetCashAcceptorStatus();
            if (cessetteStatus == CashAcceptorCassetteStatus.OK || cessetteStatus == CashAcceptorCassetteStatus.HIGH)
            {
                vm.BackVisibility = true;
            }
            var cassetteAvailability = _cashAcceptor.AvailableToDeposit() - insertedCash.TotalNotes;
            vm.AvailableCapacity = cassetteAvailability.ToString();*/
            //var cassetteAvailability = _cashAcceptor.AvailableToDeposit();
            
            //_cashAcceptor.GetMediaInfo();
            _logger.Info("inserted cash length: " + insertedCash.Denominations.Length);
			CashDenomination[] denoms = insertedCash.Denominations;

			var depositedCash = denoms.Select(d => new DepositDenomination
			{
				Denomination = d.Value,
				Quantity = d.Count
			}).ToList();

            //int _1Count = 0;
            int _5Count = 0;
            int _10Count = 0;
			int _20Count = 0;
			int _50Count = 0;
			int _100Count = 0;
			int _200Count = 0;
			int _500Count = 0;
			int _1000Count = 0;

			foreach (var item in depositedCash)
			{
                /*if (item.Denomination == 1)
                    _1Count += item.Quantity;*/

                if (item.Denomination == 5)
                    _5Count += item.Quantity;

                if (item.Denomination == 10)
					_10Count += item.Quantity;

				if (item.Denomination == 20)
					_20Count += item.Quantity;

				if (item.Denomination == 50)
					_50Count += item.Quantity;

				if (item.Denomination == 100)
					_100Count += item.Quantity;

				if (item.Denomination == 200)
					_200Count += item.Quantity;

				if (item.Denomination == 500)
					_500Count += item.Quantity;

				if (item.Denomination == 1000)
					_1000Count += item.Quantity;
			}

			var depCash = new List<DepositDenomination>();

            /*if (_1Count > 0)
                depCash.Add(new DepositDenomination()
                {
                    Quantity = _1Count,
                    Denomination = 1
                });*/

            if (_5Count > 0)
                depCash.Add(new DepositDenomination()
                {
                    Quantity = _5Count,
                    Denomination = 5
                });

            if (_10Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _10Count,
					Denomination = 10
				});

			if (_20Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _20Count,
					Denomination = 20
				});

			if (_50Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _50Count,
					Denomination = 50
				});

			if (_100Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _100Count,
					Denomination = 100
				});

			if (_200Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _200Count,
					Denomination = 200
				});

			if (_500Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _500Count,
					Denomination = 500
				});

			if (_1000Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _1000Count,
					Denomination = 1000
				});

			_context.DepositedCash = depCash;
			_context.Currency = TerminalConfiguration.Section?.Currency;
			_context.TotalAmount = insertedCash.TotalAmount;
			_context.TotalNotes = insertedCash.TotalNotes;

			return _context;
		}

		private async Task<ICashDepositContext> ProcessAddMoreCash(ICashDepositContext _context)
		{
            _logger?.Info($"Execute Step: Add More Cash");

            var insertedCash = await _cashAcceptor.AcceptMoreCashAsync();
            //_cashAcceptor.GetMediaInfo();
            //var vm = _container.Resolve<IDepositConfirmationViewModel>();
            vm.BackVisibility = false;
            //_cashAcceptor.GetMediaInfo();
            if (insertedCash == null)
			{
				return _context;
			}
            //var insertedNotes = _cashAcceptor.GetStackerCash();
            //var escrowAvailability = _cashAcceptor.GetMaxStackerLimit() - insertedNotes;
            //var cessetteStatus = _cashAcceptor.GetCashAcceptorStatus();
            //if (cessetteStatus == CashAcceptorCassetteStatus.OK || cessetteStatus == CashAcceptorCassetteStatus.HIGH || escrowAvailability <= 10)
            //{
            //    vm.BackVisibility = true;
            //}
            //var cassetteAvailability = _cashAcceptor.AvailableToDepositCashIn() - insertedCash.TotalNotes;          //with MaxCashIn Cassette

            var stackerStatus = _cashAcceptor.GetStackerStatus();
            //var escrowAvailability = _cashAcceptor.GetMaxStackerLimit() - insertedCash;
            //var cessetteStatus = _cashAcceptor.GetCashAcceptorStatus();
            if (stackerStatus == CashAcceptorStackerStatus.EMPTY || stackerStatus == CashAcceptorStackerStatus.NOTEMPTY)
            {
                vm.BackVisibility = true;
            }


            //vm.AvailableCapacity = escrowAvailability.ToString();

            CashDenomination[] denoms = insertedCash.Denominations;

			List<DepositDenomination> depositedCash = denoms.Select(d => new DepositDenomination
			{
				Denomination = d.Value,
				Quantity = d.Count
			})
			.ToList();

            //int _1Count = 0;
            int _5Count = 0;
            int _10Count = 0;
			int _20Count = 0;
			int _50Count = 0;
			int _100Count = 0;
			int _200Count = 0;
			int _500Count = 0;
			int _1000Count = 0;

			foreach (var item in depositedCash)
			{
                /*if (item.Denomination == 1)
                    _1Count += item.Quantity;*/

                if (item.Denomination == 5)
                    _5Count += item.Quantity;

                if (item.Denomination == 10)
					_10Count += item.Quantity;

				if (item.Denomination == 20)
					_20Count += item.Quantity;

				if (item.Denomination == 50)
					_50Count += item.Quantity;

				if (item.Denomination == 100)
					_100Count += item.Quantity;

				if (item.Denomination == 200)
					_200Count += item.Quantity;

				if (item.Denomination == 500)
					_500Count += item.Quantity;

				if (item.Denomination == 1000)
					_1000Count += item.Quantity;
			}

			var depCash = new List<DepositDenomination>();

            /*if (_1Count > 0)
                depCash.Add(new DepositDenomination()
                {
                    Quantity = _1Count,
                    Denomination = 1
                });*/

            if (_5Count > 0)
                depCash.Add(new DepositDenomination()
                {
                    Quantity = _5Count,
                    Denomination = 5
                });

            if (_10Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _10Count,
					Denomination = 10
				});

			if (_20Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _20Count,
					Denomination = 20
				});

			if (_50Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _50Count,
					Denomination = 50
				});

			if (_100Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _100Count,
					Denomination = 100
				});

			if (_200Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _200Count,
					Denomination = 200
				});

			if (_500Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _500Count,
					Denomination = 500
				});

			if (_1000Count > 0)
				depCash.Add(new DepositDenomination()
				{
					Quantity = _1000Count,
					Denomination = 1000
				});

			_context.DepositedCash = depCash;
			_context.Currency = TerminalConfiguration.Section?.Currency;
			_context.TotalAmount = insertedCash.TotalAmount;
			_context.TotalNotes = insertedCash.TotalNotes;
			return _context;
		}

		public async Task<bool> DepositCash()
		{
            _logger?.Info($"Execute Step: Deposit Cash");

            var _context = Context.Get<ICashDepositContext>();
			SetCurrentStep(Properties.Resources.StepDenomination);

			_navigator.RequestNavigationTo<IAnimationViewModel>(async (viewmodel) =>
			{
				var cancellationToken = new CancellationTokenSource();
				try
				{
					viewmodel.Type(AnimationType.InsertCash);
					_context = await ProcessCash(_context);
					//var vm = _container.Resolve<IDepositConfirmationViewModel>();
					vm.DepositedCash = _context.DepositedCash;
					vm.Currency = _context.Currency;
					vm.TotalNotes = _context.TotalNotes;
					vm.TotalAmount = _context.TotalAmount;
                    //vm.AvailableCapacity = (_cashAcceptor.AvailableToDepositCashIn() - _context.TotalNotes).ToString();     //With MaxCashIn cassette
                    vm.AvailableCapacity = (_cashAcceptor.GetMaxStackerLimit() - _context.TotalNotes).ToString();       // with stacker limit


                    vm.SelectedAccount = _context.SelectedAccount;
					/*if (_context.TotalAmount == 0)
					{
						cancellationToken?.Cancel();
						cancellationToken = null;

						_taskSource.SetException(new DeviceMalfunctionException("CashDepositFailed"));
						return;
					}*/
                    vm.BackVisibility = false;
                    
                    var stackerStatus = _cashAcceptor.GetStackerStatus();
                    //var escrowAvailability = _cashAcceptor.GetMaxStackerLimit() - insertedCash;
                    //var cessetteStatus = _cashAcceptor.GetCashAcceptorStatus();
                    _logger?.Info($"CashAcceptorStackerStatus: " + stackerStatus);
                    if (stackerStatus == CashAcceptorStackerStatus.EMPTY || stackerStatus == CashAcceptorStackerStatus.NOTEMPTY)
                    {
                        vm.BackVisibility = true;
                    }
                    
					vm.AddMoreAction = () =>
					{
                        cancellationToken?.Cancel();
                        cancellationToken = null;
                        _journal.Write("Add more cash");

                        _navigator.RequestNavigationTo<IAnimationViewModel>(async (vm1) =>
						{
							try
							{
								vm1.Type(AnimationType.InsertCash);
								_context = await ProcessAddMoreCash(_context);

								//vm.BackVisibility = false;
								vm.DepositedCash = _context.DepositedCash;
								vm.Currency = _context.Currency;
								vm.TotalNotes = _context.TotalNotes;
								vm.TotalAmount = _context.TotalAmount;
                                if (_context.TotalAmount > 0)
                                {
                                    vm.DefaultVisibility = true;
                                }
                                cancellationToken = new CancellationTokenSource();
                                vm.StartUserActivityTimer(cancellationToken.Token);

                                _navigator.RequestNavigation(vm);
							}
							catch (DeviceTimeoutException ex)
							{
								_logger.Exception(ex);

								//vm.BackVisibility = false;
								vm.DepositedCash = _context.DepositedCash;
								vm.Currency = _context.Currency;
								vm.TotalNotes = _context.TotalNotes;
								vm.TotalAmount = _context.TotalAmount;

								_navigator.RequestNavigation(vm);

                                _navigator.Push<IActiveConfirmationViewModel>((viewmod) =>
                                {
                                    viewmod.StartTimer(new TimeSpan(0, 0, InactivityTimer));
                                    viewmod.YesAction = () =>
                                    {
                                        cancellationToken?.Cancel();
                                        cancellationToken = null;
                                        cancellationToken = new CancellationTokenSource();
                                        vm.StartUserActivityTimer(cancellationToken.Token);
                                        _navigator.Pop();
                                        vm.AddMoreAction();
                                    };
                                    viewmod.NoAction = () =>
                                    {
                                        cancellationToken?.Cancel();
                                        cancellationToken = null;
                                        cancellationToken = new CancellationTokenSource();
                                        vm.StartUserActivityTimer(cancellationToken.Token);
                                        _navigator.Pop();
                                    };
                                    viewmod.ExpiredAction = () =>
                                    {
                                        cancellationToken?.Cancel();
                                        cancellationToken = null;

                                        ExpiredAction();
                                        _taskSource.SetResult(true);
                                    };
                                });
                            }
							catch (Exception ex)
							{
								_logger.Exception(ex);

								cancellationToken?.Cancel();
								cancellationToken = null;

								_taskSource.SetException(new DeviceMalfunctionException("CashDepositFailed"));
								return;
							}


						});
					};

                    vm.DefaultVisibility = false;

                    if (_context.TotalAmount > 0)
                    {
                        vm.DefaultVisibility = true;
                    }
                    vm.DefaultAction = () =>
					{
                    _navigator.RequestNavigationTo<IAnimationViewModel>((vm1) =>
                    /*cancellationToken?.Cancel();
                    cancellationToken = null;
                    _depositConfirmedAction();*/
                    {
                        try
							{
								vm1.Type(AnimationType.Wait);

								cancellationToken?.Cancel();
								cancellationToken = null;

								_depositConfirmedAction();
							}
							catch (Exception ex)
							{
								_logger.Exception(ex);

								cancellationToken?.Cancel();
								cancellationToken = null;

								_taskSource.SetException(new DeviceMalfunctionException("CashDepositFailed"));
								return;
							}
						});
                    };

					vm.CancelVisibility = true;
					vm.CancelAction = () =>
					{
						try
						{
							_context.IsCanceled = true;

							cancellationToken?.Cancel();
							cancellationToken = null;

							_depositConfirmedAction();
						}
						catch (Exception ex)
						{
							_logger.Exception(ex);

							cancellationToken?.Cancel();
							cancellationToken = null;

							_taskSource.SetException(new DeviceMalfunctionException("CashDepositFailed"));
							return;
						}
					};

					if (_context.SelfService)
					{
						vm.StartUserActivityTimer(cancellationToken.Token);
						vm.ExpiredAction = () =>
						{
							_navigator.Push<IActiveConfirmationViewModel>((viewmod) =>
							{
								viewmod.StartTimer(new TimeSpan(0, 0, InactivityTimer));
								viewmod.YesAction = () =>
								{
									vm.StartUserActivityTimer(cancellationToken.Token);
									_navigator.Pop();
								};
								viewmod.NoAction = () =>
								{
									cancellationToken?.Cancel();
									cancellationToken = null;

									CancelAction();
									_taskSource.SetResult(true);
								};
                                viewmod.ExpiredAction = () =>
                                {
                                    cancellationToken?.Cancel();
                                    cancellationToken = null;

                                    ExpiredAction();
                                    _taskSource.SetResult(true);
                                };
                            });
						};
					}

					_navigator.Push(vm);
				}
				catch (DeviceTimeoutException ex)
				{
					_logger.Exception(ex);

					cancellationToken?.Cancel();
					cancellationToken = null;

					_taskSource.SetException(new DeviceTimeoutException("CashDepositFailed"));
					return;
				}
				catch (DeviceMalfunctionException ex)
				{
					_logger.Exception(ex);

					cancellationToken?.Cancel();
					cancellationToken = null;

					_taskSource.SetException(new DeviceMalfunctionException("CashDepositFailed"));
					return;
				}
				catch (Exception ex)
				{
					_logger.Exception(ex);

					//if (_cashAcceptor.HasPendingCashInside)
					//{
					//	while(_cashAcceptor.HasPendingCashInside)
					//		await LoadErrorScreenAsync(ErrorType.CollectCash);
					//}
					//if (_cashAcceptor.HasMediaInserted)
					//{
					//	await _cashAcceptor.RollbackCashAndWaitTakenAsync();
					//}
					_cashAcceptor.CancelAcceptCash();

					cancellationToken?.Cancel();
					cancellationToken = null;

					_taskSource.SetException(new DeviceMalfunctionException("CashDepositFailed"));
					return;
				}
			});

			return await _taskSource.Task;
		}

		public override void Dispose()
		{

		}
	}
}