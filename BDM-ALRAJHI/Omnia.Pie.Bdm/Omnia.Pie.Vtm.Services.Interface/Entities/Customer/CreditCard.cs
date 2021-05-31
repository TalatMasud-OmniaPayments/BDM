namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	using System;

	public class CreditCard
	{
		/// <remarks>
		/// Originally the Name was not provided in the mapping. The property was added to support Title (Name) in UC50.
		/// </remarks>
		public string Name { get; set; }
		public string Type { get; set; }
		public string Number { get; set; }
		public string Currency { get; set; }
		public double? AvailableLimit { get; set; }
		public DateTime? LastStatementDue { get; set; }
		public double? CardLimit { get; set; }
		public double? CurrentOutstanding { get; set; }
		public double? MinimumPayment { get; set; }
		public DateTime? MinimumPaymentDue { get; set; } // or PaymentDueDate
		public double? OpeningBalance { get; set; }
	}
}