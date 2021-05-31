namespace Omnia.Pie.Vtm.Bootstrapper.Configurations
{
	using Omnia.Pie.Vtm.Bootstrapper.Views;
	using Omnia.Pie.Vtm.Framework.Interface.Module;
	using System;

	internal class Module : IModule
	{
		private MainWindow MainView { get; }
		private TopVideoWindow TopVideoWindowView { get; }

		public Module(MainWindow mainView, TopVideoWindow topVideoWindowView)
		{
			MainView = mainView ?? throw new ArgumentNullException(nameof(mainView));
			TopVideoWindowView = topVideoWindowView ?? throw new ArgumentNullException(nameof(topVideoWindowView));
			SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
		}

		public void Initialize()
		{
			MainView.Show();
			TopVideoWindowView.Show();
		}
	}
}