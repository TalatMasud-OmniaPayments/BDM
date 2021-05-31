using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Omnia.Pie.Vtm.Framework.DelegateCommand;

namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	public class OperationViewModel : ViewModel
	{
		protected OperationViewModel(Func<bool> canExecute)
		{
			this.canExecute = canExecute ?? (() => true);
		}

		public OperationViewModel(Action onExecute, Func<bool> canExecute = null) :
			this(canExecute)
		{
			this.onExecute = onExecute;
			Execute = new DelegateCommand(
				() => execute(onExecute),
				canExecute);
		}

		public OperationViewModel(Func<Task> onExecute, Func<bool> canExecute = null) :
			this(canExecute)
		{
			this.onExecuteAsync = onExecute;
			Execute = new DelegateCommand(
				async () => await execute(onExecuteAsync),
				canExecute);
		}

		public ICommand Execute { get; protected set; }

		readonly Action onExecute;
		readonly Func<Task> onExecuteAsync;
		readonly Func<bool> canExecute;

		protected void AddResult(object result) => UnityContainer.Container.Resolve<MainViewModel>().Results.Insert(0, result ?? "null");

		public override void Load()
		{
			base.Load();
			UnityContainer.Container.Resolve<MainViewModel>().Load();
		}

		protected void execute(Action f)
		{
			try
			{
				AddResult($"{Id} started");
				f();
				AddResult($"{Id} ok");
			}
			catch(Exception ex)
			{
				AddResult(ex);
			}
			finally
			{
				Load();
			}
		}

		protected async Task execute(Func<Task> f)
		{
			try
			{
				AddResult($"{Id} started");
				await f();
				AddResult($"{Id} ok");
			}
			catch(Exception ex)
			{
				AddResult(ex);
			}
			finally
			{
				Load();
			}
		}
	}

	public class OperationViewModel<TResult> : OperationViewModel
	{
		public OperationViewModel(Func<Task<TResult>> onExecute, Func<bool> canExecute = null) :
			base(canExecute)
		{
			this.onExecuteAsync = onExecute;
			Execute = new DelegateCommand(
				async () => await execute(onExecuteAsync()),
				canExecute);
		}

		public OperationViewModel(Func<TResult> onExecute, Func<bool> canExecute = null) :
			base(canExecute)
		{
			this.onExecute = onExecute;
			Execute = new DelegateCommand(
				() => execute(onExecute()),
				canExecute);
		}

		readonly Func<Task<TResult>> onExecuteAsync;
		readonly Func<TResult> onExecute;

		async Task execute(Task<TResult> f)
		{
			try
			{
				AddResult($"{Id} started");
				Result = await f;
				AddResult($"{Id} ok");
				AddResult(Result);
			}
			catch(Exception ex)
			{
				AddResult(ex);
			}
			finally
			{
				Load();
			}
		}

		void execute(TResult f)
		{
			try
			{
				AddResult($"{Id} started");
				Result = f;
				AddResult($"{Id} ok");
				AddResult(Result);
			}
			catch(Exception ex)
			{
				AddResult(ex);
			}
			finally
			{
				Load();
			}
		}

		public TResult Result { get; set; }
	}
}
