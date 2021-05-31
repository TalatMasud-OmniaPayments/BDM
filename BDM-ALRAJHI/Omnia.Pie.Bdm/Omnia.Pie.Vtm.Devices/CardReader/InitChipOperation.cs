namespace Omnia.Pie.Vtm.Devices.Emv
{
	using AxNXCardReaderXLib;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Threading.Tasks;

	internal class InitChipOperation
	{
		private readonly AxNXCardReaderX _cardReaderCom;
		private readonly ILogger _logger;
		private readonly TaskCompletionSource<bool> _completion;

		public InitChipOperation(AxNXCardReaderX cardReaderCom, ILogger logger)
		{
			_cardReaderCom = cardReaderCom;
			_logger = logger;
			_completion = new TaskCompletionSource<bool>(TaskContinuationOptions.RunContinuationsAsynchronously);
		}

		public async Task<bool> ExecuteAsync()
		{
			try
			{
				_cardReaderCom.ChipIOComplete += CardReader_ChipIOComplete;
				_cardReaderCom.ChipIOFailure += CardReaderCom_ChipIOFailure;
				_cardReaderCom.InvalidMedia += CardReaderCom_InvalidMedia;
				_cardReaderCom.Timeout += CardReaderCom_Timeout;

				int deviceResult = _cardReaderCom.ChipIO(0, "INIT", 0, Timeout.Operation);
				if (deviceResult != DeviceResult.Ok)
				{
					throw new DeviceMalfunctionException("ChipIO", deviceResult);
				}

				bool result = await _completion.Task;
				return result;
			}
			finally
			{
				_cardReaderCom.ChipIOComplete -= CardReader_ChipIOComplete;
				_cardReaderCom.ChipIOFailure -= CardReaderCom_ChipIOFailure;
				_cardReaderCom.InvalidMedia -= CardReaderCom_InvalidMedia;
				_cardReaderCom.Timeout -= CardReaderCom_Timeout;
			}
		}

		private void CardReaderCom_ChipIOFailure(object sender, _DNXCardReaderXEvents_ChipIOFailureEvent e)
		{
			_completion.TrySetException(new DeviceMalfunctionException("ChipIO", $"ChipIOFailure(e.token={e.token})"));
		}

		private void CardReader_ChipIOComplete(object sender, _DNXCardReaderXEvents_ChipIOCompleteEvent e)
		{
			_completion.TrySetResult(true);
		}

		private void CardReaderCom_InvalidMedia(object sender, EventArgs e)
		{
			_completion.TrySetException(new DeviceMalfunctionException("ChipIO", "InvalidMedia"));
		}

		private void CardReaderCom_Timeout(object sender, EventArgs e)
		{
			_completion.TrySetException(new DeviceTimeoutException($"ChipIO"));
		}
	}
}