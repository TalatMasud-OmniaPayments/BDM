using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.PIE.VTA.ViewModels;

namespace Omnia.PIE.VTA.Core.Model
{
	public class Device : BaseViewModel
	{
		private string _DeviceName;
		public string DeviceName
		{
			get { return _DeviceName; }
			set
			{
				if (value != _DeviceName)
				{
					_DeviceName = value;
					OnPropertyChanged(() => DeviceName);
				}
			}
		}

		private string _DeviceStatus = nameof(RTDeviceStatus.Offline).ToUpper();
		public string DeviceStatus
		{
			get { return _DeviceStatus; }
			set
			{
				if (value != _DeviceStatus)
				{
					_DeviceStatus = value;
					OnPropertyChanged(() => DeviceStatus);
				}
			}
		}

		private string _DeviceStatusImage;
		public string DeviceStatusImage
		{
			get { return _DeviceStatusImage; }
			set
			{
				if (value != _DeviceStatusImage)
				{
					_DeviceStatusImage = value;
					OnPropertyChanged(() => DeviceStatusImage);
				}
			}
		}
	}

	public struct JSONData
	{
		public string Data;
		public string DataLength;
		public string SenderID;
		public string StatusCode;
	}
}
