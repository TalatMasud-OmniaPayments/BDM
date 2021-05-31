namespace Omnia.Pie.Vtm.Devices.Interface
{
    using System;
    using System.Threading.Tasks;
    using Omnia.Pie.Vtm.Devices.Interface.Entities;
    //using System.Threading.Tasks;

	public interface IFingerPrintScanner:IDevice
	{
        //Task<string> CaptureFingerPrintAsync();
        Task<String> CaptureFingerPrintAsync();
        void InitializeFingerScanner();
        void StopFingerScanner();
        DeviceStatus GetFingerPrintStatus();
        string GetFingerPrintErrorCode();

    }
}