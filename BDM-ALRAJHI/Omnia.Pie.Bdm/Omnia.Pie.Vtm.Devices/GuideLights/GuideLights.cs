namespace Omnia.Pie.Vtm.Devices.GuideLights
{
	using System;
	using System.Windows.Forms;
	using AxNXGuidLightsXLib;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using System.Threading.Tasks;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;

	public class GuideLights : Device, IGuideLights
	{
		private AxNXGuidLightsX ax;
		private GuideLight _cashDispenser;
		private GuideLight _cardReader;
		private GuideLight _emiratesIdScanner;
		private GuideLight _pinPad;
		private GuideLight _receiptPrinter;
		private GuideLight _documentPrinter;
		private GuideLight _scanner;
		private GuideLight _chequeAcceptor;
		private GuideLight _coinDispenser;

		public GuideLights(IDeviceErrorStore deviceErrorStore, ILogger logger)
			: base(deviceErrorStore, logger, null, null)
		{
			Logger.Info("GuideLights Initialized");
		}

		protected override void OnInitialized()
		{
			_cashDispenser = new GuideLight(Logger, ax, "NOTESDISPENSER");
			_cardReader = new GuideLight(Logger, ax, "CARDUNIT");
			_emiratesIdScanner = new GuideLight(Logger, ax, "PASSBOOKPRINTER");
			_pinPad = new GuideLight(Logger, ax, "PINPAD");
			_receiptPrinter = new GuideLight(Logger, ax, "RECEIPTPRINTER");
			_documentPrinter = new GuideLight(Logger, ax, "DOCUMENTPRINTER");
			_chequeAcceptor = new GuideLight(Logger, ax, "CHEQUEUNIT");
			_scanner = new GuideLight(Logger, ax, "SCANNER");
			_coinDispenser = new GuideLight(Logger, ax, "COINDISPENSER");

			ax.DeviceError += Ax_DeviceError;
			ax.FatalError += Ax_FatalError;
			ax.ResetComplete += Ax_ResetComplete;
		}

		protected override void OnDisposing()
		{
			ax.DeviceError -= Ax_DeviceError;
			ax.FatalError -= Ax_FatalError;
			ax.ResetComplete -= Ax_ResetComplete;
		}

		void Ax_ResetComplete(object sender, EventArgs e) => ResetOperation.Stop(true);

		void Ax_FatalError(object sender, _DNXGuidLightsXEvents_FatalErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));
		void Ax_DeviceError(object sender, _DNXGuidLightsXEvents_DeviceErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));

		protected override AxHost CreateAx() => ax = new AxNXGuidLightsX();
		protected override int OpenSessionSync(int timeout) => ax.OpenSessionSync(timeout);
		protected override int CloseSessionSync() => ax.CloseSessionSync();
		protected override string GetDeviceStatus() => ax.DeviceStatus;

		public IGuideLight CashDispenser
		{
			get { return _cashDispenser; }
		}

		public IGuideLight CardReader
		{
			get { return _cardReader; }
		}

		public IGuideLight EmiratesIdScanner
		{
			get { return _emiratesIdScanner; }
		}

		public IGuideLight PinPad
		{
			get { return _pinPad; }
		}

		public IGuideLight ReceiptPrinter
		{
			get { return _receiptPrinter; }
		}

		public IGuideLight DocumentPrinter
		{
			get { return _documentPrinter; }
		}

		public IGuideLight Scanner
		{
			get { return _scanner; }
		}

		public IGuideLight ChequeAcceptor
		{
			get { return _chequeAcceptor; }
		}

		public IGuideLight CoinDispenser
		{
			get { return _coinDispenser; }
		}

		public override Task ResetAsync() => ResetOperation.StartAsync(() => ax.Reset());
	}
}