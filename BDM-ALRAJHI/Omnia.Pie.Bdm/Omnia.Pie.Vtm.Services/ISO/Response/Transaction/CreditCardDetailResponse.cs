namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	public class CreditCardDetail
	{
		public string CreditCardCategory { get; set; }
		public string CardNumber { get; set; }
		public string CardType { get; set; }
		public string CurrencyCode { get; set; }
		public string CurrentOutStandingAmount { get; set; }
		public string BilledAmount { get; set; }
		public string MinimumDueAmount { get; set; }
		public string AvailableCreditLimit { get; set; }
		public string CardStatus { get; set; }
	}
	public class CreditCardDetailResponse : ResponseBase<CreditCardDetail>
	{
		public CreditCardDetail CreditCardDetail { get; set; }
	}
}
