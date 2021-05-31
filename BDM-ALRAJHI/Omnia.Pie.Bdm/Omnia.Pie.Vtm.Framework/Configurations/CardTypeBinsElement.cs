namespace Omnia.Pie.Vtm.Framework.Configurations
{
	using System.Configuration;

	public class CardTypeBinsElement : ConfigurationElement
	{
		[ConfigurationProperty("bins")]
		public string Bins => (string)base["bins"];
	}
}