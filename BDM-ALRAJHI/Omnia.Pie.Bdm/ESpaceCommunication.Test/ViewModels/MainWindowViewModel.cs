namespace ESpaceCommunication.Test.ViewModels
{
	using ESpaceCommunication.Test.Views;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using System;
	using System.Threading;
	using System.Windows;
	using System.Windows.Input;

	public class MainWindowViewModel : BaseViewModel
	{
		private CancellationTokenSource _token;

		public ICommand CallTeller { get; }
		public ICommand CancelCall { get; }

		Video terminalVideo = new Video();
		Video tellerVideo = new Video();

		public MainWindowViewModel()
		{

			CancelCall = new DelegateCommand(
					() =>
					{
						if (_token != null)
						{
							_token.Cancel();
							_token.Dispose();
							_token = null;
						}
					});
			
		}

		
		public override void Dispose()
		{
		}
	}
}