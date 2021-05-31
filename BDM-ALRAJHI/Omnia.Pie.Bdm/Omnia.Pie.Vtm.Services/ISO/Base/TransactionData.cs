namespace Omnia.Pie.Vtm.Services.ISO
{
	public class TransactionData
	{
		public string TransactionType { get; set; }
		public string TransactionNumber { get; set; }
		public string SessionId { get; set; }
        public string SessionLanguage { get; set; }
        public string MessageTimestamp { get; set; }
    }
}