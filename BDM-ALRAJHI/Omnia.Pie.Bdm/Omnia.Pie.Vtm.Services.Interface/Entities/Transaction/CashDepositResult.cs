namespace Omnia.Pie.Vtm.Services.Interface.Entities.Transaction
{
	public class CashDepositResult
	{
		//public string HostTransCode { get; set; }
		//public string Duplicate { get; set; }
        public string ResponseCode { get; set; }
        public string MessageId { get; set; }
        public string SessionId { get; set; }
        public string TransactionId { get; set; }
    }
}