namespace Omnia.Pie.Vtm.Services.Interface.Entities.Customer
{
	public class GetLinkedAccountResult
	{
		public string Name { get; set; }
		public string Type { get; set; }
		public string TypeCode { get; set; }
		public string Currency { get; set; }
		public string CurrencyCode { get; set; }
		public string Number { get; set; }
		public string AvailableBalance { get; set; }
		public string AccountingUnitId { get; set; }
		public string ConditionId { get; set; }
	}
}