namespace Omnia.Pie.Vtm.DataAccess.Stores
{
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.DataAccess.Interface.Entities;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System.Threading.Tasks;

	internal class JournalMessageStore : StoreBase, IJournalMessageStore
	{
		public JournalMessageStore(IResolver container) : base(container)
		{
			ExecuteCommand(@"
				CREATE TABLE IF NOT EXISTS [JournalMessages] (
					[Id]                    INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT,
					[Text]                  TEXT            NOT NULL,
					[Timestamp]             TEXT            NOT NULL,
					[TimestampStyle]        INTEGER         NOT NULL
				)");
		}

		public Task ClearAll()
		{
			return ExecuteCommand(@"
				DELETE FROM [JournalMessages]");
		}

		public Task Delete(JournalMessage message)
		{
			return ExecuteCommand(@"
				DELETE FROM [JournalMessages]
				WHERE Id = @Id", message);
		}

		public Task<JournalMessage> Get()
		{
			return QueryFirstOrDefault<JournalMessage>(@"
				SELECT [Id]
					,[Text]
					,[Timestamp]
					,[TimestampStyle]
				FROM [JournalMessages]
				ORDER BY [Id]");
		}

		public Task Save(JournalMessage message)
		{
			return ExecuteCommand(@"
				INSERT INTO [JournalMessages] (
					[Text]
					,[Timestamp]
					,[TimestampStyle]
				) VALUES (
					@Text
					,@Timestamp
					,@TimestampStyle
				)", message);
		}
	}
}