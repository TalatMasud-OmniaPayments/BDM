namespace Omnia.Pie.Vtm.Bootstrapper
{
	using Microsoft.Practices.Unity;
	using Omnia.Pie.Vtm.Bootstrapper.ViewModels;
	using System.Windows;

	public partial class MainWindow : Window
	{
		[Dependency]
		public MainWindowViewModel ViewModel
		{
			get { return (MainWindowViewModel)DataContext; }
			set { DataContext = value; }
		}

		public MainWindow()
		{
			InitializeComponent();
		}
	}
}
