namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class CreditCardDetailRequest : RequestBase
	{
		public string CardNumber { get; set; }
		public string CustomerIdentifier { get; set; }
	}
}