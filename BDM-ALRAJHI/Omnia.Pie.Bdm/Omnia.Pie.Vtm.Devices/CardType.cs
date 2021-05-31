namespace Omnia.Pie.Vtm.Devices
{
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using System;
	using System.Collections.Generic;
	using System.Configuration;

	public class CardTypeResolver : ICardTypeResolver
	{
		private readonly HashSet<string> _creditCardBins;
		private readonly HashSet<string> _debitCardBins;

		public CardTypeResolver()
		{
			var devicesSection = (DevicesSection)ConfigurationManager.GetSection(DevicesSection.Name);
			_creditCardBins = new HashSet<string>(devicesSection?.CreditCardBins.Bins.Split(','));
			_debitCardBins = new HashSet<string>(devicesSection?.DebitCardBins.Bins.Split(','));
		}

		public CardType? GetCardType(string cardNumber)
		{
			if (string.IsNullOrEmpty(cardNumber))
			{
				return null;
			}

			if (!char.IsDigit(cardNumber[0]))
			{
				cardNumber = cardNumber.Substring(1);
			}

			if (cardNumber.Length < 6)
			{
				throw new ArgumentException("PAN Length is less than 6 digit");
			}

			string bin = cardNumber.Substring(0, 6);

			if (_debitCardBins.Contains(bin))
			{
				return CardType.DebitCard;
			}

			if (_creditCardBins.Contains(bin))
			{
				return CardType.CreditCard;
			}

			return CardType.Offus;
		}
	}
}
