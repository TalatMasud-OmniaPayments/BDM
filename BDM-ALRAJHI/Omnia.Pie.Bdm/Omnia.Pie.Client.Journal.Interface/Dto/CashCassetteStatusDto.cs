namespace Omnia.Pie.Client.Journal.Interface.Dto
{
	/// <summary>
	/// Represents status of cash in a particular cassette.
	/// </summary>
	public class CashCassetteStatusDto
	{
		public CashCassetteStatusDto(int value, string currency, int initialCount, int rejectedCount, int remainingCount)
		{
			Value = value;
			Currency = currency;
			InitialCount = initialCount;
			RejectedCount = rejectedCount;
			RemainingCount = remainingCount;
		}

		/// <summary>
		/// Value (nominal) of notes in a cassette.
		/// </summary>
		public int Value { get; }

		/// <summary>
		/// Currency of notes in a cassette.
		/// </summary>
		public string Currency { get; }

		/// <summary>
		/// Initial number of notes in a cassette (number of notes loaded by CIT specialist during last replenishment).
		/// </summary>
		public int InitialCount { get; }

		/// <summary>
		/// Number of notes from the cassette that have been rejected (not taken by customers).
		/// </summary>
		public int RejectedCount { get; }

		/// <summary>
		/// Number of notes remaining in the cassette.
		/// </summary>
		public int RemainingCount { get; }
	}


	/// <summary>
	/// Represents status of cash in a particular cassette.
	/// </summary>
	public class CoinCassetteStatusDto
	{
		public CoinCassetteStatusDto(int value, string currency, int initialCount, int rejectedCount, int remainingCount)
		{
			Value = value;
			Currency = currency;
			InitialCount = initialCount;
			RejectedCount = rejectedCount;
			RemainingCount = remainingCount;
		}

		/// <summary>
		/// Value (nominal) of notes in a cassette.
		/// </summary>
		public int Value { get; }

		/// <summary>
		/// Currency of notes in a cassette.
		/// </summary>
		public string Currency { get; }

		/// <summary>
		/// Initial number of notes in a cassette (number of notes loaded by CIT specialist during last replenishment).
		/// </summary>
		public int InitialCount { get; }

		/// <summary>
		/// Number of notes from the cassette that have been rejected (not taken by customers).
		/// </summary>
		public int RejectedCount { get; }

		/// <summary>
		/// Number of notes remaining in the cassette.
		/// </summary>
		public int RemainingCount { get; }
	}
}