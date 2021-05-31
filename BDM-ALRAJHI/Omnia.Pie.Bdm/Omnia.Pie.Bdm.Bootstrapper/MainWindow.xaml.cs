namespace Omnia.Pie.Bdm.Bootstrapper
{
	using Microsoft.Practices.Unity;
	using Omnia.Pie.Bdm.Bootstrapper.ViewModels;
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
