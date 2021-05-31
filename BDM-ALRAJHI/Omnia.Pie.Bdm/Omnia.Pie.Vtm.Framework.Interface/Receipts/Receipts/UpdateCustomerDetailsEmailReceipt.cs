namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class UpdateCustomerDetailsEmailReceipt
	{
		public bool IsEmiratesIdAuthentication { get; set; }
		public string CardNumber { get; set; }
		public string TransactionNumber { get; set; }
		public string Email { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}