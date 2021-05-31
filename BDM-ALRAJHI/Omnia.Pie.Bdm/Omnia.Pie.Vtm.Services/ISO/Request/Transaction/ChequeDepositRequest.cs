namespace Omnia.Pie.Vtm.Services.ISO.Request.Transaction
{
	public class ChequeDepositRequest : RequestBase
	{
		public string ChequeDate { get; set; }
		public string SettlementDate { get; set; }
		public string SessionTime { get; set; }
		public string SettlementTime { get; set; }
		public string IssuerBankRtn { get; set; }
		public string AccountNumber { get; set; }
		public string SequenceNumber { get; set; }
		public string Amount { get; set; }
		public string CorrIndicator { get; set; }
		public string PreBankRtn { get; set; }
		public string EndorsementDate { get; set; }
		public string DepositIban { get; set; }
		public string PayeeName { get; set; }
		public string FrontImage { get; set; }
		public string BackImage { get; set; }
		public string FrontImageLength { get; set; }
		public string BackImageLength { get; set; }
	}
}