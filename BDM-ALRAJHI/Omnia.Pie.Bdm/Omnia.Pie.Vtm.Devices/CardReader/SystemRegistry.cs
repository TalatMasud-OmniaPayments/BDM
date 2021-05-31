namespace Omnia.Pie.Vtm.Devices.Emv
{
	using Microsoft.Win32;

	public sealed class SystemRegistry
	{
		public const string HKEY_CURRENT_USER = "HKEY_CURRENT_USER";
		public const string SOFTWARE_SUBKEY = "SOFTWARE";
		public const string APPLICATION_NAME = "Omnia ITM";
		public const string HKEY_CURRENT_USER_ROOT_SOFTWARE = HKEY_CURRENT_USER + "\\" + SOFTWARE_SUBKEY + "\\" + APPLICATION_NAME;

		private static SystemRegistry INSTANCE;
		private SystemRegistry() { }

		public static SystemRegistry Instance()
		{
			if (INSTANCE == null)
				INSTANCE = new SystemRegistry();

			return INSTANCE;
		}

		public void ToEMV(string pKey, string pValue)
		{
			string keyGrp = "EMV";
			AddStringKey(keyGrp, pKey, pValue);
		}

		public string FromEMV(string pKey)
		{
			string keyGrp = "EMV";
			var lastValue = GetStringKey(keyGrp, pKey);

			if (string.IsNullOrEmpty(lastValue))
			{
				ToEMV("LastSequenceNumber", "1");
				lastValue = GetStringKey(keyGrp, pKey);
			}

			return lastValue;
		}

		private void AddStringKey(string pSubGroup, string pKey, string pValue)
		{
			var keyPath = $@"{SOFTWARE_SUBKEY}\{APPLICATION_NAME}\{pSubGroup}\{pKey}";
			using (var x86View = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
			{
				var key = x86View?.OpenSubKey(keyPath, true);

				if (key == null)
					key = x86View?.CreateSubKey(keyPath, true);

				key?.SetValue(pKey, pValue, RegistryValueKind.String);
			}
		}

		private string GetStringKey(string pSubGroup, string pKey)
		{
			string group = HKEY_CURRENT_USER_ROOT_SOFTWARE + "\\" + pSubGroup + "\\";
			return Registry.GetValue(group, pKey, "")?.ToString();
		}
	}
}