using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for ChequeDepositCreditCardSelectedCard.xaml
    /// </summary>
    public partial class ChequeDepositCreditCardSelectedCard : Page
    {
        public ChequeDepositCreditCardSelectedCard()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
