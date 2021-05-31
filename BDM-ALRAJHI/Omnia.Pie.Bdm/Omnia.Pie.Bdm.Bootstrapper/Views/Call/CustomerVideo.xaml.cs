namespace Omnia.Pie.Bdm.Bootstrapper.Views.Call
{
	using System.Windows;

	public partial class CustomerVideo : Window
	{
		public CustomerVideo()
		{
			InitializeComponent();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}
	}
}