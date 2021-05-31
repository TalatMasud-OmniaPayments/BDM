namespace Omnia.Pie.Vtm.Services.ISO.Response.Customer
{
	public class ProductInfo
	{
		public string Status { get; set; }
		public string ReferenceNumber { get; set; }
	}

	public class ProductInfoResponse : ResponseBase<ProductInfo>
	{
		public ProductInfo ProductInfo { get; set; }
	}
}