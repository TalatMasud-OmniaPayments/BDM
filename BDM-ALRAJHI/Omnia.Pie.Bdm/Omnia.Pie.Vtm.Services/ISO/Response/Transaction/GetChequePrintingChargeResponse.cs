namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction.ChequeCharges
{
	public class Charge
	{
		public string ChargeAmount { get; set; }
		public string ChargeCurrency { get; set; }
	}

	public class GetChequePrintingChargeResponse : ResponseBase<Charge>
	{
		public Charge Charge { get; set; }
	}
}