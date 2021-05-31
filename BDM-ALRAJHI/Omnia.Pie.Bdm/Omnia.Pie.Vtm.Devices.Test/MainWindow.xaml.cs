using System;
using System.Windows;
using Microsoft.Practices.Unity;
using Omnia.Pie.Vtm.Devices.Test.ViewModels;

namespace Omnia.Pie.Vtm.Devices.Test
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
			MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			var win = new MainViewModel();
			UnityContainer.Container.RegisterInstance(win);
			DataContext = win;
		}
	}
}