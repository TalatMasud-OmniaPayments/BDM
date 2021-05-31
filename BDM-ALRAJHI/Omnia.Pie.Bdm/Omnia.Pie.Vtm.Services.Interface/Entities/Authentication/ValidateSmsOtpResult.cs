namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	public class ValidateSmsOtpResult
	{
		public bool OtpMatched { get; set; }
		public string OtpMismatchCount { get; set; }
	}
}