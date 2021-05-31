

namespace Omnia.Pie.Vtm.Framework.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml;

    public class UserRole
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
    public class UserRolesConfiguration
    {
        private static UserRoles Section => (UserRoles)ConfigurationManager.GetSection(UserRoles.Name);

        public static void SetElementValue(string key, string value)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                var selectSingleNode = xmlDoc.SelectSingleNode($"//userroles/userrole[@Key='{key}']");
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
            var selectSingleNode = xmlDoc.SelectSingleNode($"//userroles/userrole[@Key='{key}']");
            if (selectSingleNode?.Attributes != null)
                return selectSingleNode.Attributes["Value"].Value;

            return string.Empty;
        }

        public static List<UserRole> GetAllElements()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            var selectedNodes = xmlDoc.SelectNodes($"//userroles/userrole");
            var data = new List<UserRole>();
            foreach (XmlNode node in selectedNodes)
            {
                var obj = new UserRole()
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

    class UserRoles : ConfigurationSection
    {
        public const string Name = "userroles";

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public UserRolesElementCollection Elements => (UserRolesElementCollection)base[""];
    }

    public class UserRoleElement : ConfigurationElement
    {
        [ConfigurationProperty("Key", IsKey = true, IsRequired = true)]
        public string Key => (string)base["Key"];

        [ConfigurationProperty("Value", IsKey = true, IsRequired = true)]
        public string Value => (string)base["Value"];
    }

    [ConfigurationCollection(typeof(UserRoleElement), AddItemName = "userrole")]
    public class UserRolesElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new UserRoleElement();

        protected override object GetElementKey(ConfigurationElement element) => ((UserRoleElement)element).Key;
    }
}
