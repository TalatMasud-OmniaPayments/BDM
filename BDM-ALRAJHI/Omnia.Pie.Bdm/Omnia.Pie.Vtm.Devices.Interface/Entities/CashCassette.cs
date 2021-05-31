using Omnia.Pie.Vtm.Devices.Interface.Exceptions;

namespace Omnia.Pie.Vtm.Devices.Interface.Entities
{
	public class CashCassette
	{
		public int DepositedCount { get; set; }
		public int RetractedCount { get; set; }
		public int DispensedCount { get; set; }
		public int RejectedCount { get; set; }
		public int Bill100Count { get; set; }
		public int Bill200Count { get; set; }
		public int Bill500Count { get; set; }
		public int Bill1000Count { get; set; }
	}

	public class CassetteInfo
	{
		public CassetteInfo(int value, int count, int index, string type)
		{
			Value = value;
			Count = count;
			Index = index;
            Type = type;
        }

		public int Index { get; }
		public int Count { get; private set; }
		public int Value { get; }
        public string Type { get; private set; }

        /// <summary>
        /// Try dispense <param name="count"></param> notes form cassette.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public int GiveCount(int count)
		{
			if (Count < count)
			{
				throw new DenominateException($"Cannot dispense {count} notes from cassete with index {Index}.");
			}

			Count -= count;
			return count;
		}
	}
}