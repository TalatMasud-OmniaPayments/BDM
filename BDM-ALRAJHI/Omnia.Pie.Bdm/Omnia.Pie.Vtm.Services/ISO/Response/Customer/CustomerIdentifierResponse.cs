namespace Omnia.Pie.Vtm.Services.ISO.Response.Customer
{
	public class CustomerIdentifier
	{
		public string CustomerId { get; set; }
	}

	public class CustomerIdentifierResponse : ResponseBase<CustomerIdentifier>
	{
		public CustomerIdentifier CustomerIdentifier { get; set; }
	}
}