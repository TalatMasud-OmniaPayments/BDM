namespace Omnia.Pie.Vtm.Devices.CoinDispenser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Services.Interface.Entities;

	internal abstract partial class DenominateOperation
	{
		public readonly List<CassetteInfo> Cassettes;

		protected int CassetsCount { get; }

		protected DenominateOperation(MediaUnit[] mediaUnit, string currency)
		{
			CassetsCount = mediaUnit.Length;
			Cassettes = new List<CassetteInfo>();

			Cassettes.AddRange(mediaUnit
				.Where(u => u.Type.Equals(DispenserUnitType.CoinCassette, StringComparison.OrdinalIgnoreCase) && u.Currency == currency)
				.OrderByDescending(c => c.Value)
				.Select(u => new CassetteInfo(u.Value, u.Count, u.Id - 1, u.Type)));
		}

		public abstract int[] Execute(int amount);
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

        public int GiveCount(int coinCount)
		{
			if (Count < coinCount)
			{
				throw new DenominateException($"Cannot dispense {coinCount} Coins from cassete with index {Index}.");
			}

			Count -= coinCount;
			return coinCount;
		}
	}

	internal sealed class DenominateOperationWithChange : DenominateOperation
	{
		private const int NotesCountForChange = 1;
		public List<List<Denomination>> Denominations { get; set; }

		public List<CassetteInfo> GetCessettes()
		{
			return Cassettes;
		}

		public DenominateOperationWithChange(MediaUnit[] mediaUnit, string currency) : base(mediaUnit, currency)
		{

		}

		public int GetCasssettesCount()
		{
			return CassetsCount;
		}

		public override int[] Execute(int amount)
		{
			if (amount <= 0)
			{
				throw new ArgumentException("Amount should be more than 0.");
			}

			int[] coinsCount = new int[CassetsCount];

			if (AmountContainsFills(amount))
			{
				var remainingAmount = TryGiveSmallestDenomination(amount, out coinsCount);
				coinsCount = DenominateRemainingAmount(amount, remainingAmount, coinsCount);
			}
			else
			{
				coinsCount = DenominateRemainingAmount(amount, amount, coinsCount);
			}

			return coinsCount;
		}

		private int[] DenominateRemainingAmount(int amount, int remainingAmount, int[] coinsCount)
		{
			if (coinsCount == null)
				coinsCount = new int[CassetsCount];

			remainingAmount = remainingAmount / 100;
			while (remainingAmount > 0)
			{
				var cassete = Cassettes.LastOrDefault(c => c.Count > 0 && c.Value <= remainingAmount);
				if (cassete == null) throw new DenominateException(amount);

				remainingAmount -= cassete.Value;
				coinsCount[cassete.Index] += cassete.GiveCount(1);
			}

			return coinsCount;
		}

		public void GetDenominations(int highest, int sum, int goal, List<int> coins)
		{
			if (Denominations?.Count == 2)
			{
				return;
			}
			if (sum == goal)
			{
				List<Denomination> denom = new List<Denomination>();
				foreach (var amount in Cassettes)
				{
					int count = coins.Count(value => value == amount.Value);
					denom.Add(new Denomination() { Amount = amount.Value, Count = count, CassettePresent = true });
				}
				bool possible = true;
				foreach (var item in denom)
				{
					IEnumerable<CassetteInfo> cas = Cassettes.Where(x => x.Value == item.Amount);
					if (!(cas.ElementAt(0).Count >= item.Count))
					{
						possible = false;
					}
				}
				if (possible)
				{
					if (Denominations == null)
					{
						Denominations = new List<List<Denomination>>();
					}
					if (Denominations?.Count == 0)
					{
						if ((denom[0]?.Count + denom[1]?.Count + denom[2]?.Count + denom[3]?.Count) > 200)
						{
							throw new Exception("Denominations not found.");
						}
					}
					if (Denominations?.Count == 1)
					{
						if ((denom[0]?.Count + denom[1]?.Count + denom[2]?.Count + denom[3]?.Count) > 200)
						{
							Denominations.Add(Denominations[0]);
							return;
						}
					}
					if ((denom[0]?.Count + denom[1]?.Count + denom[2]?.Count + denom[3]?.Count) <= 200)
					{
						Denominations.Add(denom);
						return;
					}
				}
				else
					return;
			}

			if (sum > goal)
			{
				return;
			}

			foreach (var value in Cassettes)
			{
				if (value.Value >= highest)
				{
					List<int> copy = new List<int>(coins);
					copy.Add(value.Value);
					GetDenominations(value.Value, sum + value.Value, goal, copy);
				}
			}
		}

		private int TryGiveSmallestDenomination(int amount, out int[] coinsCount)
		{
			coinsCount = new int[CassetsCount];
			var remainingAmount = amount;

			var minimumDenominationCassette = Cassettes[0];
			var minimumDenomination = minimumDenominationCassette.Value;

			var amountForExchange = minimumDenomination * NotesCountForChange;
			var modulo = amount % amountForExchange;
			var needMinimalNotesCount = modulo == 0 ? NotesCountForChange : modulo / minimumDenomination;
			var isEnoughNotes = minimumDenominationCassette.Count >= needMinimalNotesCount;

			if (isEnoughNotes)
			{
				remainingAmount = remainingAmount - (needMinimalNotesCount * minimumDenomination);
				coinsCount[minimumDenominationCassette.Index] = minimumDenominationCassette.GiveCount(needMinimalNotesCount);
			}
			else if (modulo == 0)
			{
				var minimumAvilablCassette = Cassettes.FirstOrDefault(c => c.Count >= needMinimalNotesCount);
				if (minimumAvilablCassette == null)
				{
					throw new DenominateException(amount);
				}
				remainingAmount = remainingAmount - minimumAvilablCassette.Value;
				coinsCount[minimumAvilablCassette.Index] = minimumAvilablCassette.GiveCount(1);
			}
			else
			{
				throw new DenominateException(amount);
			}

			return remainingAmount;
		}

		private bool AmountContainsFills(int amount)
		{
			return (amount % 100) == 50;
		}
	}
}