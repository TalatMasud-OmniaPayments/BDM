namespace Omnia.Pie.Client.Journal.Interface.Extension
{
	using System.Collections.Generic;
	using Omnia.Pie.Client.Journal.Interface.Enum;
	using System.Globalization;
	using System.Threading.Tasks;
	using Omnia.Pie.Client.Journal.Interface.Dto;
	using Omnia.Pie.Vtm.Framework.Interface;

	public static partial class JournalExtensions
	{
		public static void PinEntered(this IJournal journal)
		{
			journal.Write("Pin entered");
		}

		public static void CardNumberEntered(this IJournal journal, string cardNumber)
		{
			journal.Write("Card Number Entered");

			if (cardNumber != null)
			{
				journal.Write(MaskCardNumber(cardNumber));
			}
		}

		public static void AmountEntered(this IJournal journal, string amount)
		{
			journal.Write($"Amount: {amount}");
		}

		public static void AmountEntered(this IJournal journal, int amount)
		{
			journal.Write($"Amount: {amount}");
		}

		public static void AmountEntered(this IJournal journal, double amount)
		{
			journal.Write($"Amount: {amount}");
		}

		public static void DateEntered(this IJournal journal, string date)
		{
			journal.Write($"Date: {date}");
		}

		public static void MICR(this IJournal journal, string MICR)
		{
			journal.Write($"MICR: {MICR}");
		}

		public static void AmountConverted(this IJournal journal, int amount, string originalCurrency, double exchangeRate)
		{
			journal.Write($"Converted Amount: {amount}");
			journal.Write($"Orig Curr: {originalCurrency} ExRt: {exchangeRate}");
		}

		public static void SenderMobileEntered(this IJournal journal, string senderMobile)
		{
			journal.Write($"Sender Mobile: {senderMobile}");
		}

		public static void BalanceLoaded(this IJournal journal, double? balance, string currency)
		{
			journal.Write($"Balance: {balance:F} {currency}");
		}

		public static async Task WriteReceiptAsync<TReceipt>(this IJournal journal, IReceiptFormatter receiptFormatter, TReceipt receipt)
		{
			var options = new ReceiptFormattingOptions
			{
				Culture = CultureInfo.GetCultureInfo("en-US"),  // Receipt should be always printed in English
				IsMarkupEnabled = false
			};

			var receiptText = await receiptFormatter.FormatAsync(receipt, options);
			journal.Write(receiptText, JournalTimestampStyle.None);
		}

		public static void RetractedCardsCleared(this IJournal journal, List<RetractedCardDto> retractedCardDtos)
		{
			journal.Write("Cleared retracted cards");

			foreach (var retractedCardDto in retractedCardDtos)
			{
				journal.Write(
					$"{retractedCardDto.MaskedNumber} - {retractedCardDto.Retracted.ToString("dd.MM.yy-HH:mm:ss", CultureInfo.InvariantCulture)}",
					JournalTimestampStyle.None);
			}

			journal.Write($"TOTAL = {retractedCardDtos.Count}");
		}

		public static void PasswordChanged(this IJournal journal, string id)
		{
			journal.Write($"Password has been changed for {id}");
		}

		private static string MaskCardNumber(string cardNumber)
		{
			return MaskedCardNumber(cardNumber);
		}

		public static void TerminalBranchIdChanged(this IJournal journal, string terminalId, string branchId)
		{
			journal.Write($"Terminal Id {terminalId} and Branch Id {branchId} has been changed.");
		}

		public static void PowerFailure(this IJournal journal)
		{
			journal.Write("Power Failure");
		}

		public static void PoweredUp(this IJournal journal)
		{
			journal.Write("Powered Up");
		}
	}
}