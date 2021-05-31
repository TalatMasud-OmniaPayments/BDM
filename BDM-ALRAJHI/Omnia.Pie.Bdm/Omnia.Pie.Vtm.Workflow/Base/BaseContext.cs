namespace Omnia.Pie.Vtm.Workflow
{
	using Omnia.Pie.Vtm.Framework.Interface;

	public interface IBaseContext
	{
		TransactionType TransactionType { get; set; }
		string CustomerId { get; set; }
	}

	public class BaseContext : IBaseContext
	{
		public TransactionType TransactionType { get; set; }
		public string CustomerId { get; set; }
	}

	public enum CardNumberEntryType
	{
		None,
		CardInserted,
		ManualEntry
	}
}