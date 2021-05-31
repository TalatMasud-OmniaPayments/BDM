namespace Omnia.Pie.Vtm.Bootstrapper.Views.Call
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using System.Windows;

	public partial class TellerVideo : Window
	{
		public TellerVideo(ITopVideoObserver ViewModel)
		{
			InitializeComponent();
			DataContext = ViewModel;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}
	}
}