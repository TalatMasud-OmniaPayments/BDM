using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
	/// <summary>
	/// Interaction logic for ChequePrinting02.xaml
	/// </summary>
	public partial class ChequePrinting02 : Page
	{
		public ChequePrinting02()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			this.DataContext = MainWindow.WorkFlowViewModel;
		}
	}
}