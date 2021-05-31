namespace Omnia.Pie.Vtm.Services.ISO.Response.Authentication
{
	public class EmiratesId
	{
		public string CustomerIdentifier { get; set; }
		public string Status { get; set; }
	}

	public class EmiratesIdResponse : ResponseBase<EmiratesId>
	{
		public EmiratesId EmiratesId { get; set; }
	}
}