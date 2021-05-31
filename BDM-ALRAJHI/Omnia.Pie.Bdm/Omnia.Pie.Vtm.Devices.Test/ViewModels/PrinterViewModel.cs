using System.Windows.Media.Imaging;
using Omnia.Pie.Vtm.Devices.Interface;

namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	public class PrinterViewModel : DeviceViewModel { }
	public class PrinterViewModel<TPrinter> : PrinterViewModel where TPrinter : IPrinter
	{
		public PrinterViewModel()
		{
			PrintText = new OperationViewModel(() => Model.PrintAsync("text"))
			{
				Id = nameof(Model.PrintAsync)
			};
			PrintImage = new OperationViewModel(() => ((IReceiptPrinter)Model).PrintAsync(chequeImage))
			{
				Id = nameof(Model.PrintAsync)
			};
			Eject = new OperationViewModel(() => Model.EjectAsync())
			{
				Id = nameof(Model.EjectAsync)
			};
		}

		new TPrinter Model => (TPrinter)base.Model;
		readonly BitmapImage chequeImage = ImageHelper.BitmapImageFromResource("Top1.bmp");

		public OperationViewModel PrintText { get; private set; }
		public OperationViewModel PrintImage { get; private set; }
		public OperationViewModel Eject { get; private set; }

		public PrinterStatus PrinterStatus()
		{
			return ((IStatementPrinter)Model).GetPrinterStatus();
		}

		public override void Load()
		{
			if (Model != null && Model is IStatementPrinter)
			{
				_logger.Info(PrinterStatus().ToString());

				var pprStatus = ((IStatementPrinter)Model).GetPaperStatus();
				var lowerpprStatus = ((IStatementPrinter)Model).GetChequePaperStatus();

				_logger.Info($"A4 Printer Upper paper status : {pprStatus} A4 Printer Lower paper status : {lowerpprStatus}");
			}

			base.Load();
		}
	}
}