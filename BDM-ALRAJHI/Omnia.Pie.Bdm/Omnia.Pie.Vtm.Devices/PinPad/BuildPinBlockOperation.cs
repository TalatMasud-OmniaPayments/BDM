namespace Omnia.Pie.Vtm.Devices.PinPad
{
	using AxNXPinXLib;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using System.Configuration;
	using System.Threading.Tasks;

	internal class BuildPinBlockOperation
	{
		private readonly AxNXPinX _pinPadDevice;
		private readonly TaskCompletionSource<string> _completion;

		public BuildPinBlockOperation(AxNXPinX pinPadDevice)
		{
			_pinPadDevice = pinPadDevice;
			_completion = new TaskCompletionSource<string>();
		}

		public async Task<string> ExecuteAsync(Card card)
		{
			var track2Data = card.Track2.Replace("D", "=").Split('=');
			var pan = track2Data[0].Replace("F", "").Replace("?", "");
			var formattedPan = pan.Substring(pan.Length - 12 - 1, 12);

			try
			{
				_pinPadDevice.PinBlockComplete += PinPadDevice_PinBlockComplete;
				_pinPadDevice.DeviceError += PinPadDevice_DeviceError;
				_pinPadDevice.FatalError += PinPadDevice_FatalError;

				int deviceResult = _pinPadDevice.BuildPinBlock(formattedPan, string.Empty, 0xF, "ISO0", ConfigurationManager.AppSettings["ISOPinKey"].ToString(), ConfigurationManager.AppSettings["ISOMasterKey"].ToString());
				if (deviceResult != 0)
				{
					throw new DeviceMalfunctionException("BuildPinBlock", deviceResult);
				}

				string pinBlock = await _completion.Task;
				return pinBlock;
			}
			finally
			{
				_pinPadDevice.PinBlockComplete -= PinPadDevice_PinBlockComplete;
				_pinPadDevice.DeviceError -= PinPadDevice_DeviceError;
				_pinPadDevice.FatalError -= PinPadDevice_FatalError;
			}
		}

		public async Task<string> ExecuteDieboldAsync(Card card)
		{
			var track2Data = card.Track2.Replace("D", "=").Split('=');
			var pan = track2Data[0].Replace("F", "").Replace("?", "");
			var formattedPan = pan.Substring(pan.Length - 12 - 1, 12);

			try
			{
				_pinPadDevice.PinBlockComplete += PinPadDevice_PinBlockComplete;
				_pinPadDevice.DeviceError += PinPadDevice_DeviceError;
				_pinPadDevice.FatalError += PinPadDevice_FatalError;

				int deviceResult = _pinPadDevice.BuildPinBlock(formattedPan, string.Empty, 0xF, "DIEBOLD", ConfigurationManager.AppSettings["HPSPinKey"].ToString(), ConfigurationManager.AppSettings["HPSMasterKey"].ToString());

				if (deviceResult != 0)
				{
					throw new DeviceMalfunctionException("BuildPinBlock", deviceResult);
				}

				string pinBlock = await _completion.Task;
				return pinBlock;
			}
			finally
			{
				_pinPadDevice.PinBlockComplete -= PinPadDevice_PinBlockComplete;
				_pinPadDevice.DeviceError -= PinPadDevice_DeviceError;
				_pinPadDevice.FatalError -= PinPadDevice_FatalError;
			}
		}

		public async Task<string> ExecuteDieboldAsync(string card)
		{
			var formattedPan = card;

			try
			{
				_pinPadDevice.PinBlockComplete += PinPadDevice_PinBlockComplete;
				_pinPadDevice.DeviceError += PinPadDevice_DeviceError;
				_pinPadDevice.FatalError += PinPadDevice_FatalError;

				int deviceResult = _pinPadDevice.BuildPinBlock(formattedPan, string.Empty, 0xF, "DIEBOLD", ConfigurationManager.AppSettings["HPSPinKey"].ToString(), ConfigurationManager.AppSettings["HPSMasterKey"].ToString());
				if (deviceResult != 0)
				{
					throw new DeviceMalfunctionException("BuildPinBlock", deviceResult);
				}

				string pinBlock = await _completion.Task;
				return pinBlock;
			}
			finally
			{
				_pinPadDevice.PinBlockComplete -= PinPadDevice_PinBlockComplete;
				_pinPadDevice.DeviceError -= PinPadDevice_DeviceError;
				_pinPadDevice.FatalError -= PinPadDevice_FatalError;
			}
		}

		public async Task<string> ExecuteAsync(string formattedPan)
		{
			try
			{
				_pinPadDevice.PinBlockComplete += PinPadDevice_PinBlockComplete;
				_pinPadDevice.DeviceError += PinPadDevice_DeviceError;
				_pinPadDevice.FatalError += PinPadDevice_FatalError;

				int deviceResult = _pinPadDevice.BuildPinBlock(formattedPan, string.Empty, 0xF, "ISO0", ConfigurationManager.AppSettings["ISOPinKey"].ToString(), ConfigurationManager.AppSettings["ISOMasterKey"].ToString());
				if (deviceResult != 0)
				{
					throw new DeviceMalfunctionException("BuildPinBlock", deviceResult);
				}

				string pinBlock = await _completion.Task;
				return pinBlock;
			}
			finally
			{
				_pinPadDevice.PinBlockComplete -= PinPadDevice_PinBlockComplete;
				_pinPadDevice.DeviceError -= PinPadDevice_DeviceError;
				_pinPadDevice.FatalError -= PinPadDevice_FatalError;
			}
		}

		private void PinPadDevice_FatalError(object sender, _DNXPinXEvents_FatalErrorEvent e)
		{
			_completion.TrySetException(new DeviceMalfunctionException(e.action, e.result));
		}

		private void PinPadDevice_DeviceError(object sender, _DNXPinXEvents_DeviceErrorEvent e)
		{
			_completion.TrySetException(new DeviceMalfunctionException(e.action, e.result));
		}

		private void PinPadDevice_PinBlockComplete(object sender, _DNXPinXEvents_PinBlockCompleteEvent e)
		{
			_completion.TrySetResult(e.pinBlock);
		}
	}
}