namespace Omnia.Pie.Vtm.Framework.Configurations
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Xml;

	public class SystemParameter
	{
		public string Key { get; set; }
		public string Title { get; set; }
		public string Value { get; set; }
	}

	public class SystemParametersConfiguration
	{
		private static SystemParameters Section => (SystemParameters)ConfigurationManager.GetSection(SystemParameters.Name);

		public static void SetElementValue(string key, string value)
		{
			try
			{
				var xmlDoc = new XmlDocument();
				xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
				var selectSingleNode = xmlDoc.SelectSingleNode($"//systemparameters/systemparameter[@Key='{key}']");
				if (selectSingleNode?.Attributes != null)
					selectSingleNode.Attributes["Value"].Value = value;
				xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			}
			catch (Exception)
			{

			}
		}

		public static string GetElementValue(string key)
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			var selectSingleNode = xmlDoc.SelectSingleNode($"//systemparameters/systemparameter[@Key='{key}']");
			if (selectSingleNode?.Attributes != null)
				return selectSingleNode.Attributes["Value"].Value;

			return string.Empty;
		}

		public static List<SystemParameter> GetAllElements()
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			var selectedNodes = xmlDoc.SelectNodes($"//systemparameters/systemparameter");
			var data = new List<SystemParameter>();
			foreach (XmlNode node in selectedNodes)
			{
				var obj = new SystemParameter()
				{
					Key = node.Attributes["Key"].Value,
					Value = node.Attributes["Value"].Value,
					Title = node.Attributes["Title"].Value
				};

				data.Add(obj);
			}

			return data;
		}
	}

	class SystemParameters : ConfigurationSection
	{
		public const string Name = "systemparameters";

		[ConfigurationProperty("", IsDefaultCollection = true)]
		public SystemParametersElementCollection Elements => (SystemParametersElementCollection)base[""];
	}

	public class SystemParameterElement : ConfigurationElement
	{
		[ConfigurationProperty("Key", IsKey = true, IsRequired = true)]
		public string Key => (string)base["Key"];

		[ConfigurationProperty("Value", IsKey = true, IsRequired = true)]
		public string Value => (string)base["Value"];
	}

	[ConfigurationCollection(typeof(SystemParameterElement), AddItemName = "systemparameter")]
	public class SystemParametersElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement() => new SystemParameterElement();

		protected override object GetElementKey(ConfigurationElement element) => ((SystemParameterElement)element).Key;
	}
}
