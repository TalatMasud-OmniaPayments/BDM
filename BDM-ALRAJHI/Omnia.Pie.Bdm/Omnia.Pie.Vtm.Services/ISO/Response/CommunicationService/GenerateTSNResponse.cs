namespace Omnia.Pie.Vtm.Services.ISO.Response.CommunicationService
{
	public class GenerateTSN
	{
		public string value { get; set; }
	}

	public class GenerateTSNResponse : ResponseBase<GenerateTSN>
	{
		public GenerateTSN TSN { get; set; }
	}
}