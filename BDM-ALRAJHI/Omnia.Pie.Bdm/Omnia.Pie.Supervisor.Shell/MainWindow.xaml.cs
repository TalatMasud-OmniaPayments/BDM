namespace Omnia.Pie.Supervisor.Shell
{
	using System.Windows;
	using Omnia.Pie.Supervisor.Shell.Utilities;
	using System;
	using System.Windows.Forms.Integration;

	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			ElementHost.EnableModelessKeyboardInterop(this);
			this.SetMainScreen();
			Activate();
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			Width = 1024;
			Height = 768;
		}
	}
}