namespace Omnia.Pie.Vtm.Devices.FingerScanner
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using AxNXFINGERSCANNERXLib;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
    using Omnia.Pie.Vtm.Devices.Interface.Entities;
    using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
    using Omnia.Pie.Vtm.Framework.ControlExtenders;
    using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Devices.Interface.Enum;
    using AxInterop.NITGENFINGERLib;
    using System.IO;

    public class FingerPrintScanner : Device, IFingerPrintScanner
	{

        

        public FingerPrintScanner(IGuideLights guideLights, IDeviceErrorStore deviceErrorStore,  ILogger logger, IJournal journal)
            : base(deviceErrorStore, logger, journal, guideLights)
        {
            Operations.AddRange(new DeviceOperation[]
            {
                
                FingerPrintOperation = new DeviceOperation<FingerPrint>(nameof(FingerPrintOperation), logger, journal)
            });
            

            Logger.Info("FingerPrintScanner Initialized");
            //CreateAx();
        }


        internal readonly DeviceOperation<FingerPrint> FingerPrintOperation;
        /*internal readonly DeviceOperation<Card> ReadCardOperation;
        //internal readonly ReadChipDataOperation ReadChipDataOperation;
        internal readonly DeviceOperation<bool> CancelReadCardOperation;
        internal readonly DeviceOperation<bool> EjectCardAndWaitTakenOperation;
        internal readonly DeviceOperation<bool> RetainCardOperation;
        internal readonly DeviceOperation<byte[]> ChipIOOperation;*/

        NitGenFinger ax;
        TaskCompletionSource<bool> tcs = null;
        string image;


        //protected override AxHost CreateAx() => ax = new AxNXFingerScannerX();
        protected override AxHost CreateAx()
        {
            InitializeFingerScanner();


            return null;
        }

        protected override int CloseSessionSync()
        {
            Logger.Info("FingerPrintScanner CloseSessionSync");

            //Logger.Info($"{GetType()} => FingerPrintScanner OpenSessionSync GetType");

            ax.CloseConnection();
            return 1;
        }
        //protected override int OpenSessionSync(int timeout) => ax.OpenSessionSync(timeout);
        protected override int OpenSessionSync(int timeout)
        {
            Logger.Info("FingerPrintScanner OpenSessionSync");

            //Logger.Info($"{GetType()} => FingerPrintScanner OpenSessionSync GetType");
            bool connection = ax.OpenConnection();
            return Convert.ToInt32(connection);
        }
        public string GetFingerPrintErrorCode()
        {
            //"9781010:Fail to open a device"
            Console.WriteLine("FingerPrintScanner GetFingerPrintErrorCode");
            var errorMessage = ax.GetLastErrorMsg();
            string[] error = errorMessage.Split(':');
            var errorCode = "";

            if (error != null && error.Length > 0)
            {
                errorCode = error[0];
                if (errorCode == "0000000")
                {
                    errorCode = "";
                }
            }
                return errorCode;
        }
        //protected override string GetDeviceStatus() => ax.DeviceStatus;
        public DeviceStatus GetFingerPrintStatus()
        {
            //"9781010:Fail to open a device"
            Console.WriteLine("FingerPrintScanner GetDeviceStatus");

             if (ax.OpenConnection())
            {
                return DeviceStatus.Online;

            }
            else
            {
                return DeviceStatus.Offline;
            }
            
        }

        /*public DeviceStatus GetFingerPrintDeviceStatus(string errorMessage)
        {

            var status = DeviceStatus.Offline;

            string[] error = errorMessage.Split(':');
            var errorCode = "";

            if (error != null && error.Length > 0)
            {
                errorCode = error[0];

                if (errorCode != null)
                {

                    Logger.Info("Device Status:" + errorCode);
                    switch (errorCode.ToUpper())
                    {
                        case "0000000":
                            status = DeviceStatus.Online;
                            break;
                        case "9781058":
                            status = DeviceStatus.Offline;
                            break;
                        case "9781050":
                            status = DeviceStatus.Offline;
                            break;
                        case "9781040":
                            status = DeviceStatus.Offline;
                            break;
                        case "9781022":
                            status = DeviceStatus.Offline;
                            break;
                        case "9781020":
                            status = DeviceStatus.Offline;
                            break;
                        case "9781018":
                            status = DeviceStatus.Offline;
                            break;
                        case "9781011":
                            status = DeviceStatus.Offline;
                            break;
                        case "9781010":
                            status = DeviceStatus.Offline;
                            break;
                        default:
                            status = DeviceStatus.Unknown;
                            break;
                    }

                    return status;
                }
            }

            

            return status;
            
        }*/
        #region Overridden Functions

        protected override string GetDeviceStatus()
        {
            if (GetFingerPrintStatus() == DeviceStatus.Online)
            {
                return "Online";
            }
            else
            {
                return "Offline";
            }
            
        }
        protected override void OnInitialized()
        {

            Logger.Info("FingerPrintScanner OnInitialized");
            Console.WriteLine("FingerPrintScanner OnInitialized");
            ax.AcquireFinished += Ax_AcquireFinished1; ;
            ax.DeviceError += Ax_DeviceError; ;
            ax.AcquireStopped += Ax_AcquireStopped; ;
            ax.DeviceStatusChanged += Ax_DeviceStatusChanged; ;
        }
        protected override void OnDisposing()
        {
            Console.WriteLine("FingerPrintScanner OnDisposing");
            //Console.WriteLine("{0}: {1}", handle, title);
            ax.AcquireFinished -= Ax_AcquireFinished1;
            ax.DeviceError -= Ax_DeviceError;
            ax.AcquireStopped -= Ax_AcquireStopped;
            ax.DeviceStatusChanged -= Ax_DeviceStatusChanged;
        }

        private void Ax_DeviceStatusChanged(int DeviceStatus)
        {
            Console.WriteLine("FingerPrintScanner DeviceStatus Event");


            //Status = DeviceStatus.ToString();
        }

        private void Ax_AcquireStopped()
        {
            Console.WriteLine("FingerPrintScanner Ax_AcquireStopped Event");

        }

        private void Ax_DeviceError()
        {
            //"9781010:Device is not connected"
            Console.WriteLine("FingerPrintScanner Ax_DeviceError Event");
            var errorMessage = ax.GetLastErrorMsg();
            OnError(new DeviceMalfunctionException(errorMessage));

        }

        private void Ax_AcquireFinished1(string strReturn, string strTransData)
        {
            //Console.WriteLine("FingerPrintScanner Ax_AcquireFinished Event");
            //image = strTransData;
            Logger.Info("FingerPrintScanner Ax_AcquireFinished1");
            GetImage();
            

        }

        private void GetImage()
        {

            var osVersion = GetOSVersion();
            //Hyosung\Nextware\C:\Program Files\Nextware\Exe
            string path = "C:\\Hyosung\\Nextware\\Exe\\FingerImage.wsq";

            if (osVersion == "Windows 7") { 
                path = "C:\\Program Files\\Nextware\\Exe\\FingerImage.wsq";
            }
            path = "C:\\Program Files\\Nextware\\Exe\\FingerImage.wsq";

            try
            {
                byte[] imageArray = File.ReadAllBytes(path);
                image = Convert.ToBase64String(imageArray);
                tcs?.TrySetResult(true);
            }
            catch (Exception ex)
            {
                Console.Write("There is some error, please scan finger again");
                //Button_Click_1(null, null);
                ReScanFinger();
            }
        }
        async Task ReScanFinger()
        {
            await Task.Delay(1000);
            Logger.Info("FingerPrintScanner ReCaptureFingerPrint");
            ax.AcquireFinger();

        }
        #endregion
        #region Public Functions & Events
        /*public Task<FingerPrint> CaptureFingerPrintAsync()
        {
            throw new System.NotImplementedException();
        }*/

        public async Task<String> CaptureFingerPrintAsync()
        {
            Logger.Info("FingerPrintScanner CaptureFingerPrintAsync");
            //OpenSessionSync(30);
            ax.AcquireFinger();
            Logger.Info("FingerPrintScanner ax.AcquireFinger() complete");
            tcs = null;
            tcs = new TaskCompletionSource<bool>();
            Logger.Info("FingerPrintScanner tcs task going in await()");
            await tcs.Task;

            //Logger.Info($"Image: {image}");
            Logger.Info("Image: finger scanned successfully");
            return image;
        }
        public void InitializeFingerScanner()
        {
            ax = new NitGenFinger();
            //Console.WriteLine("FingerPrintScanner CreateAx");
            ax.ShowAcquireWindow("FALSE");
            //ax.SetAcquireWindowSize(0, 0, 400, 500);
            OnInitialized();
            
           
        }
        public void StopFingerScanner()
        {
            ax.StopAcquireFinger();
            CloseSessionSync();
        }


        public override Task ResetAsync()
        {
            return ResetOperation.StartAsync();
        }

        public string GetOSVersion()
        {
            //var osVersion = Environment.Version;
            int _MajorVersion = Environment.OSVersion.Version.Major;

            switch (_MajorVersion)
            {
                case 5:
                    return "Windows XP";
                case 6:
                    switch (Environment.OSVersion.Version.Minor)
                    {
                        case 0:
                            return "Windows Vista";
                        case 1:
                            return "Windows 7";
                        default:
                            return "Windows Vista & above";
                    }
                default:
                    return "Unknown";
            }
        }

        
        #endregion
    }


}