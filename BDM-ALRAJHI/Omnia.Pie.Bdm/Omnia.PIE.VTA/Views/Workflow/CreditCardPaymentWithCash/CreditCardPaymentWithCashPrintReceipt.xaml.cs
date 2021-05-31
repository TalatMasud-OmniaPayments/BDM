using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for CreditCardPaymentWithCashPrintReceipt.xaml
    /// </summary>
    public partial class CreditCardPaymentWithCashPrintReceipt : Page
    {
        public CreditCardPaymentWithCashPrintReceipt()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
