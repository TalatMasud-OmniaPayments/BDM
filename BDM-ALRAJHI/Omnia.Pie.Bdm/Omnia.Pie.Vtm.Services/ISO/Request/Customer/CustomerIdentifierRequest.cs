namespace Omnia.Pie.Vtm.Services.ISO.Request.Customer
{
	public class CustomerIdentifierRequest : RequestBase
	{
		public string CardNumber { get; set; }
		public string CardType { get; set; }
	}
}
