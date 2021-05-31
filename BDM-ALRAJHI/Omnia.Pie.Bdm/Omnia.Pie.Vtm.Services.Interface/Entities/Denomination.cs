namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	public class Denomination
	{
		public int Amount { get; set; }
		public int Count { get; set; }
		public int Value { get; set; }
		public bool CassettePresent { get; set; } = false;
	}
}