using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Omnia.Pie.Vtm.Devices.Console {
	public class Command : ICommand {
		public Command(Action execute, Func<bool> canExecute = null) {
			commands.Add(this);
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged;
		readonly Func<bool> canExecute;
		public bool CanExecute(object parameter) => canExecute?.Invoke() ?? true;

		readonly Action execute;
		public void Execute(object parameter) {
			execute?.Invoke();
			foreach(var i in commands)
				i.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

		static readonly List<Command> commands = new List<Command>();
	}
}
