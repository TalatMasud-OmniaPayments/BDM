namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class TransactionNotificationRequest : RequestBase
	{
		public Interface.TransactionType TransactionType { get; set; }
		public string TransactionMode { get; set; }
		public string CardNumber { get; set; }
		public string Amount { get; set; }
		public string TransactionReference { get; set; }
		public string AccountNumber { get; set; }
		public string CustomerIdentifier { get; set; }
		public Interface.Enums.TransactionStatus TransactionStatus { get; set; }
		public string Reason { get; set; }
		public string StatementMonths { get; set; }
		public string ChequeLeaves { get; set; }
		public string ResponseCode { get; set; }
	}
}