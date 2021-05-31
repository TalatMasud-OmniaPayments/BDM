using Microsoft.Practices.Unity;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Supervisor.Shell.Utilities;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
//using Omnia.Pie.Vtm.Devices.FingerScanner;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Devices
{
    public class FingerPrintScannerViewModel : DeviceViewModel
    {
        public FingerPrintScannerViewModel()
        {
            Operations = new IOperationViewModel<DeviceViewModel, object>[] {
                new OperationViewModel<FingerPrintScannerViewModel, string>(this)
                {
                    Id = nameof(IFingerPrintScanner.CaptureFingerPrintAsync).ToHumanString(),
                    Execute = x => x.device.CaptureFingerPrintAsync(),
                    CanExecute = x => true
                }
            };

        }

        public override IDevice Device => device;
        readonly IFingerPrintScanner device = ServiceLocator.Instance.Resolve<IFingerPrintScanner>();
        public override string StatusText => "Online";
        public override string Id => "Finger Print Scanner";

        //public override IDevice Device => throw new System.NotImplementedException();

        /* readonly ICashAcceptor model = ServiceLocator.Instance.Resolve<ICashAcceptor>();
         public override IDevice Device => model;

         public override string Id => "Cash Acceptor";*/
    }
}
