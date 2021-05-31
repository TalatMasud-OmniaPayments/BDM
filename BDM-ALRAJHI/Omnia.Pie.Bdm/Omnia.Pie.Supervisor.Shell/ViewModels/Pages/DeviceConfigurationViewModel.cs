using System;
using Omnia.Pie.Vtm.Devices.Interface;
using Microsoft.Practices.Unity;
using System.Windows.Input;
using Omnia.Pie.Supervisor.Shell.Service;
using System.Windows;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Client.Journal.Interface.Extension;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using Omnia.Pie.Vtm.Framework.Extensions;
using Omnia.Pie.Vtm.Devices.Interface.Entities;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
    public class DeviceConfigurationViewModel : PageViewModel
    {
        private ILogger _logger = ServiceLocator.Instance.Resolve<ILogger>();
        //public override bool IsEnabled => Context.IsLoggedInMode;
        //public ICashAcceptor CashAcceptor { get; } = ServiceLocator.Instance.Resolve<ICashAcceptor>();

        public override bool IsEnabled => (Context.IsLoggedInMode && Context.UserRoles?.DeviceConfiguration == true ? true : false);
        public ObservableCollection<DeviceError> Errors { get; } = new ObservableCollection<DeviceError>();



        private IFingerPrintScanner _fingerPrintScanner = ServiceLocator.Instance.Resolve<IFingerPrintScanner>();

        public override void Load()
        {
            LoadDevices();
        }

        public DeviceConfigurationViewModel()
        {
            CashAcceptor.CashAcceptorStatusChanged += CashAcceptor_CashAcceptorStatusChanged;
            CardReader.CardReaderStatusChanged += CardReader_CardReaderStatusChanged;
            Printer.ReceiptPrinterDeviceStatusChanged += Printer_ReceiptPrinterDeviceStatusChanged;
            PinPad.PinPadStatusChanged += PinPad_PinPadStatusChanged;
            LoadDevices();
            Reset = new DelegateCommand<DeviceConfiguration>(
                async x =>
                {
                    try
                    {
                        Context.DisplayProgress = true;
                        var Device = x.Type;
                        switch (Device)
                        {
                            case DeviceConfiguration.DeviceType.CardReader:
                                {
                                    await CardReader.ResetAsync();
                                    _journal?.Reset(DeviceType.CardReader.ToString());
                                    await _channelManagementService.InsertEventAsync("Reset Card Reader", "True");

                                    //ShowErrorMessage(x.Type);
                                    break;
                                }
                            case DeviceConfiguration.DeviceType.CashAcceptor:
                                {
                                    await CashAcceptor.ResetAsync();
                                    _journal?.Reset(DeviceType.CashAcceptor.ToString());
                                    await _channelManagementService.InsertEventAsync("Reset Cash Acceptor", "True");
                                    x.Status = CashAcceptor.Status;

                                    await CashDispenser.ResetAsync();
                                    _journal?.Reset(DeviceType.CashDispenser.ToString());
                                    await _channelManagementService.InsertEventAsync("Reset Cash Dispenser", "True");

                                    //ShowErrorMessage(x.Type);
                                    //await CashDispenser.ResetAsync();
                                    //_journal?.Reset(DeviceType.CashDispenser.ToString());
                                    //await _channelManagementService.InsertEventAsync("Reset Cash Dispenser", "True");
                                    break;
                                }
                            case DeviceConfiguration.DeviceType.Cassettes:
                                {
                                    try
                                    {
                                        CashAcceptor.PerformExchange();
                                        //this.Context.RunTask(CashAcceptor.PerformExchange()).Result;
                                        _journal?.Reset(DeviceType.Cassettes.ToString());
                                        await _channelManagementService.InsertEventAsync("Reset Cassettes Start and End Exchange", "True");
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Info("Unable to perform start and end exchange, new and the old cassettes are FATAL");
                                        _logger.Exception(ex);

                                    }
                                    finally
                                    {
                                        await CashAcceptor.ResetAsync();
                                        _journal?.Reset(DeviceType.Cassettes.ToString());
                                        await _channelManagementService.InsertEventAsync("Reset Cash Acceptor for cassettes", "True");
                                        CashAcceptor.isCassettteChanged = false;
                                    }

                                    

                                    
                                    
                                    //ShowErrorMessage(x.Type);
                                    //await CashDispenser.ResetAsync();
                                    //_journal?.Reset(DeviceType.CashDispenser.ToString());
                                    //await _channelManagementService.InsertEventAsync("Reset Cash Dispenser", "True");
                                    break;
                                }
                            /*case DeviceConfiguration.DeviceType.CoinDispenser:
								{
									await CoinDispenser.ResetAsync();
									_journal?.Reset(DeviceType.CoinDispenser.ToString());
									await _channelManagementService.InsertEventAsync("Reset Coin Dispenser", "True");
									break;
								}
							case DeviceConfiguration.DeviceType.ChequeAcceptor:
								{
									await CheckAcceptor.ResetAsync();
									_journal?.Reset(DeviceType.ChequeAcceptor.ToString());
									await _channelManagementService.InsertEventAsync("Reset Cheque Acceptor", "True");
									break;
								}*/
                            case DeviceConfiguration.DeviceType.PinPad:
								{
									await PinPad.ResetAsync();
									PinPad.StartReading();

									_journal?.Reset(DeviceType.PinPad.ToString());
									await _channelManagementService.InsertEventAsync("Reset PinPad", "True");
									break;
								}
                            case DeviceConfiguration.DeviceType.ReceiptPrinter:
                                {
                                    //Printer.Dispose();
                                    //Printer.Initialize();
                                    //_journal?.Initialized(DeviceType.ReceiptPrinter.ToString());

                                    await Printer.ResetAsync();
                                    //Printer.TestAsync();

                                    _journal?.Reset(DeviceType.ReceiptPrinter.ToString());

                                    await _channelManagementService.InsertEventAsync("Reset Receipt Printer", "True");
                                    //ShowErrorMessage(x.Type);
                                    break;
                                }
                            case DeviceConfiguration.DeviceType.FingerPrintScanner:
                                {
                                    //await FingerPrintScanner.ResetAsync();
                                    _fingerPrintScanner.StopFingerScanner();
                                    _fingerPrintScanner.InitializeFingerScanner();
                                    _journal?.Reset(DeviceType.FingerPrintScanner.ToString());
                                    await _channelManagementService.InsertEventAsync("Reset Finger Print Scanner", "True");
                                    break;
                                }
                            default:
                                break;
                        }


                    }
                    catch (Exception ex)
                    {
                        _logger.Exception(ex);
                        //MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                        x.Status = DeviceStatus.HwError;
                        ShowErrorMessage(x.Type);
                    }
                    finally
                    {
                        LoadDevices();
                        //RaisePropertyChanged("Devices");
                        Context.DisplayProgress = false;
                    }
                }, x => true);

            Initialize = new DelegateCommand<DeviceConfiguration>(
                async x =>
                {
                    Context.DisplayProgress = true;

                    try
                    {
                        var Device = x.Type;
                        switch (Device)
                        {
                            case DeviceConfiguration.DeviceType.CardReader:
                                {
                                    CardReader.Dispose();
                                    CardReader.Initialize();
                                    _journal?.Initialized(DeviceType.CardReader.ToString());
                                    await _channelManagementService.InsertEventAsync("Initialize Card Reader", "True");
                                    break;
                                }
                            case DeviceConfiguration.DeviceType.CashAcceptor:
                                {
                                    CashAcceptor.Dispose();
                                    //CashDispenser.Dispose();

                                    CashAcceptor.Initialize();
                                    //CashDispenser.Initialize();
                                    _journal?.Initialized(DeviceType.CashAcceptor.ToString());
                                    await _channelManagementService.InsertEventAsync("Initialize Cash Acceptor", "True");

                                    CashDispenser.Initialize();
                                    //CashDispenser.Initialize();
                                    _journal?.Initialized(DeviceType.CashDispenser.ToString());
                                    await _channelManagementService.InsertEventAsync("Initialize Cash Dispenser", "True");

                                    break;
                                }
                            case DeviceConfiguration.DeviceType.Cassettes:
                                {
                                    CashAcceptor.Dispose();

                                    CashAcceptor.Initialize();
                                    //CashDispenser.Initialize();
                                    _journal?.Initialized(DeviceType.Cassettes.ToString());
                                    await _channelManagementService.InsertEventAsync("Initialize Cash Acceptor for cassettes", "True");

                                    
                                    break;
                                }
                            /*case DeviceConfiguration.DeviceType.CoinDispenser:
								{
									CoinDispenser.Dispose();
									CoinDispenser.Initialize();
									_journal?.Initialized(DeviceType.CoinDispenser.ToString());
									await _channelManagementService.InsertEventAsync("Initialize Coin Dispenser", "True");
									break;
								}
							case DeviceConfiguration.DeviceType.ChequeAcceptor:
								{
									CheckAcceptor.Dispose();
									CheckAcceptor.Initialize();
									_journal?.Initialized(DeviceType.ChequeAcceptor.ToString());
									await _channelManagementService.InsertEventAsync("Initialize Cheque Acceptor", "True");
									break;
								}*/
                            case DeviceConfiguration.DeviceType.PinPad:
								{
									PinPad.Dispose();
									PinPad.Initialize();
									PinPad.StartReading();
									_journal?.Initialized(DeviceType.PinPad.ToString());
									await _channelManagementService.InsertEventAsync("Initialize Pin Pad", "True");
									break;
								}
                            case DeviceConfiguration.DeviceType.ReceiptPrinter:
                                {
                                    Printer.Dispose();
                                    Printer.Initialize();
                                    _journal?.Initialized(DeviceType.ReceiptPrinter.ToString());
                                    await _channelManagementService.InsertEventAsync("Initialize Receipt Printer", "True");
                                    break;
                                }
                            case DeviceConfiguration.DeviceType.FingerPrintScanner:
                                {

                                    //FingerPrintScanner.Dispose();
                                    //FingerPrintScanner.Initialize();
                                    //await FingerPrintScanner.CaptureFingerPrintAsync();
                                    _fingerPrintScanner.StopFingerScanner();
                                    _fingerPrintScanner.InitializeFingerScanner();
                                    LoadDevices();
                                    _journal?.Initialized(DeviceType.FingerPrintScanner.ToString());
                                    await _channelManagementService.InsertEventAsync("Initialize Finger Print Scanner", "True");
                                    break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Exception(ex);
                        //MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                        x.Status = DeviceStatus.HwError;
                        ShowErrorMessage(x.Type);
                    }
                    finally
                    {
                        LoadDevices();
                        Context.DisplayProgress = false;
                    }
                }, x => true);
        }


        public ICommand Initialize { get; }
        public ICommand Reset { get; }

        private static readonly ICardReader CardReader = ServiceLocator.Instance.Resolve<ICardReader>();
        //private static readonly IChequeAcceptor CheckAcceptor = ServiceLocator.Instance.Resolve<IChequeAcceptor>();
        private static readonly ICashDispenser CashDispenser = ServiceLocator.Instance.Resolve<ICashDispenser>();
        //private static readonly ICoinDispenser CoinDispenser = ServiceLocator.Instance.Resolve<ICoinDispenser>();
        private static readonly ICashAcceptor CashAcceptor = ServiceLocator.Instance.Resolve<ICashAcceptor>();
        private static readonly IReceiptPrinter Printer = ServiceLocator.Instance.Resolve<IReceiptPrinter>();
        private static readonly IJournalPrinter JournalPrinter = ServiceLocator.Instance.Resolve<IJournalPrinter>();
        private static readonly IPinPad PinPad = ServiceLocator.Instance.Resolve<IPinPad>();
        private static readonly IFingerPrintScanner FingerPrintScanner = ServiceLocator.Instance.Resolve<IFingerPrintScanner>();

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                //RaisePropertyChanged(nameof(Message));
                RaisePropertyChanged("Message");
            }
        }

        private DeviceConfiguration[] _devices;

        public DeviceConfiguration[] Devices
        {
            get {
                //LoadDevices();
                return _devices;
            }
            set
            {
                _devices = value;
                //RaisePropertyChanged(nameof(Message));
                RaisePropertyChanged("Devices");
            }
        }

        void LoadDevices()
        {

            Devices = new[] {
            new DeviceConfiguration {
                Type = DeviceConfiguration.DeviceType.CardReader,
                Text = "Card Reader",
                Status = CardReader.Status
            },
            new DeviceConfiguration {
                Type = DeviceConfiguration.DeviceType.CashAcceptor,
                Text = "BRM",
                //Status = (  CashAcceptor.Status == DeviceStatus.Online)
                //            ? DeviceStatus.Online : DeviceStatus.NoDevice
                Status = CashAcceptor.Status
            },
            new DeviceConfiguration {
                Type = DeviceConfiguration.DeviceType.Cassettes,
                Text = "Cassettes",
                Status = (CashAcceptor.isCassettteChanged) ? DeviceStatus.ResetRequired:DeviceStatus.Online
            },
			/*new DeviceConfiguration
			{
				Type = DeviceConfiguration.DeviceType.CoinDispenser,
				Text = "Coin Dispenser",
				Status = CoinDispenser.Status
			},
			new DeviceConfiguration {
				Type = DeviceConfiguration.DeviceType.ChequeAcceptor,
				Text = "Check Acceptor",
				Status = CheckAcceptor.Status
			},*/
			new DeviceConfiguration {
                Type = DeviceConfiguration.DeviceType.PinPad,
                Text = "Pin Pad",
                Status = PinPad.Status
            },
            new DeviceConfiguration {
                Type = DeviceConfiguration.DeviceType.ReceiptPrinter,
                Text = "Receipt Printer",
                Status = Printer.Status
            },
            new DeviceConfiguration {
                Type = DeviceConfiguration.DeviceType.FingerPrintScanner,
                Text = "Finger Print Scanner",
                Status = _fingerPrintScanner.GetFingerPrintStatus()
            }
            };
        }
        private void CashAcceptor_CashAcceptorStatusChanged(object sender, string status)
        {
            Context.steps.Logger.Info("DeviceConfiguratio_CashAcceptorStatusChanged called with status: " + status);
            //Devices.Clear();
            LoadDevices();
        }
        private void CardReader_CardReaderStatusChanged(object sender, string e)
        {
            Context.steps.Logger.Info("DeviceConfiguratio_CardReaderStatusChanged called with status: " + e);
            //Devices.Clear();
            LoadDevices();
        }
        private void Printer_ReceiptPrinterDeviceStatusChanged(object sender, string e)
        {
            Context.steps.Logger.Info("DeviceConfiguratio_ReceiptPrinterDeviceStatusChanged called with status: " + e);
            //Devices.Clear();
            LoadDevices();
        }

        private void PinPad_PinPadStatusChanged(object sender, string e)
        {
            Context.steps.Logger.Info("DeviceConfiguratio_PinPad_PinPadStatusChanged called with status: " + e);
            //Devices.Clear();
            LoadDevices();
        }

        public void ShowErrorMessage(DeviceConfiguration.DeviceType Device)
        {

            var errorCode = "UNKNOWN";
            /*switch (Device)
            {
                case DeviceConfiguration.DeviceType.CardReader:
                    errorCode = CardReader.GetDeviceError(DeviceShortName.CRD)?.Code;
                    break;
                case DeviceConfiguration.DeviceType.CashAcceptor:
                    errorCode = CardReader.GetDeviceError(DeviceShortName.BRM)?.Code;
                    break;
                case DeviceConfiguration.DeviceType.CashDispenser:
                    errorCode = CardReader.GetDeviceError(DeviceShortName.BRM)?.Code;
                    break;
                case DeviceConfiguration.DeviceType.PinPad:
                    errorCode = CardReader.GetDeviceError(DeviceShortName.PINPAD)?.Code;
                    break;
                case DeviceConfiguration.DeviceType.ReceiptPrinter:
                    errorCode = CardReader.GetDeviceError(DeviceShortName.SPR)?.Code;
                    break;
                case DeviceConfiguration.DeviceType.FingerPrintScanner:
                    errorCode = FingerPrintScanner.GetFingerPrintErrorCode();
                    break;
                default:
                    errorCode = "UNKNOWN";
                    break;
            }*/

            Context.steps.Logger?.Info($"Device: {Device}");
            Errors.Clear();

            

            var path = @"SOFTWARE\ATM\ErrorCode";

            if (EnvironmentHelper.Is64BitOperatingSystem())
            {
                path = @"SOFTWARE\WOW6432Node\ATM\ErrorCode";
            }

            using (var reg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                foreach (var i in reg.OpenSubKey(path).GetValues())
                {
                    if (string.IsNullOrWhiteSpace(i.Key))
                        continue;

                    var s = i.Value as string;
                    if (string.IsNullOrWhiteSpace(s))
                        continue;

                    int x;
                    if (int.TryParse(s, out x) && x == 0)
                        continue;

                    /*Errors.Add(new DeviceError
                    {
                        Message = i.Key,
                        Code = s
                    });*/



                    Context.steps.Logger?.Info($"Error Key: {i.Key} And Code: {s}");
                    if (i.Key == "BRM" && (Device == DeviceConfiguration.DeviceType.CashAcceptor || Device == DeviceConfiguration.DeviceType.CashDispenser))
                    {
                        errorCode = s;
                        break;
                    }
                    else if (i.Key == "CRD" && Device == DeviceConfiguration.DeviceType.CardReader)
                    {
                        errorCode = s;
                        break;
                    }
                    else if (i.Key == "PINPAD" && Device == DeviceConfiguration.DeviceType.PinPad)
                    {
                        errorCode = s;
                        break;
                    }
                    else if (i.Key == "SPR" && Device == DeviceConfiguration.DeviceType.ReceiptPrinter)
                    {
                        errorCode = s;
                        break;
                    }
                    else if (i.Key == "FGR" && Device == DeviceConfiguration.DeviceType.FingerPrintScanner)
                    {
                        errorCode = s;
                        break;
                    }
                    //await _channelManagementService.InsertEventAsync("CIT: Clear Cash", "Step not followed");
                    //_channelManagementService.InsertEventAsync("DEVICE_ERROR_CODE", s);
                    //Logger.Info("DEVICE_ERROR_CODE: " + s);
                }
            }

            MessageBox.Show("Operation Failed with error code " + errorCode, "Error", MessageBoxButton.OK);


            Context.steps.Logger.Info("ShowErrorMessage called For Device");
        }
    }

}