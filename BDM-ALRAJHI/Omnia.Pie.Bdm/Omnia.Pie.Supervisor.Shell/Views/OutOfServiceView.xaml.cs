using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Omnia.Pie.Supervisor.Shell.Utilities;

namespace Omnia.Pie.Supervisor.Shell.Views
{
	/// <summary>
	/// Interaction logic for InService.xaml
	/// </summary>
	public partial class OutOfServiceView : Window
	{
		public OutOfServiceView()
		{
			InitializeComponent();

			Closing += OnClosing;
		}

		private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
		{
			Hide();
			cancelEventArgs.Cancel = true;
		}
	}
}