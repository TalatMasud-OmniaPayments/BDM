namespace Omnia.Pie.Vtm.Framework.Interface
{
	using System.Threading.Tasks;

	public interface IReceiptFormatter
	{
		ReceiptMetadata Metadata { get; set; }
		Task<string> FormatAsync<T>(T receipt, ReceiptFormattingOptions formattingOptions = null);
	}
}