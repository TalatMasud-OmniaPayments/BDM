namespace Omnia.Pie.Vtm.Services.ISO.Authentication
{
	public class PinVerification
	{
		public string CustomerIdentifier { get; set; }
		public string IccResponse { get; set; }
		public string AuthCode { get; set; }
	}

	public class PinVerificationResponse : ResponseBase<PinVerification>
	{
		public PinVerification PinVerification { get; set; }
	}
}