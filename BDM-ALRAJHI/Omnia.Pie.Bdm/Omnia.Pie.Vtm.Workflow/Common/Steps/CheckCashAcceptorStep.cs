using Omnia.Pie.Vtm.Bootstrapper.Interface.ViewModels.Common;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.Common.Steps
{
    public class CheckCashAcceptorStep : WorkflowStep
    {
        ICashAcceptor _cashAcceptor;
        ICashDispenser _cashDispenser;
        TaskCompletionSource<bool> completionSource;

        public CheckCashAcceptorStep(IResolver container) : base(container)
        {
            _cashAcceptor = container.Resolve<ICashAcceptor>();
            _cashDispenser = container.Resolve<ICashDispenser>();
            
        }

        public async Task<bool> CheckCashAcceptorAsync()
        {
            _logger?.Info($"Execute Step: Check Cash Acceptor");
            _logger?.Info($"Cash Acceptor Status: {_cashAcceptor.Status}");
            _logger?.Info($"Cash Acceptor Is Available? {_cashAcceptor.IsAvailable}");

            

            if (_cashAcceptor.Status != DeviceStatus.Online || !_cashAcceptor.IsAvailable)
            {

                LoadWaitScreen();
                await _cashAcceptor.ResetAsync();
                //_cashAcceptor.Initialize();
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
