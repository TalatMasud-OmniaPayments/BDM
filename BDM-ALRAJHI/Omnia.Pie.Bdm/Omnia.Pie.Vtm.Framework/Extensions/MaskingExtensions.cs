namespace Omnia.Pie.Vtm.Framework.Extensions
{
	using System;
	using System.Text.RegularExpressions;

	public static class MaskingExtensions
	{
		private const char MaskSymbol = '*';
		private const string MaskPattern = "^(.{6})(.+)(.{4})$";

		public static string ToMaskedCardNumber(this string v)
		{
			var result = v;

			if (!string.IsNullOrEmpty(result))
			{
				result = Regex.Replace(result, MaskPattern, m => $"{m.Groups[1]}{new String(MaskSymbol, m.Groups[2].Length)}{m.Groups[3]}");
			}

			return result;
		}
	}
}