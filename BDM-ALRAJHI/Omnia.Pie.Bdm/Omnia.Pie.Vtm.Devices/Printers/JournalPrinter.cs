namespace Omnia.Pie.Vtm.Devices.JournalPrinter
{
	using System;
	using System.IO;
	using System.Windows.Forms;
	using AxNXJournalPrinterXLib;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Enum;
	using System.Text;

	public class JournalPrinter : PrinterScanner, IJournalPrinter
	{
		public override string GetPaperStatus()
		{
			return ax.get_PaperStatus("UPPER");
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

        public string GetReceiptPaperStatus()
        {
            var mediaStatus = ax?.MediaStatus;
            Logger.Info("JournalPrinter mediaStatus" + mediaStatus);
            return mediaStatus;
        }

        public JournalPrinter(IDeviceErrorStore deviceErrorStore, ILogger logger, IGuideLights guideLights)
			: base(deviceErrorStore, logger, null, guideLights)
		{
			Logger.Info("JournalPrinter Initialized");
		}

		AxNXJournalPrinterX ax;
		protected override AxHost CreateAx() => ax = new AxNXJournalPrinterX();
		protected override int OpenSessionSync(int timeout) => ax.OpenSessionSync(timeout);
		protected override int CloseSessionSync() => ax.CloseSessionSync();
		protected override string GetDeviceStatus() => ax.DeviceStatus;
		protected override int sendRawData(string s) => ax.SendRawData(1, s);
		protected override int printForm(FileInfo image) => DeviceResult.Ok;
		protected override int controlMedia() => ax.ControlMedia("EJECT", Timeout.AwaitTaken);
		protected override int reset() => ax.Reset(PrinterScannerResetAction.EJECT.ToString(), 1);

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

		void Ax_ResetComplete(object sender, EventArgs e) => ResetOperation.Stop(true);
		void Ax_MediaTaken(object sender, EventArgs e) => EjectOperation.Stop(true);
		void Ax_SendRawDataComplete(object sender, _DNXJournalPrinterXEvents_SendRawDataCompleteEvent e) => PrintTextOperation.Stop(true);
		void Ax_PrintFormComplete(object sender, EventArgs e) => PrintImageOperation.Stop(true);
		void Ax_ControlMediaComplete(object sender, _DNXJournalPrinterXEvents_ControlMediaCompleteEvent e) { }

		void Ax_FieldError(object sender, _DNXJournalPrinterXEvents_FieldErrorEvent e) => OnError(new DeviceMalfunctionException(nameof(Ax_FieldError)));
		void Ax_FatalError(object sender, _DNXJournalPrinterXEvents_FatalErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));
		void Ax_DeviceError(object sender, _DNXJournalPrinterXEvents_DeviceErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));
		void Ax_Timeout(object sender, EventArgs e) => OnError(new DeviceTimeoutException(nameof(JournalPrinter)));

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

		//protected override string GetNibbleFromString(string pData)
		//{
		//	Logger.Info($"GetNibbleFromString : {pData}");

		//	if (string.IsNullOrEmpty(pData))
		//	{
		//		return string.Empty;
		//	}

		//	char[] array = new char[pData.Length * 2];
		//	const string hexDigits = "0123456789ABCDEF";
		//	for (int i = 0; i < pData.Length; i++)
		//	{
		//		int num = (int)pData[i];
		//		array[i * 2] = hexDigits[num >> 4];
		//		array[i * 2 + 1] = hexDigits[num & 15];
		//	}

		//	return new string(array);
		//}
	}
}