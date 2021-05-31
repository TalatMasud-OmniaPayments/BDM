using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for ChequeDepositCreditCardActivateChequeDeposit.xaml
    /// </summary>
    public partial class ChequeDepositCreditCardActivateChequeDeposit : Page
    {
        public ChequeDepositCreditCardActivateChequeDeposit()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
