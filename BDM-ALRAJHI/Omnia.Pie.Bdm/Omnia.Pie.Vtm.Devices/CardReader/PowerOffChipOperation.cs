namespace Omnia.Pie.Vtm.Devices.Emv
{
	using AxNXCardReaderXLib;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Threading.Tasks;

	internal class PowerOffChipOperation
	{
		private readonly ILogger _logger;
		private readonly AxNXCardReaderX _cardReaderCom;
		private readonly TaskCompletionSource<bool> _completion;

		public PowerOffChipOperation(AxNXCardReaderX cardReaderCom, ILogger logger)
		{
			_cardReaderCom = cardReaderCom;
			_logger = logger;
			_completion = new TaskCompletionSource<bool>(TaskContinuationOptions.RunContinuationsAsynchronously);
		}

		public async Task ExecuteAsync()
		{
			try
			{
				_cardReaderCom.ChipPowerComplete += CardReaderCom_ChipPowerComplete;
				_cardReaderCom.InvalidMedia += CardReaderCom_InvalidMedia;

				int deviceResult = _cardReaderCom.ChipPower("OFF");
				if (deviceResult != DeviceResult.Ok)
				{
					throw new DeviceMalfunctionException("ChipPower", deviceResult);
				}

				await _completion.Task;
			}
			finally
			{
				_cardReaderCom.ChipPowerComplete -= CardReaderCom_ChipPowerComplete;
				_cardReaderCom.InvalidMedia -= CardReaderCom_InvalidMedia;
			}
		}

		private void CardReaderCom_ChipPowerComplete(object sender, EventArgs e)
		{
			_completion.TrySetResult(true);
		}

		private void CardReaderCom_InvalidMedia(object sender, EventArgs e)
		{
			_completion.TrySetException(new DeviceMalfunctionException("ChipIO", "InvalidMedia"));
		}
	}
}