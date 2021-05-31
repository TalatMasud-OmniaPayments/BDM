namespace Omnia.Pie.Supervisor.Shell.Configuration
{
	using Microsoft.Practices.EnterpriseLibrary.Logging;
	using Omnia.Pie.Supervisor.Shell.Constants;
	using Omnia.Pie.Supervisor.Shell.Service;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Microsoft.Practices.Unity;
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Xml;

	internal static class SupervisoryConfiguration
	{
		private static SupervisoryConfigurationSection Section = (SupervisoryConfigurationSection)ConfigurationManager.GetSection(SupervisoryConfigurationSection.Name);
		private static ILogger _logger = ServiceLocator.Instance.Resolve<ILogger>();
		public static string GetValue(string key)
		{
			return (from SupervisoryConfigurationElement item in Section.SupervisoryConfigurations
					where item.Id == key
					select item.Value).FirstOrDefault();
		}

		public static Tuple<string, string> GetIPandPort()
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			var selectSingleNode = xmlDoc.SelectSingleNode(ConfigurationKeys.ActiveDirectoryClients);
			if (selectSingleNode != null && !string.IsNullOrEmpty(selectSingleNode.Attributes["baseAddress"].Value))
			{
				string baseAddress = selectSingleNode.Attributes["baseAddress"].Value;
				string IPAddressWithPort = baseAddress.Substring(8).Split('/')[0];
				string IPAddress = IPAddressWithPort.Split(':')[0];
				string Port = IPAddressWithPort.Split(':')[1];
				return new Tuple<string, string>(IPAddress, Port);
			}
			else
			{
				return new Tuple<string, string>("", "");
			}
		}

		public static bool SetServerAddress(string IPAddress, string Port)
		{
			List<string> lst = ConfigurationKeys.GetAllKeys();
			var xmlDoc = new XmlDocument();
			xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			foreach (var item in lst)
			{
				var selectSingleNode = xmlDoc.SelectSingleNode(item);
				string baseAddress = selectSingleNode.Attributes["baseAddress"].Value;
				string newBaseAddress = string.Empty;
				string fqBaseAddress = baseAddress.Substring(8);
				string serviceAddress = fqBaseAddress.Substring(fqBaseAddress.IndexOf('/') + 1);
				newBaseAddress = $"https://{IPAddress}:{Port}/{serviceAddress}";
				selectSingleNode.Attributes["baseAddress"].Value = newBaseAddress;
			}
			xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			ConfigurationManager.RefreshSection("clients");
			return true;
		}

		public static void SetElementValue(string key, string value)
		{
			try
			{
				var xmlDoc = new XmlDocument();
				xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
				var selectSingleNode = xmlDoc.SelectSingleNode($"//supervisor/settings/setting[@id='{key}']");
				if (selectSingleNode?.Attributes != null)
					selectSingleNode.Attributes["value"].Value = value;
				xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			}
			catch (Exception ex)
			{
				_logger.Error($"Element value cannot be set");
				_logger.Exception(ex);
			}
		}

		public static string GetElementValue(string key)
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			var selectSingleNode = xmlDoc.SelectSingleNode($"//supervisor/settings/setting[@id='{key}']");
			if (selectSingleNode?.Attributes != null)
				return selectSingleNode.Attributes["value"].Value;
			return "";
		}

		public static string GetRoleValue(string key)
		{
			return (
				from SupervisoryConfigurationElement item in Section.SupervisoryConfigurationRoles
				where item.Id == key
				select item.Value).FirstOrDefault();
		}

		public static Tuple<string, string> GetCommonSettings()
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			var selectSingleNode = xmlDoc.SelectSingleNode($"//terminal");
			Tuple<string, string> tuple = new Tuple<string, string>("", "");
			if (selectSingleNode?.Attributes != null)
			{
				tuple = new Tuple<string, string>(selectSingleNode.Attributes["id"].Value, selectSingleNode.Attributes["branchId"].Value);
			}
			xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			return tuple;
		}

		public static void SetCommonSettings(string terminalID, string branchID)
		{
			try
			{
				var xmlDoc = new XmlDocument();
				xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
				var selectSingleNode = xmlDoc.SelectSingleNode($"//terminal");
				if (selectSingleNode?.Attributes != null)
				{
					selectSingleNode.Attributes["id"].Value = terminalID;
					selectSingleNode.Attributes["branchId"].Value = branchID;
				}
				xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

				ConfigurationManager.RefreshSection("terminal");
			}
			catch (Exception ex)
			{
				_logger.Error($"App.config Element value cannot be set");
				_logger.Exception(ex);
			}
		}

		public static void SetRoleElementValue(string key, string value)
		{
			try
			{
				var xmlDoc = new XmlDocument();
				xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
				var selectSingleNode = xmlDoc.SelectSingleNode($"//{SupervisoryConfigurationSection.Name}/roles/role[@id='{key}']");
				if (selectSingleNode?.Attributes != null)
					selectSingleNode.Attributes["value"].Value = value;
				xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

				ConfigurationManager.RefreshSection(SupervisoryConfigurationSection.Name);
			}
			catch (Exception ex)
			{
				_logger.Error($"App.config Element value cannot be set");
				_logger.Exception(ex);
			}
		}

		public static void SetCassetteElementValues(Dictionary<string, string> keyValues)
		{
			try
			{
				var xmlDoc = new XmlDocument();
				xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
				foreach (var keyValue in keyValues)
				{
					var selectSingleNode = xmlDoc.SelectSingleNode($"//{SupervisoryConfigurationSection.Name}/cassettes/cassette[@id='{keyValue.Key}']");
					if (selectSingleNode?.Attributes != null)
						selectSingleNode.Attributes["value"].Value = keyValue.Value;
				}
				xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

				ConfigurationManager.RefreshSection(SupervisoryConfigurationSection.Name);
			}
			catch (Exception ex)
			{
				_logger.Error($"App.config Element value cannot be set");
				_logger.Exception(ex);
			}
		}

		public static string GetConfigValueWithXPath(string xPath, string attributeToRead)
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			var selectSingleNode = xmlDoc.SelectSingleNode(xPath);
			if (selectSingleNode?.Attributes != null)
				return selectSingleNode.Attributes[attributeToRead].Value;
			else
				return "";
		}
	}
}