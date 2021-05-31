namespace Omnia.Pie.Vtm.Bootstrapper.Views
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using System.Windows;

	public partial class TopVideoWindow : Window
	{
		public TopVideoWindow(ITopVideoObserver ViewModel)
		{
			InitializeComponent();
			DataContext = ViewModel;
			ViewModel.StartVideos();
		}
	}
}