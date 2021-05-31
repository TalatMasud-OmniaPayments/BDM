using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
	/// <summary>
	/// Interaction logic for UpdateCustomerDetailsWithOTP.xaml
	/// </summary>
	public partial class UpdateCustomerDetailsWithOTP : Page
    {
        public UpdateCustomerDetailsWithOTP()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
