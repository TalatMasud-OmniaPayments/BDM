using System.Collections.Generic;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.DataAccess.Interface;
using Omnia.Pie.Vtm.DataAccess.Interface.Entities;
using Omnia.Pie.Vtm.Framework.Interface;

namespace Omnia.Pie.Vtm.DataAccess.Stores
{
	internal class RetractedCardStore : StoreBase, IRetractedCardStore
	{
		public RetractedCardStore(IResolver container) : base(container)
		{
			ExecuteCommand(@"
				CREATE TABLE IF NOT EXISTS [RetractedCards] (
					[Id]           INTEGER      NOT NULL PRIMARY KEY AUTOINCREMENT,
					[Retracted]    TEXT         NOT NULL,
					[MaskedNumber] NVARCHAR(32) NOT NULL
				)");
		}

		public Task ClearAll()
		{
			return ExecuteCommand(@"
				DELETE FROM [RetractedCards]");
		}

		public Task<List<RetractedCard>> GetList()
		{
			return ExecuteQuery<RetractedCard>(@"
				SELECT
					[Retracted]
					,[MaskedNumber]
				FROM [RetractedCards]
				ORDER BY [Id]");
		}

		public Task Save(RetractedCard card)
		{
			return ExecuteCommand(@"
				INSERT INTO [RetractedCards] (
					[Retracted]
					,[MaskedNumber]
				) VALUES (
					@Retracted
					,@MaskedNumber
				)", card);
		}
	}
}