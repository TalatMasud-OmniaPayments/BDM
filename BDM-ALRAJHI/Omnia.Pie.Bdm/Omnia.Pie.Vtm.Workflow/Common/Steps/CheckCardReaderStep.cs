using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Workflow.Common.Steps
{
    public class CheckCardReaderStep : WorkflowStep
    {
        ICardReader _cardReader;

        public CheckCardReaderStep(IResolver container) : base(container)
        {
            _cardReader = container.Resolve<ICardReader>();
        }

        public async Task<bool> CheckCardReaderAsync()
        {
            _logger?.Info($"Execute Step: Check Card Reader");
            _logger?.Info($"Check Card Reader Status: {_cardReader.Status}");
            _logger?.Info($"Check Card Reader Is Available? {_cardReader.IsAvailable}");

            if (_cardReader.Status != DeviceStatus.Online || !_cardReader.IsAvailable)
            {
                LoadWaitScreen();
                await _cardReader.ResetAsync();
                //_cardReader.Initialize();
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
