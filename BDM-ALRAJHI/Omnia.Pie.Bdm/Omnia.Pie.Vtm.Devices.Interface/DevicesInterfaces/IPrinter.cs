namespace Omnia.Pie.Vtm.Devices.Interface
{
	using System.IO;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;

	public interface IPrinter : IDevice
	{
		Task PrintAsync(string text);
		Task PrintAsync(BitmapImage image);
		Task PrintAsync(FileInfo image);
		Task PrintAndEjectAsync(string text);
		Task PrintAndEjectAsync(BitmapImage image);
		Task PrintAndEjectAsync(FileInfo image);
		Task EjectAsync();
		string GetPaperStatus();
		PrinterStatus GetPrinterStatus();
	}
}