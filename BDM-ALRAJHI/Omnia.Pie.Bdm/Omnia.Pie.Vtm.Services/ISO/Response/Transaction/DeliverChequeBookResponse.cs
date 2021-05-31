namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{

	public class DeliveredChequeBook
	{
		public string ReferenceNumber { get; set; }
	}

	public class DeliverChequeBookResponse : ResponseBase<DeliveredChequeBook>
	{
		public DeliveredChequeBook DeliveredChequeBook { get; set; }
	}
}