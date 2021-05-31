using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for ChequeDepositAccountConfirmDeposit.xaml
    /// </summary>
    public partial class ChequeDepositAccountConfirmDeposit : Page
    {
        public ChequeDepositAccountConfirmDeposit()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
