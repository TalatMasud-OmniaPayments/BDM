using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for ChequeDepositCreditCardPrintReceipt.xaml
    /// </summary>
    public partial class ChequeDepositCreditCardPrintReceipt : Page
    {
        public ChequeDepositCreditCardPrintReceipt()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
