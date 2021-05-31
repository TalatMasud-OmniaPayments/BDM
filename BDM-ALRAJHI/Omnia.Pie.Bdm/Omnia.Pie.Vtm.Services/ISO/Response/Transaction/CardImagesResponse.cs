
namespace Omnia.Pie.Vtm.Services.ISO.Response.Transaction
{
	using System.Collections.Generic;

	public class CardImages
	{
		public string Track2 { get; set; }
		public string Pan { get; set; }
		public string ExpiryDate { get; set; }
		public int CardImageNo { get; set; }
	}


	public class CardImagesResponse : ResponseBase<List<CardImages>>
	{
		public List<CardImages> CardImages { get; set; }
	}
}