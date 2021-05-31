using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Omnia.PIE.VTA.Common
{
	public static class AudioSwitcherWrapper
	{
		private static List<Guid> _mutedDeviceIds;

		public static async Task MuteAll()
		{
			try
			{
				var audioController = new CoreAudioController();
				var devices = await audioController.GetDevicesAsync(DeviceType.Capture, DeviceState.Active);
				if (devices == null)
					return;

				var mutedDevices = devices.Where(x => !x.IsMuted).ToList();
				if (mutedDevices.Count == 0)
					return;

				if (_mutedDeviceIds == null)
					_mutedDeviceIds = new List<Guid>();
				else
					_mutedDeviceIds.Clear();

				foreach (var device in mutedDevices)
				{
					await device.MuteAsync(true);
					_mutedDeviceIds.Add(device.Id);
				}

			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}

		public static async Task UnMuteAll()
		{
			try
			{
				if (_mutedDeviceIds == null || _mutedDeviceIds.Count == 0)
					return;

				var audioController = new CoreAudioController();
				var devices = await audioController.GetDevicesAsync(DeviceType.Capture, DeviceState.Active);
				if (devices == null)
					return;

				foreach (var device in devices)
				{
					if (_mutedDeviceIds.Contains(device.Id) && device.IsMuted)
						await device.MuteAsync(false);
				}
				_mutedDeviceIds.Clear();
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}
	}
}