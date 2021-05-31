namespace Omnia.Pie.Vtm.Framework.Configurations
{
	using System.Configuration;

	public class ContractExceptionSection : ConfigurationSection
	{
		public const string Name = "contractexception";

		[ConfigurationProperty("", IsDefaultCollection = true)]
		public OperationElementCollection Elements => (OperationElementCollection)base[""];
	}

	[ConfigurationCollection(typeof(OperationElement), AddItemName = "operation")]
	public class OperationElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement() => new OperationElement();

		protected override object GetElementKey(ConfigurationElement element) => ((OperationElement)element).Contract;
	}

	public class OperationElement : ConfigurationElement
	{
		[ConfigurationProperty("contract", IsKey = true, IsRequired = true)]
		public string Contract => (string)base["contract"];


		[ConfigurationProperty("", IsDefaultCollection = true)]
		public ErrorElementCollection Elements => (ErrorElementCollection)base[""];
	}

	[ConfigurationCollection(typeof(ErrorElement), AddItemName = "error")]
	public class ErrorElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement() => new ErrorElement();

		protected override object GetElementKey(ConfigurationElement element) => ((ErrorElement)element).Code;
	}

	public class ErrorElement : ConfigurationElement
	{
		[ConfigurationProperty("code")]
		public string Code => (string)base["code"];

		[ConfigurationProperty("message")]
		public string Message => (string)base["message"];

		[ConfigurationProperty("exception")]
		public string Exception => (string)base["exception"];
	}
}