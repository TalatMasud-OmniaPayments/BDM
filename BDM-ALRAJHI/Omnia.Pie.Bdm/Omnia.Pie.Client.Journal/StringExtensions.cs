namespace Omnia.Pie.Client.Journal
{
	internal static class StringExtensions
	{
		public static bool ContainsAtIndex(this string s, string value, int index)
		{
			if (index + value.Length > s.Length)
			{
				return false;
			}

			for (int i = 0; i < value.Length; ++i)
			{
				if (s[index + i] != value[i])
				{
					return false;
				}
			}

			return true;
		}

		public static string Trim(this string s, string trimString)
		{
			int trimmedStartIndex = 0;
			while (s.ContainsAtIndex(trimString, trimmedStartIndex))
			{
				trimmedStartIndex += trimString.Length;
			}

			int trimmedLength = s.Length - trimmedStartIndex;
			while (trimmedLength > 0)
			{
				int newTrimmedLength = trimmedLength - trimString.Length;
				if (newTrimmedLength < 0 || !s.ContainsAtIndex(trimString, trimmedStartIndex + newTrimmedLength))
				{
					break;
				}
				else
				{
					trimmedLength = newTrimmedLength;
				}
			}

			if (trimmedLength == 0)
			{
				return string.Empty;
			}
			return s.Substring(trimmedStartIndex, trimmedLength);
		}
	}
}