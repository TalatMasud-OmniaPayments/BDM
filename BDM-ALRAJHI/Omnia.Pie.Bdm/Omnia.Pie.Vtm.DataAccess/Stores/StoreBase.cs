namespace Omnia.Pie.Vtm.DataAccess.Stores
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Omnia.Pie.Vtm.DataAccess.DataAccess;
	using Omnia.Pie.Vtm.Framework.Interface;

	internal abstract class StoreBase
	{
		private IResolver _container { get; }
		private ILogger _logger { get; }

		protected StoreBase(IResolver container)
		{
			_container = container ?? throw new ArgumentNullException(nameof(container));
			_logger = _container.Resolve<ILogger>();
		}

		protected Task ExecuteCommand(string commandText, object param = null)
		{
			try
			{
				return Database.Instance.ExecuteCommand(commandText, param);
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				throw;
			}
		}

		protected async Task<List<T>> ExecuteQuery<T>(string commandText, object param = null)
		{
			try
			{
				var result = await Database.Instance.ExecuteQuery<T>(commandText, param);
				return result.ToList();
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				throw;
			}
		}

		protected async Task<T> QueryFirstOrDefault<T>(string commandText, object param = null)
		{
			try
			{
				return await Database.Instance.QueryFirstOrDefault<T>(commandText, param);
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				throw;
			}
		}
	}
}