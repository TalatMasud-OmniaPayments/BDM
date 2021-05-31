using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.PIE.VTA.Utilities
{
	internal static class ApplicationVersion
	{
		public static string Value => Assembly.GetExecutingAssembly().GetName().Version.ToString();
	}
}
