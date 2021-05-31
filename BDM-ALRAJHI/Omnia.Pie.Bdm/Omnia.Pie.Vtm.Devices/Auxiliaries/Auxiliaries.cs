using AxNXAuxiliariesXLib;
using Omnia.Pie.Client.Journal.Interface;
using Omnia.Pie.Client.Journal.Interface.Extension;
using Omnia.Pie.Vtm.DataAccess.Interface;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
using Omnia.Pie.Vtm.Framework.Interface;
using System;
using System.Windows.Forms;

namespace Omnia.Pie.Vtm.Devices.Auxiliaries
{
	public class Auxiliaries : Device, IAuxiliaries
	{
		public Auxiliaries(IDeviceErrorStore deviceErrorStore, IGuideLights guideLights, ILogger logger, IJournal journal)
			: base(deviceErrorStore, logger, journal, guideLights)
		{
			Logger.Info("Auxiliaries Initialized");
		}

		AxNXAuxiliariesX ax;
		protected override AxHost CreateAx() => ax = new AxNXAuxiliariesX();
		protected override int CloseSessionSync() => ax.CloseSessionSync();
		protected override int OpenSessionSync(int timeout) => ax.OpenSessionSync(timeout);
		protected override string GetDeviceStatus() => ax.DeviceStatus;
		public event EventHandler PowerFailure;
		public event EventHandler PoweredUp;

		protected override void OnInitialized()
		{
			ax.UPSChanged += Ax_UPSChanged;
			ax.ControlUPSComplete += Ax_ControlUPSComplete;
			ax.DeviceError += Ax_DeviceError;
			ax.FatalError += Ax_FatalError;
		}

		private void Ax_ControlUPSComplete(object sender, EventArgs e)
		{
			Logger.Info($"Ax_ControlUPSComplete");
		}

		private void Ax_FatalError(object sender, _DNXAuxiliariesXEvents_FatalErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_DeviceError(object sender, _DNXAuxiliariesXEvents_DeviceErrorEvent e)
		{
			OnError(new DeviceMalfunctionException(e.action, e.result));
		}

		private void Ax_UPSChanged(object sender, _DNXAuxiliariesXEvents_UPSChangedEvent e)
		{
			Logger.Info($"Ax_UPSChanged e.State : {e.state} e.value : {e.value}");

			if (e.state == "POWERING" && e.value == -1)
			{
				Journal.PowerFailure();
				PowerFailure?.Invoke(sender, EventArgs.Empty);
			}
			else if (e.state == "POWERING" && e.value == 0)
			{
				Journal.PoweredUp();
				PoweredUp?.Invoke(sender, EventArgs.Empty);
			}
		}

		protected override void OnDisposing()
		{
			ax.UPSChanged -= Ax_UPSChanged;
			ax.ControlUPSComplete -= Ax_ControlUPSComplete;
			ax.DeviceError -= Ax_DeviceError;
			ax.FatalError -= Ax_FatalError;
		}
	}
}