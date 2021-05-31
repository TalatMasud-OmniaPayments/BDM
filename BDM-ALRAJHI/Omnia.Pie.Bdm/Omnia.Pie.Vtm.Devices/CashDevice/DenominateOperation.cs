namespace Omnia.Pie.Vtm.Devices.CashDispenser.Denominate
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
				//.Where(u => u.Currency == "AED")
				.OrderBy(c => c.Value)
				.Select(u => new CassetteInfo(u.Value, u.Count, u.Id - 1, u.Type)));      // This is the older code. Talat is commenting it to read all types of cassettes and send the status to use in channel management service.
                                                                                          //list.AddRange(GetBigCars().Where(bigcar => !list.Contains(bigcar, car => car.id == bigcar.id)));
            Cassettes = Cassettes.Distinct().ToList();
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

        /// <summary>
        /// Try dispense <param name="notesCount"></param> notes form cassette.
        /// </summary>
        /// <param name="notesCount"></param>
        /// <returns></returns>
        public int GiveCount(int notesCount)
		{
			if (Count < notesCount)
			{
				throw new DenominateException($"Cannot dispense {notesCount} notes from cassete with index {Index}.");
			}

			Count -= notesCount;
			return notesCount;
		}
	}

	internal sealed class DenominateOperationWithChange : DenominateOperation
	{
		private const int NotesCountForChange = 5;
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

			int[] notesCount;
			var remainingAmount = TryGiveSmallestDenomination(amount, out notesCount);
			notesCount = DenominateRemainingAmount(amount, remainingAmount, notesCount);

			return notesCount;
		}

		private int[] DenominateRemainingAmount(int amount, int remainingAmount, int[] notesCount)
		{
			while (remainingAmount > 0)
			{
				var cassete = Cassettes.LastOrDefault(c => c.Count > 0 && c.Value <= remainingAmount);
				if (cassete == null) throw new DenominateException(amount);

				remainingAmount -= cassete.Value;
				notesCount[cassete.Index] += cassete.GiveCount(1);
			}

			return notesCount;
		}
		
		public void GetDenominations(int highest, int sum, int goal, List<int> notes)
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
					int count = notes.Count(value => value == amount.Value);
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
					List<int> copy = new List<int>(notes);
					copy.Add(value.Value);
					GetDenominations(value.Value, sum + value.Value, goal, copy);
				}
			}
		}

		private int TryGiveSmallestDenomination(int amount, out int[] notesCount)
		{
			notesCount = new int[CassetsCount];
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
				notesCount[minimumDenominationCassette.Index] = minimumDenominationCassette.GiveCount(needMinimalNotesCount);
			}
			else if (modulo == 0)
			{
				var minimumAvilablCassette = Cassettes.FirstOrDefault(c => c.Count >= needMinimalNotesCount);
				if (minimumAvilablCassette == null)
				{
					throw new DenominateException(amount);
				}
				remainingAmount = remainingAmount - minimumAvilablCassette.Value;
				notesCount[minimumAvilablCassette.Index] = minimumAvilablCassette.GiveCount(1);
			}
			else
			{
				throw new DenominateException(amount);
			}

			return remainingAmount;
		}
	}
}