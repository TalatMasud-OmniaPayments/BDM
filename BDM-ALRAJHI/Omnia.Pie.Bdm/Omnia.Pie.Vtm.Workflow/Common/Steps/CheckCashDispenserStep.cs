using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.Common;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.Common.Steps
{
    public class CheckCashDispenserStep : WorkflowStep
    {
        ICashDispenser _cashDispenser;
        TaskCompletionSource<bool> completionSource;

        public CheckCashDispenserStep(IResolver container) : base(container)
        {
            _cashDispenser = container.Resolve<ICashDispenser>();
        }

        public async Task<bool> CheckCashDispenserAsync()
        {
            
            _logger?.Info($"Execute Step: Check Cash Dispenser");

            _logger?.Info($"Cash Dispenser Status: {_cashDispenser.Status}");
            _logger?.Info($"Cash Dispenser Is Available? {_cashDispenser.IsAvailable}");

            if (_cashDispenser.Status != DeviceStatus.Online || !_cashDispenser.IsAvailable)
            {
                LoadWaitScreen();
                await _cashDispenser.ResetAsync();
                //_cashDispenser.Initialize();

                return true;
            }
            else
            {
                return false;
            }
            
        }

        public override void Dispose()
        {

        }
    }
}
