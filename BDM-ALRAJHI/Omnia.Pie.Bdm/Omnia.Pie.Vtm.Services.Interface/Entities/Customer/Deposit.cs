namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	public class Deposit
	{
		public string Type { get; set; }
		public string Number { get; set; }
		public double? Balance { get; set; }
		public string Currency { get; set; }
	}
}