namespace Omnia.Pie.Vtm.DataAccess.Stores
{
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.DataAccess.Interface.Entities;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	internal class DeviceErrorStore : StoreBase, IDeviceErrorStore
	{
		public DeviceErrorStore(IResolver container) : base(container)
		{
			ExecuteCommand(@"
				CREATE TABLE IF NOT EXISTS [DeviceErrors] (
					[Id]		INTEGER			NOT NULL PRIMARY KEY AUTOINCREMENT,
					[Source]	NVARCHAR(128)	NOT NULL,
					[Created]	TEXT			NOT NULL,
					[Message]	NVARCHAR(1024)	NOT NULL,
					[StatusSent] INTEGER		NOT NULL
				)");
		}

		public Task ClearAll()
		{
			return ExecuteCommand(@"
				DELETE FROM [DeviceErrors]");
		}

		public Task Save(DeviceError deviceError)
		{
			return ExecuteCommand(@"
				INSERT INTO [DeviceErrors] (
					 [Source]
					,[Created]
					,[Message]
					,[StatusSent]
				) VALUES (
					@Source
					,@Created
					,@Message
					,@StatusSent
				)", deviceError);
		}

		public async Task<string> GetList(string devName)
		{
			var list = await ExecuteQuery<DeviceError>($@"
				SELECT
					 [Source]
					,[Created]
					,[Message]
					,[StatusSent]
				FROM [DeviceErrors]
				WHERE 
					[Source] == '{devName}'
					AND
					[StatusSent] == 0
				ORDER BY [Id]");

			var result = string.Join(",", list.Select(x => x.Message));

			list.ForEach(x => x.StatusSent = 1);
			list.ForEach(x => Save(x));

			return result;
		}

		public Task<List<DeviceError>> GetList(DateTime start, DateTime end)
		{
			return ExecuteQuery<DeviceError>($@"
				SELECT
					 [Source]
					,[Created]
					,[Message]
					,[StatusSent]
				FROM [DeviceErrors]
				WHERE 
					[Created] >= '{start:yyyy-MM-dd}'
				AND [Created] < '{(end != DateTime.MaxValue ? end.AddDays(1) : end):yyyy-MM-dd}'
				ORDER BY [Id]");
		}
	}
}