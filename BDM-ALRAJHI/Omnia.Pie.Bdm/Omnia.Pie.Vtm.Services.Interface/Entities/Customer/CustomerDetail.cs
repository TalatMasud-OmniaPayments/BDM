using System;

namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	public class CustomerDetail
	{
		public string FullName { get; set; }
		public string Address3 { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Country { get; set; }
		public string Nationality { get; set; }
		public string PassportNumber { get; set; }
		public DateTime? PassportExpiryDate { get; set; }
		public string VisaRefNumber { get; set; }
		public DateTime? VisaExpiryDate { get; set; }
		public string Email { get; set; }
		public string Gender { get; set; }
		public string BirthDate { get; set; }
		public string CustomerCategory { get; set; }
		public string CustomerInformationFileId { get; set; }
		public string CustomerStatus { get; set; }
		public string MobileNumber { get; set; }
		public string EmiratesId { get; set; }
		public string Salary { get; set; }
	}
}