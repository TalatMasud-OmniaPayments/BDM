namespace Omnia.Pie.Vtm.Services.ISO
{
	public abstract class RequestBase
	{
		public TransactionData TransactionData { get; set; }
		public VTMTerminal Terminal { get; set; }
	}
}