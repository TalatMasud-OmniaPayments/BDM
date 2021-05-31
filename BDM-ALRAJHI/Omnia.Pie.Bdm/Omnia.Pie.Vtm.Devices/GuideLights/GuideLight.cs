namespace Omnia.Pie.Vtm.Devices.GuideLights
{
	using System;
	using AxNXGuidLightsXLib;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Framework.Interface;

	internal sealed class GuideLight : IGuideLight
	{
		private readonly AxNXGuidLightsX _guideLightsDevice;
		private readonly string _deviceName;
		private readonly ILogger _logger;

		public GuideLight(ILogger logger, AxNXGuidLightsX guideLightsDevice, string deviceName)
		{
			_guideLightsDevice = guideLightsDevice;
			_deviceName = deviceName;
			_logger = logger;
		}

		public void TurnOn()
		{
			SetFlashRate("MEDIUM");
		}

		public void TurnOff()
		{
			SetFlashRate("OFF");
		}

		private void SetFlashRate(string flashRate)
		{
			try
			{
				int deviceResult = _guideLightsDevice.SetGuidLight(_deviceName, flashRate);
				if (deviceResult != DeviceResult.Ok)
				{
					_logger.Error($"SetGuidLight operation has failed with result={deviceResult}.");
				}
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
			}
		}
	}
}