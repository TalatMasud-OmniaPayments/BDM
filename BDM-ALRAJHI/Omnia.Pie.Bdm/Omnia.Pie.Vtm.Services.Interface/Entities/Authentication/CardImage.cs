namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
	public class CardImage
	{
		public string Track2 { get; set; }
		public string Pan { get; set; }
		public string ExpiryDate { get; set; }
		public int CardImageNo { get; set; }
		public string CardImageSource
		{
			get
			{
				return $"/Resources/Images/Cards/0{CardImageNo}.jpg";
			}
		}
	}
}