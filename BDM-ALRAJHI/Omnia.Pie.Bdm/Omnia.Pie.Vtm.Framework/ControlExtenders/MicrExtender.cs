namespace Omnia.Pie.Vtm.Framework.ControlExtenders
{
	using System.Globalization;
	using System.Text;

	public static class MicrExtender
	{
		public static string GetMicr(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}

			var stringBuilder = new StringBuilder();
			for (int i = 0; i < str.Length / 2; i++)
			{
				string hexByteString = str.Substring(i * 2, 2);
				byte characterByteCode = GetByte(hexByteString, NumberStyles.AllowHexSpecifier);

				stringBuilder.Append((char)characterByteCode);
			}
			return stringBuilder.ToString();
		}

		private static byte GetByte(string str, NumberStyles numberStyle)
		{
			if (string.IsNullOrEmpty(str))
				return default(byte);

			byte result;
			byte.TryParse(str, numberStyle, CultureInfo.InvariantCulture, out result);

			return result;
		}
	}
}