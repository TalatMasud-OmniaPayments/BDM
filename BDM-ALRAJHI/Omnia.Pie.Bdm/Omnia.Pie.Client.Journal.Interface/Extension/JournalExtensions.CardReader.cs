using Omnia.Pie.Client.Journal.Interface.Enum;
using System;
using System.Text;

namespace Omnia.Pie.Client.Journal.Interface.Extension
{
    public static partial class JournalExtensions
    {
		public static void CardReaderCardInserted(this IJournal journal)
		{
			journal.Write(Environment.NewLine, JournalTimestampStyle.None);
			journal.Write("CARD INSERTED");
		}

		public static void CardReaderCardEjected(this IJournal journal)
		{
			journal.Write("CARD TAKEN");
			journal.Write(Environment.NewLine, JournalTimestampStyle.None);
		}

		public static void CardReaderCardTaken(this IJournal journal)
		{
			journal.Write("CARD EJECTED");
		}

		public static void CardReaderCardRead(this IJournal journal, string cardNumber)
		{
			journal.Write("CARD READ");
			journal.CardReaderCardNumber(cardNumber);
			journal.CardReaderCardScheme(cardNumber);
		}

		public static void EmiratesIdCardRead(this IJournal journal, string cardNumber)
		{
			journal.Write("EID READ");
			journal.Write(cardNumber);
		}

		public static void CardReaderCardCaptured(this IJournal journal)
		{
			journal.Write("CARD CAPTURED");
		}

		public static void CardNotTaken(this IJournal journal)
		{
			journal.Write("CARD NOT TAKEN");
		}

		private static void CardReaderCardNumber(this IJournal journal, string cardNumber)
		{
			if (cardNumber != null)
			{
				var str = new StringBuilder();
				str.AppendLine(MaskCardNumber(cardNumber));
				str.Append(Environment.NewLine);
				journal.Write(str.ToString());
			}
		}

		private static void CardReaderCardScheme(this IJournal journal, string cardNumber)
		{
			if (cardNumber != null)
			{
				string cardScheme = GetCardSchema(cardNumber);

				if (cardScheme != null)
				{
					journal.Write(cardScheme);
				}
			}
		}

		private static string GetCardSchema(string cardNumber)
		{
			// TODO: [VD]:
			// 1. Move this method to CardHelper
			// 2. Implement this method when Farai or Nelson will provide logic
			if (cardNumber.StartsWith("4"))
			{
				return "VISA";
			}

			if (cardNumber.StartsWith("5"))
			{
				return "MASTERCARD";
			}

			return null;
		}
	}
}
