namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	public class OffUsCashWithdrawal
	{
		public string AuthCode { get; set; }
		public string Currency { get; set; }
		public double? AvailableBalance { get; set; }
		public bool IsUaeSwitchTransaction { get; set; }
		public string IccData { get; set; }
	}
}