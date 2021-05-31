namespace Omnia.Pie.Vtm.DataAccess.DataAccess
{
	using Dapper;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Data.SQLite;
	using System.IO;
	using System.Threading.Tasks;

	internal class Database
	{
		private static Database _database;
		public static Database Instance => _database ?? (_database = new Database());
		private readonly SQLiteConnection _connection;
		private readonly object _lockObj = new object();

		private Database()
		{
			var databaseFile = GetDatabasePath();
			if (!File.Exists(databaseFile))
			{
				SQLiteConnection.CreateFile(databaseFile);
			}
			_connection = new SQLiteConnection($"Data Source={databaseFile};Version=3;");
		}

		private static string GetDatabasePath()
		{
			var dbSection = (DatabaseSection) ConfigurationManager.GetSection(DatabaseSection.Name);
			var dbPath = dbSection?.Path;
			return dbPath;
		}

		public Task ExecuteCommand(string commandText, object param = null)
		{
			lock (_lockObj)
			{
				if (_connection.State != ConnectionState.Open)
					_connection.Open();
				try
				{
					return _connection.ExecuteAsync(commandText, param);
				}
				finally
				{
					_connection.Close();
				}
			}
		}

		public Task<IEnumerable<T>> ExecuteQuery<T>(string commandText, object param = null)
		{
			lock (_lockObj)
			{
				if (_connection.State != ConnectionState.Open)
					_connection.Open();
				try
				{
					return _connection.QueryAsync<T>(commandText, param);
				}
				finally
				{
					_connection.Close();
				}
			}
		}

		public Task<T> QueryFirstOrDefault<T>(string commandText, object param = null)
		{
			lock (_lockObj)
			{
				if (_connection.State != ConnectionState.Open)
					_connection.Open();
				try
				{
					return _connection.QueryFirstOrDefaultAsync<T>(commandText, param);
				}
				finally
				{
					_connection.Close();
				}
			}
		}
	}
}