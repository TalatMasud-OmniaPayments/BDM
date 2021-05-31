namespace Omnia.Pie.Vtm.Services.ISO.Customer
{
	public class CustomerDetails
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }
		public string Nationality { get; set; }
		public string BirthDate { get; set; }
		public string CustomerCategory { get; set; }
		public string CifId { get; set; }
		public string PassportNumber { get; set; }
		public string PassportExpiry { get; set; }
		public string CustomerStatus { get; set; }
		public string MobileNumber { get; set; }
		public string Email { get; set; }
		public string VisaRefNumber { get; set; }
		public string VisaExpiry { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string AddressLine3 { get; set; }
		public string Country { get; set; }
		public string EmiratesId { get; set; }
		public string Salary { get; set; }
	}

	public class CustomerDetailsResponse : ResponseBase<CustomerDetails>
	{
		public CustomerDetails CustomerDetail { get; set; }
	}

}