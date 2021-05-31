namespace Omnia.Pie.Vtm.Devices.Interface
{
	public interface ICardTypeResolver
	{
		CardType? GetCardType(string cardNumber);
	}
}