namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	using System.Collections.Generic;

	public class TransactionNotification
	{
		public string Status { get; set; }
		public string ReferenceNumber { get; set; }
	}

	public class TransactionNotificationResponse : ResponseBase<List<TransactionNotification>>
	{
		public TransactionNotification TransactionNotification { get; set; }
	}
}