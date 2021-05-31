namespace Omnia.Pie.Vtm.Framework.Interface.Receipts
{
	using System;

	public class UpdateCustomerDetailsEmiratesIdReceipt
	{
		public bool IsEmiratesIdAuthentication { get; set; }
		public string CardNumber { get; set; }
		public string TransactionNumber { get; set; }
		public string EmiratesIdNumber { get; set; }
		public DateTime? EmiratesIdExpiryDate { get; set; }
		public TransactionStatus TransactionStatus { get; set; }
	}
}