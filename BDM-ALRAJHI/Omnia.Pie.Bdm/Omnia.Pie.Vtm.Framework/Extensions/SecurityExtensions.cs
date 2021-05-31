namespace Omnia.Pie.Vtm.Framework.Extensions
{
	using System;
	using System.Runtime.InteropServices;
	using System.Security;

	public static class SecurityExtensions
	{
		public static SecureString ToSecureString(this string str)
		{
			if (str == null)
				throw new ArgumentNullException("str");

			var secStr = new SecureString();
			secStr.Clear();

			foreach (Char c in str)
				secStr.AppendChar(c);

			secStr.MakeReadOnly();
			return secStr;
		}

		public static string Read(this SecureString str)
		{
			if (str == null)
				throw new ArgumentNullException("str");

			IntPtr pointer = IntPtr.Zero;

			try
			{
				pointer = Marshal.SecureStringToBSTR(str);
				return Marshal.PtrToStringBSTR(pointer);
			}
			finally
			{
				Marshal.ZeroFreeBSTR(pointer);
			}
		}
	}
}
