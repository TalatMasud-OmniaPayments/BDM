namespace Omnia.Pie.Vtm.ServicesNdc.Interface.Entities
{
	public class NdcCard
	{
		public string CardNumber { get; set; }
		public string CardFDK { get; set; }
		public string ImageName { get; set; }
		public string CardImageSource
		{
			get
			{
				return $"/Resources/Images/Cards/{ImageName}.jpg";
			}
		}
	}
}