namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	public class ValidateSmsOtp
	{
		public string OtpMatched { get; set; }
		public string OtpMismatchCount { get; set; }
	}

	public class ValidateSmsOtpResponse : ResponseBase<ValidateSmsOtp>
	{
		public ValidateSmsOtp ValidateSmsOtp { get; set; }
	}
}
