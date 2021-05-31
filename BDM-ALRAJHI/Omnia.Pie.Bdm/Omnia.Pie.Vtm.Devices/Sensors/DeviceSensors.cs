using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxNXSignpadScannerXLib;
using Omnia.Pie.Client.Journal.Interface;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Devices.Interface.Constants;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
using Omnia.Pie.Vtm.DataAccess.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Framework.ControlExtenders;
using Omnia.Pie.Vtm.Devices.Interface.Enum;
using AxNXSensorsXLib;

namespace Omnia.Pie.Vtm.Devices.Sensors
{
    public class DeviceSensors : Device, IDeviceSensors
    {
        public DeviceSensors(IDeviceErrorStore deviceErrorStore, IGuideLights guideLights, ILogger logger, IJournal journal)
            : base(deviceErrorStore, logger, journal, guideLights)
        {

            Logger.Info("Sensors Initialized");
        }

        AxNXSensorsX ax;
        public event EventHandler <SensorsStatus> OperatorStatusChanged;

        protected override AxHost CreateAx()
        {
            return ax = new AxNXSensorsX();
        }

        protected override int CloseSessionSync() => ax.CloseSessionSync();
        protected override int OpenSessionSync(int timeout) => ax.OpenSessionSync(timeout);
        protected override string GetDeviceStatus() => ax.DeviceStatus;

        protected override void OnInitialized()
        {

            ax.OperatorSwitchChanged += Ax_OperatorSwitchChanged;
            ax.DeviceStatusChanged += Ax_DeviceStatusChanged;
            ax.FatalError += Ax_FatalError; ;
            ax.XFSEvent += Ax_XFSEvent;
            ax.DeviceError += Ax_DeviceError;
            ax.ResetComplete += Ax_ResetComplete;
        }

        public SensorsStatus GetOperatorStatus()
        {
            return getSensorsStatus(ax.OperatorSwitchStatus);

        }
        private SensorsStatus getSensorsStatus(string devStatus)
        {
            SensorsStatus status = SensorsStatus.Run;

            switch (devStatus.ToUpper())
            {
                case "NOTAVAILABLE":
                    status = SensorsStatus.NotAvailable;
                    break;
                case "RUN":
                    status = SensorsStatus.Run;
                    break;
                case "MAINTENANCE":
                    status = SensorsStatus.Maintenance;
                    break;
                default:
                    status = SensorsStatus.Supervisor;
                    break;
            }

            return status;
        }
        private void Ax_XFSEvent(object sender, _DNXSensorsXEvents_XFSEventEvent e)
        {
            Logger.Info($"Ax_XFSEvent Type: {e.eventType} and CommandCode: {e.commandCode} and hResult: {e.hResult}");
        }

        
        private void Ax_DeviceStatusChanged(object sender, _DNXSensorsXEvents_DeviceStatusChangedEvent e)
        {
            Logger.Info($"Ax_DeviceStatusChanged: {e.value}");
        }

        private void Ax_OperatorSwitchChanged(object sender, _DNXSensorsXEvents_OperatorSwitchChangedEvent e)
        {
            Logger.Info($"Ax_OperatorSwitchChanged: {e.status}");
            SensorsStatus status = getSensorsStatus(e.status);
            OperatorStatusChanged?.Invoke(this, status);
        }

        protected override void OnDisposing()
        {
            ax.OperatorSwitchChanged -= Ax_OperatorSwitchChanged;
            ax.DeviceStatusChanged -= Ax_DeviceStatusChanged;
            ax.FatalError -= Ax_FatalError; ;
            ax.XFSEvent -= Ax_XFSEvent;
            ax.DeviceError -= Ax_DeviceError;
            ax.ResetComplete -= Ax_ResetComplete;
        }

        void Ax_FatalError(object sender, _DNXSensorsXEvents_FatalErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));
        void Ax_DeviceError(object sender, _DNXSensorsXEvents_DeviceErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));
        void Ax_ResetComplete(object sender, EventArgs e) => ResetOperation.Stop(true);
        //public override Task ResetAsync() => ResetOperation.StartAsync(() => ax.Reset(PrinterScannerResetAction.EJECT.ToString(), 0));
    }
}