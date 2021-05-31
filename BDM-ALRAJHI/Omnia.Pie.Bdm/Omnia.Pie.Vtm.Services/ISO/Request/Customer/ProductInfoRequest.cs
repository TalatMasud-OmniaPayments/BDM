namespace Omnia.Pie.Vtm.Services.ISO.Request.Customer
{
	public class ProductInfoRequest : RequestBase
	{
		public string LeadType { get; set; }
		public string MobileNumber { get; set; }
		public string Email { get; set; }
		public string Language { get; set; }
	}
}