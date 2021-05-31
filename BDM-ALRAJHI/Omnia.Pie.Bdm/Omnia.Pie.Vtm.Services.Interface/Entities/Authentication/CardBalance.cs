namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	public class CardBalance
	{
		public string Currency { get; set; }
		public double? AvailableBalance { get; set; }
		public bool IsUaeSwitchTransaction { get; set; }
		public string IccData { get; set; }
	}
}