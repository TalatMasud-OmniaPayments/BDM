namespace Omnia.Pie.Vtm.Services.ISO.Request.Customer
{
	public class AccountDetailRequest : RequestBase
	{
		public string AccountNumber { get; set; }
		public string CustomerId { get; set; }
	}

    public class AccountDetailRequest1 : RequestBase
    {
        public string Username { get; set; }
        public string CustomerIdentifier { get; set; }
        public string ConditionId { get; set; }
    }
}
