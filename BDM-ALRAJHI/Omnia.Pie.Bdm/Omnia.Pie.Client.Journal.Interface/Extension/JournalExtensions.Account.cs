using System;
using System.Text.RegularExpressions;

namespace Omnia.Pie.Client.Journal.Interface.Extension
{
	public static partial class JournalExtensions
	{
		public static void AccountSelected(this IJournal journal, string accountNumber)
		{
            journal.Write($"Account Selected: {accountNumber}");
        }

		public static void SelectedReceiptSelection(this IJournal journal, string receiptType)
		{
			journal.Write($"Receipt Selected: {(receiptType)}");
		}

		public static void CardSelected(this IJournal journal, string cardNumber)
		{
			journal.Write($"Card Selected: {MaskCardNumber(cardNumber)}");
		}

		public static void SourceAccountSelected(this IJournal journal, string accountNumber)
		{
            journal.Write($"Src Account: {accountNumber}");
        }

		public static void DestinationAccountSelected(this IJournal journal, string accountNumber)
		{
            journal.Write($"Dest Account: {accountNumber}");
        }

		public static void BeneficiarySelected(this IJournal journal, string accountIban)
		{
            journal.Write($"Beneficiary: {accountIban}");
        }

		public static void NumberOfChequesSelected(this IJournal journal, string numberOfCheques)
		{
			journal.Write($"Number of Cheques Selected: {numberOfCheques}");
		}

		public static void NumberOfChequesPrinting(this IJournal journal, string numberOfCheques)
		{
			journal.Write($"Number of Cheques Printing : {numberOfCheques}");
		}

		public static void ChequeNumberPrinted(this IJournal journal, string number)
		{
			journal.Write($"Cheque with Number : {number} Printed.");
		}

		public static void CIF(this IJournal journal, string CIF)
		{
			journal.Write($"Customer CIF: {CIF}");
		}

		public static void BeneficiaryEntered(this IJournal journal, string id, string name, string accountNumber, string mobile)
		{
			if (id != null)
			{
				journal.Write($"id: {id}");
			}
			if (name != null)
			{
				journal.Write($"name: {name}");
			}
			if (accountNumber != null)
			{
                journal.Write($"account number: {accountNumber}");
            }
			if (mobile != null)
			{
				journal.Write($"mobile: {mobile}");
			}
		}

        public static string MaskedCardNumber(string v)
        {
            char MaskSymbol = '*';
            string MaskPattern = "^(.{4})(.+)(.{4})$";

            var result = v;

            if (!string.IsNullOrEmpty(result))
            {
                result = Regex.Replace(result, MaskPattern, m => $"{m.Groups[1]}{new String(MaskSymbol, m.Groups[2].Length)}{m.Groups[3]}");
            }

            return result;
        }

        public static string MaskAccountNumber(string v)
        {
            char MaskSymbol = '*';
            string MaskPattern = "^(.{3})(.+)(.{3})$";

            var result = v;

            if (!string.IsNullOrEmpty(result))
            {
                result = Regex.Replace(result, MaskPattern, m => $"{m.Groups[1]}{new String(MaskSymbol, m.Groups[2].Length)}{m.Groups[3]}");
            }

            return result;
        }

        /*
        private static string MaskAccountNumber(string accountNumber)
		{
			if (string.IsNullOrEmpty(accountNumber))
			{
				return accountNumber;
			}

			if (accountNumber.Length <= 6)
			{
				return accountNumber;
			}

			const int MinObfuscationLength = 3;
			if (accountNumber.Length <= MinObfuscationLength)
			{
				return new string('*', accountNumber.Length);
			}

			int nonObfuscatedPartSize = 6;
			int nonObfuscatedPartSizeEnd = 4;

			while (accountNumber.Length < 2 * nonObfuscatedPartSize + MinObfuscationLength)
			{
				nonObfuscatedPartSize -= 1;
			}
			string nonObfuscatedStart = accountNumber.Substring(0, nonObfuscatedPartSize);
			string nonObfuscatedEnd = accountNumber.Substring(accountNumber.Length - nonObfuscatedPartSizeEnd);

			return nonObfuscatedStart + new string('*', accountNumber.Length - 2 * nonObfuscatedPartSize) + nonObfuscatedEnd;
		}
        */
	}
}