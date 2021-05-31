namespace Omnia.Pie.Vtm.Services.ISO.Request.Customer
{
	public class GetLinkedAccountRequest : RequestBase
	{
		public string CardNumber { get; set; }
		public string CustomerIdentifier { get; set; }
	}
}