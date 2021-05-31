namespace Omnia.Pie.Vtm.Framework.Configurations
{
	using System.Configuration;

	public class DevicesSection : ConfigurationSection
	{
		public const string Name = "devices";

		[ConfigurationProperty("creditCardBins", IsRequired = true)]
		public CardTypeBinsElement CreditCardBins => (CardTypeBinsElement)base["creditCardBins"];

		[ConfigurationProperty("debitCardBins", IsRequired = true)]
		public CardTypeBinsElement DebitCardBins => (CardTypeBinsElement)base["debitCardBins"];

		[ConfigurationProperty("RMMAgentService", IsRequired = true)]
		public WindowsServiceElement RMMAgentService => (WindowsServiceElement)base["RMMAgentService"];
	}
}