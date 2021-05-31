namespace Omnia.Pie.Vtm.Devices.Interface.Entities
{
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

	public class Cheque
	{
		public int MediaId { get; set; }
		public BitmapImage FrontImage { get; set; }
		public BitmapImage BackImage { get; set; }
		public BitmapImage ValidationImage { get; set; }
		public string Micr { get; set; }
		public string ChequeNumber { get; set; }
		public string RoutingCode { get; set; }
		public string FromAccount { get; set; }
		public bool MicrAvailable => !string.IsNullOrEmpty(Micr) && Micr != "0";
		public string TextToPrint { get; set; }
		public RotateTransform ChequeImageTransform { get; set; } = new RotateTransform() { Angle = 0 };
	}
}