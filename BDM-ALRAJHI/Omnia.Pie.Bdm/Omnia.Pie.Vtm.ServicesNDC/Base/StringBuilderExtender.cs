namespace Omnia.Pie.Vtm.ServicesNdc.Base
{
	using Omnia.Pie.Vtm.Framework.Extensions;
	using System;
	using System.Text;

	public static class StringBuilderExtender
	{
		private const string FieldSeparator = "\u001c";
		private const string EscapeCharacter = "\u001B";
		private const string GreaterThan = "\u003E";


		public static string MakeConfigIdInfo()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("2", "[b] Message Class(Solicited)");
			stringBuilder.Append("2", "[c] Message Sub-Class(Status)");
			stringBuilder.Append(BuildHeader());
			stringBuilder.Append("123", "[e] Time Variant Number");
			stringBuilder.Append(FieldSeparator, "    Field Separator");
			stringBuilder.Append("F", "[f] Status Descriptor");
			stringBuilder.Append(FieldSeparator, "    Field Separator");
			stringBuilder.Append("6", "[g1] Message Identifier(ConfigID)");
			stringBuilder.Append(AdjustDataFillRight("123", 4), "[g2] Configuration ID");
			return stringBuilder.ToStringExtend();
		}

		public static string AdjustDataFillRight(string sourceData, int expectedLength)
		{
			if (expectedLength == 0)
			{
				throw new ArgumentOutOfRangeException("pExpectedLength", "Wrong parameter");
			}
			if (sourceData.Length != expectedLength)
			{
				if (sourceData.Length < expectedLength)
				{
					int count = expectedLength - sourceData.Length;
					sourceData += new string('0', count);
				}
				else
				{
					sourceData = sourceData.Substring(0, expectedLength);
				}
			}

			return sourceData;
		}

		public static string BuildHeader()
		{
			var stringBuilder = new StringBuilder();
			var value = AdjustDataFillRight("CWD", 3);

			stringBuilder.Append(FieldSeparator, "    Field Separator");
			stringBuilder.Append(value, "[d] Logical Unit Number");
			stringBuilder.Append("terminalNumber", "[d] Security Terminal Number");
			stringBuilder.Append(FieldSeparator, "    Field Separator");
			stringBuilder.Append(FieldSeparator, "    Field Separator");

			return stringBuilder.ToStringExtend();
		}
	}
}
