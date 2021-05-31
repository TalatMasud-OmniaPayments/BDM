namespace Omnia.Pie.Vtm.Framework.Configurations
{
	using System.Configuration;

	public static class TerminalConfiguration
	{
		public static TerminalSection Section => (TerminalSection)ConfigurationManager.GetSection(TerminalSection.Name);
	}

	public class TerminalSection : ConfigurationSection
	{
		public const string Name = "terminal";

		[ConfigurationProperty("id", IsRequired = true)]
		public string Id => (string)base["id"];

		[ConfigurationProperty("currency", IsRequired = true)]
		public string Currency => (string)base["currency"];

        [ConfigurationProperty("currencyCode", IsRequired = true)]
        public string CurrencyCode => (string)base["currencyCode"];

        [ConfigurationProperty("currencyNumber", IsRequired = true)]
        public string CurrencyNumber => (string)base["currencyNumber"];

        [ConfigurationProperty("locationCity", IsRequired = true)]
		public string LocationCity => (string)base["locationCity"];

		[ConfigurationProperty("CountryCode", IsRequired = true)]
		public string CountryCode => (string)base["CountryCode"];

		[ConfigurationProperty("location", IsRequired = true)]
		public string Location => (string)base["location"];

		[ConfigurationProperty("merchantId", IsRequired = true)]
		public string MerchantId => (string)base["merchantId"];

		[ConfigurationProperty("type", IsRequired = true)]
		public string Type => (string)base["type"];

		[ConfigurationProperty("ownerName", IsRequired = true)]
		public string OwnerName => (string)base["ownerName"];

		[ConfigurationProperty("stateName", IsRequired = true)]
		public string StateName => (string)base["stateName"];

		[ConfigurationProperty("acquiringInstitutionId", IsRequired = true)]
		public string AcquiringInstitutionId => (string)base["acquiringInstitutionId"];

		[ConfigurationProperty("branchId", IsRequired = true)]
		public string BranchId => (string)base["branchId"];

		[ConfigurationProperty("branchCode", IsRequired = true)]
		public string BranchCode => (string)base["branchCode"];

		[ConfigurationProperty("platform", IsRequired = true)]
		public string Platform => (string)base["platform"];

        [ConfigurationProperty("RqID", IsRequired = true)]
        public string RqID => (string)base["RqID"];

        [ConfigurationProperty("SvcID", IsRequired = true)]
        public string SvcID => (string)base["SvcID"];

        [ConfigurationProperty("SubSvcID", IsRequired = true)]
        public string SubSvcID => (string)base["SubSvcID"];

        [ConfigurationProperty("FuncID", IsRequired = true)]
        public string FuncID => (string)base["FuncID"];

        [ConfigurationProperty("MsgVer", IsRequired = true)]
        public string MsgVer => (string)base["MsgVer"];

        [ConfigurationProperty("CICNum", IsRequired = true)]
        public string CICNum => (string)base["CICNum"];

        [ConfigurationProperty("AcctBranch", IsRequired = true)]
        public string AcctBranch => (string)base["AcctBranch"];

        [ConfigurationProperty("AcctTyp", IsRequired = true)]
        public string AcctTyp => (string)base["AcctTyp"];

        [ConfigurationProperty("AcctNum", IsRequired = true)]
        public string AcctNum => (string)base["AcctNum"];

        [ConfigurationProperty("CardNo", IsRequired = true)]
        public string CardNo => (string)base["CardNo"];

        [ConfigurationProperty("IDTyp", IsRequired = true)]
        public string IDTyp => (string)base["IDTyp"];

        [ConfigurationProperty("IDNum", IsRequired = true)]
        public string IDNum => (string)base["IDNum"];

        [ConfigurationProperty("TerminalLanguage", IsRequired = true)]
        public string TerminalLanguage => (string)base["TerminalLanguage"];

        [ConfigurationProperty("UserMobile", IsRequired = true)]
        public string UserMobile => (string)base["UserMobile"];

        [ConfigurationProperty("UserEMail", IsRequired = true)]
        public string UserEMail => (string)base["UserEMail"];

        [ConfigurationProperty("ChID", IsRequired = true)]
        public string ChID => (string)base["ChID"];

        [ConfigurationProperty("SubChID", IsRequired = true)]
        public string SubChID => (string)base["SubChID"];

        [ConfigurationProperty("TerminalDesc", IsRequired = true)]
        public string TerminalDesc => (string)base["TerminalDesc"];

        [ConfigurationProperty("IPAddr", IsRequired = true)]
        public string IPAddr => (string)base["IPAddr"];

        [ConfigurationProperty("UserID", IsRequired = true)]
        public string UserID => (string)base["UserID"];

        [ConfigurationProperty("OSID", IsRequired = true)]
        public string OSID => (string)base["OSID"];

        [ConfigurationProperty("CtryCd", IsRequired = true)]
        public string CtryCd => (string)base["CtryCd"];

        [ConfigurationProperty("SessionLang", IsRequired = true)]
        public string SessionLang => (string)base["SessionLang"];

        [ConfigurationProperty("AppTyp", IsRequired = true)]
        public string AppTyp => (string)base["AppTyp"];

        [ConfigurationProperty("MachineSerialNo", IsRequired = true)]
        public string MachineSerialNo => (string)base["MachineSerialNo"];

    }
}