using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for ChequeDepositAccountPrintReceipt.xaml
    /// </summary>
    public partial class ChequeDepositAccountPrintReceipt : Page
    {
        public ChequeDepositAccountPrintReceipt()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
