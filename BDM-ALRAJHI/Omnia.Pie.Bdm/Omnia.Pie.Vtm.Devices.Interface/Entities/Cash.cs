namespace Omnia.Pie.Vtm.Devices.Interface.Entities
{
	using System.Linq;

	public class Cash
	{
		public int TotalNotes { get; set; }
		public int TotalAmount { get; set; }
		public CashDenomination[] Denominations { get; set; }

		public int GetNotesCount(int value)
		{
			var denomination = Denominations?.FirstOrDefault(i => i.Value == value);
			if (denomination == null)
			{ return 0; }
			return denomination.Count;
		}
        public Cash() { }
        public Cash(Cash other)
        {
            TotalNotes = other.TotalNotes;
            TotalAmount = other.TotalAmount;
            Denominations = other.Denominations;
        }

    }

}