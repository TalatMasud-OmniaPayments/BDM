namespace Omnia.Pie.Vtm.Services.ISO.Authentication
{
	public class PinVerificationRequest : RequestBase
	{
		public string Track2 { get; set; }
		public string Pin { get; set; }
		public string IccRequest { get; set; }
	}
}