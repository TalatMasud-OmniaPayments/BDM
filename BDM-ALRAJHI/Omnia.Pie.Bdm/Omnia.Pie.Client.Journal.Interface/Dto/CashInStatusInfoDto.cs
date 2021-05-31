namespace Omnia.Pie.Client.Journal.Interface.Dto
{
	public class CashInStatusDto
	{
		public CashInStatusDto(int value, string currency, int itemCount)
		{
			Value = value;
			Currency = currency;
			ItemCount = itemCount;
		}

		public int Value { get; }
		public string Currency { get; }
		public int ItemCount { get; }
	}
}