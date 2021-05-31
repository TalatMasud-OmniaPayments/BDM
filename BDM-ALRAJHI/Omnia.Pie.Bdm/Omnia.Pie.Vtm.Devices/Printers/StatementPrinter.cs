namespace Omnia.Pie.Vtm.Devices.StatementPrinter
{
	using System;
	using System.IO;
	using System.Text;
	using System.Windows.Forms;
	using AxNXStatementPrinterXLib;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Enum;
	using System.Threading.Tasks;

	public class StatementPrinter : PrinterScanner, IStatementPrinter
	{
		public StatementPrinter(IDeviceErrorStore deviceErrorStore, ILogger logger, IGuideLights guideLights)
			: base(deviceErrorStore, logger, null, guideLights)
		{
			Logger.Info("StatementPrinter Initialized");
		}

		// should be GuideLights.DocumentPrinter wires are not connected properly switched DocumentPrinter/Scanner
		protected override IGuideLight GuideLight => GuideLights.Scanner;

		AxNXStatementPrinterX ax;
		protected override AxHost CreateAx() => ax = new AxNXStatementPrinterX();
		protected override int OpenSessionSync(int timeout) => ax.OpenSessionSync(timeout);
		protected override int CloseSessionSync() => ax.CloseSessionSync();
		protected override string GetDeviceStatus()
		{
			var status = ax.DeviceStatus;
			Logger.Info($"Device {ax.MediaName} Status {status}");
			return status;
		}

		protected override int sendRawData(string s) => ax.SendRawData(1, s);
		protected override int printForm(FileInfo image) => ax.PrintForm("CHECKV", "CHECKMEDIA", "TOPLEFT", 0, 0, "MEDIUM", $"FImage={image}", -1, "ANY");
		protected override int controlMedia() => ax.ControlMedia("EJECT", Timeout.AwaitTaken);
		protected override int reset() => ax.Reset(PrinterScannerResetAction.RETRACT.ToString(), 1);

		public async Task PrintAsync(FileInfo image, PaperSource paperSource)
		{
			await PrintImageOperation.StartAsync(
				() =>
					ax.PrintForm("CHECKV", "CHECKMEDIA", "TOPLEFT", 0, 0, "MEDIUM", $"FImage={image}", -1, paperSource.ToString())
				);
		}

		protected override void OnInitialized()
		{
			ax.DeviceError += Ax_DeviceError;
			ax.FatalError += Ax_FatalError;
			ax.ResetComplete += Ax_ResetComplete;
			ax.SendRawDataComplete += Ax_SendRawDataComplete;
			ax.Timeout += Ax_Timeout;
			ax.PrintFormComplete += Ax_PrintFormComplete;
			ax.FieldError += Ax_FieldError;
			ax.ControlMediaComplete += Ax_ControlMediaComplete;
			ax.MediaTaken += Ax_MediaTaken;
		}

		protected override void OnDisposing()
		{
			ax.DeviceError -= Ax_DeviceError;
			ax.FatalError -= Ax_FatalError;
			ax.ResetComplete -= Ax_ResetComplete;
			ax.SendRawDataComplete -= Ax_SendRawDataComplete;
			ax.Timeout -= Ax_Timeout;
			ax.PrintFormComplete -= Ax_PrintFormComplete;
			ax.FieldError -= Ax_FieldError;
			ax.ControlMediaComplete -= Ax_ControlMediaComplete;
			ax.MediaTaken -= Ax_MediaTaken;
		}

		void Ax_MediaTaken(object sender, EventArgs e) => EjectOperation.Stop(true);
		void Ax_ControlMediaComplete(object sender, _DNXStatementPrinterXEvents_ControlMediaCompleteEvent e) { }
		void Ax_PrintFormComplete(object sender, EventArgs e) => PrintImageOperation.Stop(true);
		void Ax_SendRawDataComplete(object sender, _DNXStatementPrinterXEvents_SendRawDataCompleteEvent e) => PrintTextOperation.Stop(true);
		void Ax_ResetComplete(object sender, EventArgs e) => ResetOperation.Stop(true);

		void Ax_FieldError(object sender, _DNXStatementPrinterXEvents_FieldErrorEvent e) => OnError(new DeviceMalfunctionException(nameof(Ax_FieldError)));
		void Ax_Timeout(object sender, EventArgs e) => OnError(new DeviceTimeoutException(nameof(StatementPrinter)));
		void Ax_FatalError(object sender, _DNXStatementPrinterXEvents_FatalErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));
		void Ax_DeviceError(object sender, _DNXStatementPrinterXEvents_DeviceErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));

		protected override string GetNibbleFromString(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return string.Empty;
			}
			//FFFE used to mark string as UNICODE to be able to print Arabic
			var res = $"FFFE{BitConverter.ToString(Encoding.Unicode.GetBytes(data)).Replace("-", "")}";
			// 1B45 and 1B46 used inside printer to escape BOLD chars.
			// 5E0042005E00 is hex representation of ^B^ that is used in templates to start bold chars.
			// 5E002F0042005E00 is hex representation of ^/B^ that is used in templates to end bold chars.
			res = res.Replace("5E0042005E00", "1B45").Replace("5E002F0042005E00", "1B46");
			return res;
		}

		public override string GetPaperStatus()
		{
			return ax.get_PaperStatus("UPPER");
		}

		public string GetChequePaperStatus()
		{
			return ax.get_PaperStatus("EXTERNAL");
		}

		public string GetInkStatu()
		{
			return ax.InkStatus;
		}

		public string GetTonerStatus()
		{
			return ax.TonerStatus;
		}

		public override PrinterStatus GetPrinterStatus()
		{
			var mediaStatus = ax?.MediaStatus;
			var status = PrinterStatus.NotPresent;

			if (!string.IsNullOrEmpty(mediaStatus))
			{
				Logger.Info(mediaStatus);

				if (mediaStatus == "PRESENT")
					status = PrinterStatus.Present;
				else if (mediaStatus == "NOTPRESENT")
					status = PrinterStatus.NotPresent;
				else if (mediaStatus == "JAMMED")
					status = PrinterStatus.Jammed;
				else if (mediaStatus == "UNKNOWN")
					status = PrinterStatus.Unknown;
				else if (mediaStatus == "NOTSUPP")
					status = PrinterStatus.NotSupp;
				else if (mediaStatus == "ENTERING")
					status = PrinterStatus.Entering;
			}

			return status;
		}

		public void TurnOnGuideLights()
		{
			GuideLight?.TurnOn();
		}

		public void TurnOffGuideLights()
		{
			GuideLight?.TurnOff();
		}
	}
}