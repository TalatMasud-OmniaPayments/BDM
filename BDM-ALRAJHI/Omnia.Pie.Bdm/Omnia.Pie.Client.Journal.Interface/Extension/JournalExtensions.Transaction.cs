using System;
using Omnia.Pie.Client.Journal.Interface.Enum;

namespace Omnia.Pie.Client.Journal.Interface.Extension
{
	public static partial class JournalExtensions
	{
		public static void DepositTransactionsSelected(this IJournal journal)
		{
			journal.Write("Deposit Transactions");
		}

		public static void TransactionStarted(this IJournal journal)
		{
			journal.Write(Environment.NewLine, JournalTimestampStyle.None);
			journal.Write("Transaction Started", JournalTimestampStyle.DateTime);
		}

		public static void TransactionStarted(this IJournal journal, EJTransactionType type)
		{
			journal.Write(Environment.NewLine, JournalTimestampStyle.None);
			journal.Write($"Transaction Started {type.ToString()}", JournalTimestampStyle.DateTime);
		}

		public static void TransactionStarted(this IJournal journal, EJTransactionType type, string transactionName)
		{
			journal.Write(Environment.NewLine, JournalTimestampStyle.None);
			journal.Write($"Transaction {transactionName} {type.ToString()} Started", JournalTimestampStyle.DateTime);
		}

		public static void TransactionName(this IJournal journal, string transactionName)
		{
			if (!string.IsNullOrEmpty(transactionName))
			{
				journal.Write(transactionName);
			}
		}

		public static void TransactionSucceeded(this IJournal journal, string transactionNumber, string referenceNumber)
		{
			journal.Write("Transaction Successful");
			if (transactionNumber != null)
			{
				journal.Write($"TSN: {transactionNumber}");
			}
			if (referenceNumber != null)
			{
				journal.Write($"RefNo: {referenceNumber}");
			}
		}
		public static void TransactionSucceeded(this IJournal journal, string transactionNumber)
		{
			journal.Write("Transaction Successful");
			if (transactionNumber != null)
			{
				journal.Write($"TSN: {transactionNumber}");
			}
		}

        public static void TransactionSucceeded( this IJournal journal)
        {
            journal.Write("Transaction Successful");           
        }

        public static void TransactionFailed(this IJournal journal, string errorMessage)
		{
			journal.Write("Transaction Failed");

			if (errorMessage != null)
			{
				journal.Write($"Error: {errorMessage}");
			}
		}

		public static void TransactionFailed(this IJournal journal, string transactionNumber, string errorMessage)
		{
			journal.Write("Transaction Failed");
			if (transactionNumber != null)
			{
				journal.Write($"TSN: {transactionNumber}");
			}
			if (errorMessage != null)
			{
				journal.Write($"Error: {errorMessage}");
			}
		}

		public static void TransactionEnded(this IJournal journal)
		{
			journal.Write("Transaction Ended");
			journal.Write(Environment.NewLine, JournalTimestampStyle.None);
		}

		public static void ReversalTransactionStarted(this IJournal journal)
		{
			journal.Write("Reverting Transaction");
		}

		public static void ReversalTransactionSucceded(this IJournal journal, string authorizationCode)
		{
			journal.Write("Transaction Reverted");
			if (authorizationCode != null)
			{
				journal.Write($"AuthCode: {authorizationCode}");
			}
		}

		public static void ReversalTransactionFailed(this IJournal journal, string errorMessage)
		{
			journal.Write("Reverting Failed");
			if (errorMessage != null)
			{
				journal.Write($"Error: {errorMessage}", JournalTimestampStyle.None);
			}
		}

        public static void CryptogramValidationFailure(this IJournal journal)
        {
            journal.Write("Cryptogram Validation Failed");
        }

        public static void NotesInserted(this IJournal journal, string NotesID)
        {
            journal.Write(Environment.NewLine, JournalTimestampStyle.None);
            journal.Write($"Note(s) Inserted: {NotesID}", JournalTimestampStyle.DateTime);
        }

        public static void DeviceStatus(this IJournal journal, string NotesID)
        {
            //journal.Write(Environment.NewLine, JournalTimestampStyle.None);
            //journal.Write($"Note Inserted {NotesID}", JournalTimestampStyle.DateTime);
        }

    }

	public enum EJTransactionType
	{
		Financial,
        NonFinancial,
	}
}