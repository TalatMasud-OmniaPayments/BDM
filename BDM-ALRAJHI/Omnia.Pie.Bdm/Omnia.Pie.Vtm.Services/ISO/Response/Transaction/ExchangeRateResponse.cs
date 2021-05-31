namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{

	public class ApplyExchangeRate
	{
		public string ExchangeRate { get; set; }
		public string ExchangeRateCurrency { get; set; }
	}

	public class ExchangeRateResponse : ResponseBase<ApplyExchangeRate>
	{
		public ApplyExchangeRate ApplyExchangeRate { get; set; }
	}
}
