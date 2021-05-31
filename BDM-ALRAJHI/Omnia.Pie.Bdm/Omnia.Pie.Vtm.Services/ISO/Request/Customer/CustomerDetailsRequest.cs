namespace Omnia.Pie.Vtm.Services.ISO.Customer
{

    public class EnquiryAccount
    {
        public string Username { get; set; }
    }

    public class CustomerDetailsRequest: RequestBase
	{
		public string Username { get; set; }
        public EnquiryAccount UserAccount { get; set; }
    }
}