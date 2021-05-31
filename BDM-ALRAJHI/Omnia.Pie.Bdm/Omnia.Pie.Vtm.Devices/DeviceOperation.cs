namespace Omnia.Pie.Vtm.Devices
{
	using System;
	using System.Threading.Tasks;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Extensions;

	internal class DeviceOperation
	{
		protected DeviceOperation() { }
		public virtual void Stop(Exception ex) { }
		public bool IsRunning { get; protected set; }
	}

	internal class DeviceOperation<TResult> : DeviceOperation
	{
		public DeviceOperation(string id, ILogger logger, IJournal journal, bool throwExceptions = true)
		{
			Logger = logger;
			Journal = journal;
			this.throwExceptions = throwExceptions;
			this.id = id;
		}

		TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
		readonly bool throwExceptions;
		protected readonly IJournal Journal;
		protected readonly ILogger Logger;
		readonly string id;

		public override string ToString() => $"{nameof(Device)} {id}";

		public async Task<TResult> StartAsync(Func<int> f = null)
		{
			if (!IsRunning)
				try
				{
					//Logger.Info($"{this} started");
					IsRunning = true;
					taskCompletionSource = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
					if (f != null)
					{
						var result = f();
						if (result != DeviceResult.Ok)
							throw new DeviceMalfunctionException(ToString(), result);
					}
					else
						return await OnInvoke();
					return await taskCompletionSource.Task;
				}
				catch (Exception ex)
				{
					ex = ex.ToDeviceException();
					Logger.Exception(ex);
					if (throwExceptions)
						throw;
					else
						return default(TResult);
				}
				finally
				{
					taskCompletionSource = null;
					IsRunning = false;
					Logger.Info($"{this} stopped");
				}
			else
				throw new DeviceMalfunctionException($"{this} is running");
		}

		public TResult Start(Func<TResult> f) => start(f, null);

		public void Start(Func<int> f) => start<int>(f, result =>
		{
			if (result != DeviceResult.Ok)
				throw new DeviceMalfunctionException(ToString(), result);
		});

		TResult2 start<TResult2>(Func<TResult2> f, Action<TResult2> checkResult)
		{
			if (!IsRunning)
				try
				{
					Logger.Info($"{this} started");
					IsRunning = true;
					var result = f();
					checkResult?.Invoke(result);
					return result;
				}
				catch (Exception ex)
				{
					ex = ex.ToDeviceException();
					Logger.Exception(ex);
					if (throwExceptions)
						throw;
					else
						return default(TResult2);
				}
				finally
				{
					IsRunning = false;
					Logger.Info($"{this} stopped");
				}
			else
				throw new DeviceMalfunctionException($"{this} is runing");
		}

		void stop(Action f)
		{
			if (IsRunning)
				try
				{
					f();
				}
				catch (Exception ex)
				{
					ex = ex.ToDeviceException();
					Logger.Exception(ex);
					if (throwExceptions)
						throw;
				}
				finally
				{
					IsRunning = false;
				}
		}

		public void Stop(TResult result) => stop(() => taskCompletionSource?.TrySetResult(result));

		public override void Stop(Exception ex) => stop(() =>
		{
			ex = ex.ToDeviceException();
			if (taskCompletionSource != null)
				try
				{
					taskCompletionSource.TrySetException(ex);
				}
				catch
				{
					throw ex;
				}
			else
				throw ex;
		});

		protected virtual Task<TResult> OnInvoke()
		{
			return Task.FromResult(default(TResult));
		}

		protected virtual int Invoke()
		{
			return DeviceResult.Ok;
		}

		protected virtual void LogSuccess()
		{

		}

		protected virtual void LogTimeout()
		{

		}
	}
}