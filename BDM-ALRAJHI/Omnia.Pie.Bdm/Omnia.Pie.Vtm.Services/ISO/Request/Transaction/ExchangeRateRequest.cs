namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class ExchangeRateRequest : RequestBase
	{
		public string FromCurrency { get; set; }
		public string ToCurrency { get; set; }
		public string TransactionAmount { get; set; }
		public string PaymentType { get; set; }
	}
}
