namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	public class PinVerificationResult
	{
		public string CustomerIdentifier { get; set; }
		public string IccResponse { get; set; }
		public string AuthCode { get; set; }
		public string ResponseCode { get; set; }
	}
}