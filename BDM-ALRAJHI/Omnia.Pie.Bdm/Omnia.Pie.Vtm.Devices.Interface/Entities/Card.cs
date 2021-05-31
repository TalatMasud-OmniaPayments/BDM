namespace Omnia.Pie.Vtm.Devices.Interface.Entities
{
	public class Card
	{
		private IEmvData _emvData;
		public string AccountName { get; set; }
		public string CardNumber { get; set; }
		public string Track1 { get; set; }
		public string Track2 { get; set; }
		public string Track3 { get; set; }
		public string ChipData1 { get; set; }
		public string ChipData2 { get; set; }
		public string ChipData3 { get; set; }
		public CardType? CardType { get; set; }
		public EmiratesId EmiratesId { get; set; }
		public IEmvData EmvData
		{
			get { return _emvData; }
			set
			{
				_emvData = value;
				if (_emvData != null && !string.IsNullOrEmpty(_emvData.Track2))
				{
					Track2 = _emvData.Track2;
				}
				if (_emvData != null && !string.IsNullOrEmpty(_emvData.CardNumber))
				{
					CardNumber = _emvData.CardNumber;
				}
			}
		}
	}
}