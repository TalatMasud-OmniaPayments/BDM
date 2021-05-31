namespace Omnia.Pie.Vtm.Framework.Extensions
{
	using System.Collections.Generic;
	using System.Text;

	public static class StringBuilderExtensions
	{
		private static IList<KeyValuePair<string, string>> _dictionary = new List<KeyValuePair<string, string>>();

		public static StringBuilder Append(this StringBuilder stringBuilder, string value, string definition)
		{
			if (!string.IsNullOrEmpty(definition))
			{
				_dictionary.Add(new KeyValuePair<string, string>(definition, value));
			}

			stringBuilder.Append(value);
			return stringBuilder;
		}

		public static StringBuilder Append(this StringBuilder stringBuilder, char value, string definition)
		{
			if (!string.IsNullOrEmpty(definition))
			{
				_dictionary.Add(new KeyValuePair<string, string>(definition, value.ToString()));
			}

			stringBuilder.Append(value);

			return stringBuilder;
		}

		public static string ToStringExtend(this StringBuilder stringBuilder)
		{
			foreach (var current in _dictionary)
			{
				string str = current.Value;
				if (current.Value == '\u001c'.ToString() || current.Value == '\u001d'.ToString())
				{
					str = ".";
				}
			}

			_dictionary.Clear();
			return stringBuilder.ToString();
		}
	}
}