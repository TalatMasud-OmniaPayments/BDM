using System;
using System.Collections.Generic;
using System.Linq;
using Omnia.Pie.Client.Journal.Interface.Dto;
using Omnia.Pie.Client.Journal.Interface.Enum;

namespace Omnia.Pie.Client.Journal.Interface.Extension
{
    public static partial class JournalExtensions
    {
		public static void CashAcceptorDepositActivated(this IJournal journal)
		{
			journal.Write("CASH DEPOSIT ACTIVATED");
		}

		public static void CashAcceptorDepositCanceled(this IJournal journal)
		{
			journal.Write("CASH DEPOSIT CANCELED");
		}

		public static void TransactionCanceled(this IJournal journal)
		{
			journal.Write("Transaction Canceled");
			journal.Write(Environment.NewLine, JournalTimestampStyle.None);
		}
        public static void userLogout(this IJournal journal, string user)
        {
            journal.Write(user + "Loggedout");
            journal.Write(Environment.NewLine, JournalTimestampStyle.None);
        }

        public static void CashAcceptorShutterOpened(this IJournal journal)
		{
			journal.Write("SHUTTER OPENED");
		}

		public static void CashAcceptorShutterClosed(this IJournal journal)
		{
			journal.Write("SHUTTER CLOSED");
		}

        public static void CashAcceptorShutterOpenedEvent(this IJournal journal)
        {
            journal.Write("SHUTTER OPENED EVENT");
        }

        public static void CashDispenserShutterClosedEvent(this IJournal journal)
        {
            journal.Write("Dispenser: SHUTTER CLOSED EVENT");
        }

        public static void CashDispenserShutterStatusChanged(this IJournal journal, string value)
        {
            journal.Write("Dispenser: SHUTTER STATUS" + value);
            journal.Write(Environment.NewLine, JournalTimestampStyle.None);
        }

        public static void CashDispenserShutterOpenedEvent(this IJournal journal)
        {
            journal.Write("Dispenser: SHUTTER OPENED EVENT");
        }

        public static void CashAcceptorShutterClosedEvent(this IJournal journal)
        {
            journal.Write("SHUTTER CLOSED EVENT");
        }

        public static void CashAcceptorItemsInserted(this IJournal journal)
		{
			journal.Write("CASH INSERTED");
		}

		public static void CashAcceptorItemsTaken(this IJournal journal)
		{
			journal.Write("CASHACC: ITEMS TAKEN");
		}

        public static void CashAcceptorCassetteChanged(this IJournal journal, string unitNumber)
        {

            journal.Write($"CASSETTE UNIT {unitNumber} CHANGED");
            
        }

        public static void CashAcceptorCassetteRemoved(this IJournal journal, string unitNumber)
        {

            journal.Write($"CASSETTE UNIT {unitNumber} Removed");

        }

        public static void CashAcceptorCassetteInstalled(this IJournal journal, string unitNumber, string status)
        {

            journal.Write($"CASSETTE UNIT {unitNumber} {status}");

        }

        public static void CashAcceptorCashAccepted(this IJournal journal, IEnumerable<CashInStatusDto> cashStatuses, int rejectedItemsCount)
		{
			int totalItemsCount = GetTotalItemsCount(cashStatuses);

			journal.Write($"ESCROW: {totalItemsCount:D3}");
			WriteCash(cashStatuses, journal);
			journal.Write($"VAULTED: {0:D3}");
			journal.Write($"REFUNDED: {0:D3}");
			journal.Write($"RETRACTED: {0:D3}");
			journal.Write($"REJECTS: {rejectedItemsCount:D3}");
		}

		public static void CashAcceptorCashStored(this IJournal journal, IEnumerable<CashInStatusDto> cashStatuses, int rejectedItemsCount)
		{
			int totalItemsCount = GetTotalItemsCount(cashStatuses);

			journal.Write($"ESCROW  : {0:D3}");
			journal.Write($"VAULTED: {totalItemsCount:D3}");
			WriteCash(cashStatuses, journal);
			journal.Write($"REFUNDED: {0:D3}");
			journal.Write($"RETRACTED: {0:D3}");
			journal.Write($"REJECTS: {rejectedItemsCount:D3}");

			journal.Write("CASH DEPOSIT COMPLETED");
		}

		public static void CashAcceptorCashRefunded(this IJournal journal, IEnumerable<CashInStatusDto> cashStatuses, int rejectedItemsCount)
		{
			int totalItemsCount = GetTotalItemsCount(cashStatuses);

			journal.Write("CASH REFUNDED");
			journal.Write($"ESCROW: {0:D3}");
			journal.Write($"VAULTED: {0:D3}");
			journal.Write($"REFUNDED: {totalItemsCount:D3}");
			WriteCash(cashStatuses, journal);
			journal.Write($"RETRACTED: {0:D3}");
			journal.Write($"REJECTS: {rejectedItemsCount:D3}");
		}

		public static void CashAcceptorCashRetracted(this IJournal journal, IEnumerable<CashInStatusDto> cashStatuses, int rejectedItemsCount)
		{
			int totalItemsCount = GetTotalItemsCount(cashStatuses);

			journal.Write("CASH RETRACTED");
			journal.Write($"ESCROW: {0:D3}");
			journal.Write($"VAULTED: {0:D3}");
			journal.Write($"REFUNDED: {0:D3}");
			journal.Write($"RETRACTED: {totalItemsCount:D3}");
			WriteCash(cashStatuses, journal);
			journal.Write($"REJECTS: {rejectedItemsCount:D3}");
		}

		#region Helper Formatting Methods

		private static void WriteCash(IEnumerable<CashInStatusDto> cashStatuses, IJournal journal)
		{
			foreach (var cashStatusesOfSameCurrency in cashStatuses.GroupBy(x => x.Currency))
			{
				string currency = cashStatusesOfSameCurrency.Key;

				int totalAmount = 0;
				foreach (CashInStatusDto cashStatus in cashStatusesOfSameCurrency)
				{
					int amountInCassette = cashStatus.Value * cashStatus.ItemCount;
					journal.Write($"{currency} {cashStatus.Value} X {cashStatus.ItemCount} = {amountInCassette}");

					totalAmount += amountInCassette;
				}

				journal.Write($"{currency} TOTAL = {totalAmount}");
			}
		}

		/// <summary>
		/// Returns total number of bills in all cassettes.
		/// </summary>
		/// <param name="cashStatuses">Statuses of cash in each cassette.</param>
		/// <returns>Total number of bills in all cassettes.</returns>
		private static int GetTotalItemsCount(IEnumerable<CashInStatusDto> cashStatuses)
		{
			return cashStatuses.Sum(x => x.ItemCount);
		}

		#endregion
	}
}
