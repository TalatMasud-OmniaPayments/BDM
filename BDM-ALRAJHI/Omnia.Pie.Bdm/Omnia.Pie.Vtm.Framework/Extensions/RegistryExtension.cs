namespace Omnia.Pie.Vtm.Framework.Extensions
{
	using Microsoft.Win32;
	using System.Collections.Generic;

	public static class RegistryExtension
	{
		public static IEnumerable<KeyValuePair<string, object>> GetValues(this RegistryKey reg)
		{
			if (reg != null)
				foreach (var i in reg.GetValueNames())
					yield return new KeyValuePair<string, object>(i, reg.GetValue(i));
		}
	}
}