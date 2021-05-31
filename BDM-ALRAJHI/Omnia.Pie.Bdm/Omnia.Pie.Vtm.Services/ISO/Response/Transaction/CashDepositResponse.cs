namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	public class CashDeposit
	{
		public string MessageId { get; set; }
		public string SessionId { get; set; }
        public string TransactionId { get; set; }
   	}

	public class CashDepositResponse : ResponseBase<CashDeposit>
	{
        //public CashDeposit CashDepositAccount { get; set; }
        public string MessageId { get; set; }
        public string SessionId { get; set; }
        public string TransactionId { get; set; }
    }
}