using System;

namespace Omnia.Pie.Client.Journal.Interface.Dto
{
	public class RetractedCardDto
	{
		public RetractedCardDto(DateTime retracted, string maskedNumber)
		{
			Retracted = retracted;
			MaskedNumber = maskedNumber;
		}

		public DateTime Retracted { get; }

		public string MaskedNumber { get; }
	}
}