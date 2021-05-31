namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	public class UpdateCustomerDetailsMobileReceipt
	{
		public bool IsEmiratesIdAuthentication { get; set; }
		public string CardNumber { get; set; }
		public string TransactionNumber { get; set; }
		public string MobileNumber { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}