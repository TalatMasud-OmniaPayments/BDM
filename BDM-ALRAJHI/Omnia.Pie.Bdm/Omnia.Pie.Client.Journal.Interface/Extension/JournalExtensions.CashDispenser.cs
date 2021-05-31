using System.Collections.Generic;
using Omnia.Pie.Client.Journal.Interface.Dto;
using Omnia.Pie.Client.Journal.Interface.Enum;

namespace Omnia.Pie.Client.Journal.Interface.Extension
{
	public static partial class JournalExtensions
	{
		public static void CashDispenserNotesStacked(this IJournal journal)
		{
			journal.Write("CASHDSP: NOTES STACKED");
		}

		public static void CashDispenserNotesRejected(this IJournal journal)
		{
			journal.Write("CASHDSP: NOTES REJECTED");
		}

		public static void CoinDispenserCoinsRejected(this IJournal journal)
		{
			journal.Write("CASHDSP: COINS REJECTED");
		}

		public static void CashDispenserNotesDispensed(this IJournal journal, int[] notesCount)
		{
			var notes = string.Join(",", notesCount);
			journal.Write($"CASHDSP: NOTES DISPENSED {notes}");
		}

		public static void CoinDispenserCoinsDispensed(this IJournal journal, object[] notesCount)
		{
			var notes = string.Join(",", notesCount);
			journal.Write($"CASHDSP: COINS DISPENSED {notes}");
		}

		public static void CashDispenserCassetteStatuses(this IJournal journal, IEnumerable<CashCassetteStatusDto> cashCassetteStatuses)
		{
			string cashTypeHeader = "CASH CNT";
			string initialCountHeader = "INITIAL";
			string rejectedCountHeader = "REJECTED";
			string remainsCountHeader = "REMAINS";

			journal.Write($"{cashTypeHeader} {initialCountHeader} {rejectedCountHeader} {remainsCountHeader}", JournalTimestampStyle.None);

			foreach (var cashCassetteStatus in cashCassetteStatuses)
			{
				string cashType = $"{cashCassetteStatus.Value}{cashCassetteStatus.Currency}".PadRight(cashTypeHeader.Length);
				string initialCount = cashCassetteStatus.InitialCount.ToString().PadLeft(initialCountHeader.Length);
				string rejectedCount = cashCassetteStatus.RejectedCount.ToString().PadLeft(rejectedCountHeader.Length);
				string remainsCount = cashCassetteStatus.RemainingCount.ToString().PadLeft(remainsCountHeader.Length);

				journal.Write($"{cashType} {initialCount} {rejectedCount} {remainsCount}", JournalTimestampStyle.None);
			}
		}

		public static void CoinDispenserCassetteStatuses(this IJournal journal, IEnumerable<CoinCassetteStatusDto> coinCassetteStatuses)
		{
			string coinTypeHeader = "COIN CNT";
			string initialCountHeader = "INITIAL";
			string rejectedCountHeader = "REJECTED";
			string remainsCountHeader = "REMAINS";

			journal.Write($"{coinTypeHeader} {initialCountHeader} {rejectedCountHeader} {remainsCountHeader}", JournalTimestampStyle.None);

			foreach (var cashCassetteStatus in coinCassetteStatuses)
			{
				string cashType = $"{cashCassetteStatus.Value}{cashCassetteStatus.Currency}".PadRight(coinTypeHeader.Length);
				string initialCount = cashCassetteStatus.InitialCount.ToString().PadLeft(initialCountHeader.Length);
				string rejectedCount = cashCassetteStatus.RejectedCount.ToString().PadLeft(rejectedCountHeader.Length);
				string remainsCount = cashCassetteStatus.RemainingCount.ToString().PadLeft(remainsCountHeader.Length);

				journal.Write($"{cashType} {initialCount} {rejectedCount} {remainsCount}", JournalTimestampStyle.None);
			}
		}

		public static void CashDispenserNotesPresented(this IJournal journal)
		{
			journal.Write("CASHDSP: NOTES PRESENTED");
		}

		public static void CoinDispenserCoinsPresented(this IJournal journal)
		{
			journal.Write("COINDSP: COINS PRESENTED");
		}

		public static void CashDispenserNotesTaken(this IJournal journal)
		{
			journal.Write("CASHDSP: NOTES TAKEN");
		}

		public static void CoinDispenserCoinsTaken(this IJournal journal)
		{
			journal.Write("COINDSP: COINS TAKEN");
		}

		public static void CashDispenserNotesRetracted(this IJournal journal)
		{
			journal.Write("CASHDSP: NOTES RETRACTED");
		}

		public static void CoinDispenserCoinsRetracted(this IJournal journal)
		{
			journal.Write("COINDSP: COINS RETRACTED");
		}

		public static void CashDispenserNotesNotTaken(this IJournal journal)
		{
			journal.Write("CASHDSP: NOTES NOT TAKEN");
		}

		public static void CoinDispenserCoinsNotTaken(this IJournal journal)
		{
			journal.Write("COINDSP: COINS NOT TAKEN");
		}
	}
}