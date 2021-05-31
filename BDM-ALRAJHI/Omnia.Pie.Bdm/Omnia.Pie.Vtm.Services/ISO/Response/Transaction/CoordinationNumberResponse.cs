namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	public class CoordinationNumber
	{
		public string Number { get; set; }
	}

	public class CoordinationNumberResponse : ResponseBase<CoordinationNumber>
	{
		public CoordinationNumber CoordinationNumber { get; set; }
	}
}