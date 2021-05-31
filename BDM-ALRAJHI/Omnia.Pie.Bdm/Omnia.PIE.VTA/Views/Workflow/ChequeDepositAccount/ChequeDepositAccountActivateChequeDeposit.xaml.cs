using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for ChequeDepositAccountActivateChequeDeposit.xaml
    /// </summary>
    public partial class ChequeDepositAccountActivateChequeDeposit : Page
    {
        public ChequeDepositAccountActivateChequeDeposit()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
