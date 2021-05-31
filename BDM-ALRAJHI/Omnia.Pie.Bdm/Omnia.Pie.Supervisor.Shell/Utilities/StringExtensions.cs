using System.Collections.Generic;
using System.Linq;

namespace Omnia.Pie.Supervisor.Shell.Utilities
{
	internal static class StringExtensions
	{
		public static string ToHumanString(this string s)
		{
			s = s.Trim('_');
			s = new[] {"Async", "ViewModel"}.Aggregate(s, (current, i) => current.TrimEnd(i));
			s = string.Join(" ", s.Aggregate(new List<string>(), (result, ch) => {
				if (result.Count == 0 || char.IsUpper(ch))
					result.Add(ch.ToString());
				else
					result[result.Count - 1] += ch;
				return result;
			}));
			return s;
		}

		public static string TrimEnd(this string s, string end) =>
			s?.EndsWith(end) ?? false ? s.Substring(0, s.Length - end.Length) : s;
	}
}
