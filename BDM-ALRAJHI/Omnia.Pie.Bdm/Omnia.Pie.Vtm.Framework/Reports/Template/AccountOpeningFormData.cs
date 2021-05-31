namespace Omnia.Pie.Vtm.Framework.Reports.Template
{
	public class AccountOpeningFormData
	{
		public string CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string AccountType { get; set; }
		public string AccountCurrency { get; set; }
		public bool IsChequeBook { get; set; }
		public string Signature1Base64Content { get; set; }
		public string Signature2Base64Content { get; set; }
		public string CheckedById { get; set; }
		public string CheckedByName { get; set; }
	}
}