namespace Omnia.Pie.Vtm.Services.ISO.Request.Authentication
{
	public class EmiratesIdRequest : RequestBase
	{
		public string EidNumber { get; set; }
		public string Name { get; set; }
		public string ExpiryDate { get; set; }
	}
}