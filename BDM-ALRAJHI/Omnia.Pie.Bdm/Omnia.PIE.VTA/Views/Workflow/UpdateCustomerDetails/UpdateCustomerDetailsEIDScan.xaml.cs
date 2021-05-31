using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for UpdateCustomerDetailsEIDScan.xaml
    /// </summary>
    public partial class UpdateCustomerDetailsEIDScan : Page
    {
        public UpdateCustomerDetailsEIDScan()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
