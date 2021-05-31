using System.Windows;

namespace ESpaceCommunication.Test.Views
{
	/// <summary>
	/// Interaction logic for Video.xaml
	/// </summary>
	public partial class Video : Window
	{
		public Video()
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