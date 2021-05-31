namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	public class CreditCardResult
	{
		public string Type { get; set; }
		public string Number { get; set; }
		public string Balance { get; set; }
		public string Currency { get; set; }
		public string CurrencyCode { get; set; }
		public string CardLimit { get; set; }
		public string StatementMinimumDue { get; set; }
		public string CardCode { get; set; }
	}
}