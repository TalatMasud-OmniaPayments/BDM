using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.Common.Steps
{
    public class CheckPinpadStep : WorkflowStep
    {
        IPinPad _Pinpad;
        TaskCompletionSource<bool> completionSource;

        public CheckPinpadStep(IResolver container) : base(container)
        {
            _Pinpad = container.Resolve<IPinPad>();
        }

        public async Task<bool> CheckPinpadAsync()
        {
            
            _logger?.Info($"Execute Step: Check Pinpad");
            _logger?.Info($"Pinpad Status: {_Pinpad.Status}");
            _logger?.Info($"Pinpad Is Available? {_Pinpad.IsAvailable}");

            if (_Pinpad.Status != DeviceStatus.Online || !_Pinpad.IsAvailable)
            {
                LoadWaitScreen();
                await _Pinpad.ResetAsync();
                //_Pinpad.Initialize();
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
