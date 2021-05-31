using Omnia.Pie.Vtm.Devices.Interface;
using System.Windows.Media.Imaging;
using Omnia.Pie.Client.Journal.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Constants;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Omnia.Pie.Vtm.DataAccess.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Framework.ControlExtenders;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omnia.Pie.Vtm.Devices.FingerScanner
{
    public class FingerPrintScannerStub : Device, IFingerPrintScanner
    {
        public FingerPrintScannerStub(IDeviceErrorStore deviceErrorStore, ILogger logger, IJournal journal,
            IGuideLights guideLights) : base(deviceErrorStore, logger, journal, guideLights)
        {
        }

        public Task<string> CaptureFingerPrintAsync() => null;
        //readonly BitmapSource image = BitmapExtender.LoadImageFromExecutablePath("sign.bmp");

        protected override AxHost CreateAx() => null;
        protected override int CloseSessionSync() => DeviceResult.Ok;
        protected override int OpenSessionSync(int timeout) => DeviceResult.Ok;
        protected override string GetDeviceStatus() => null;

        void IFingerPrintScanner.InitializeFingerScanner()
        {

        }
        void IFingerPrintScanner.StopFingerScanner()
        {

        }

        public DeviceStatus GetFingerPrintStatus()
        {
            //throw new System.NotImplementedException();
            return DeviceStatus.Unknown;
        }

        public string GetFingerPrintErrorCode()
        {
            return "";
        }
    }
}
