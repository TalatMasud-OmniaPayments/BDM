namespace Omnia.Pie.Vtm.Framework.Extensions
{
	using System.Runtime.Remoting.Metadata.W3cXsd2001;
	using System.Security.Cryptography;

	public static class StringExtensions
	{
		public static string ToTripleDESHexString(this string hexString, string hexKey)
		{
			return ToTripleDESHexString(hexString.ToHexBytes(), hexKey.ToHexBytes());
		}

		private static byte[] ToHexBytes(this string value)
		{
			return SoapHexBinary.Parse(value).Value;
		}

		private static string ToTripleDESHexString(this byte[] hexString, byte[] hexKey)
		{
			return new TripleDESCryptoServiceProvider
			{
				Key = hexKey,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.None
			}
			.CreateEncryptor()
			.TransformFinalBlock(hexString, 0, hexString.Length)
			.ToHexString();
		}

		private static string ToHexString(this byte[] value)
		{
			return new SoapHexBinary(value).ToString();
		}

		public static string GetLastCharacters(this string str, int length)
		{
			if (str == null)
				return null;
			else if (str.Length >= length)
				return str.Substring(str.Length - length, length);
			else
				return str;
		}
	}
}