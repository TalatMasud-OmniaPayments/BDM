namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	public class Terminal
	{
		public string Id { get; set; }
		public string BranchId { get; set; }
		public string Platform { get; set; }
		public string BranchCode { get; set; }
		//public string Currency { get; set; }
		//public string LocationCity { get; set; }
		//public string LocationCountry { get; set; }
		public string MerchantId { get; set; }
		public string Type { get; set; }
		public string OwnerName { get; set; }
		public string StateName { get; set; }
        public string MachineSerialNo { get; set; }
        public string CountryCode { get; set; }
        public string TerminalLanguage { get; set; }
    }
}