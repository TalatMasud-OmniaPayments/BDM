using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for CreditCardPaymentWithCashSelectedCard.xaml
    /// </summary>
    public partial class CreditCardPaymentWithCashSelectedCard : Page
    {
        public CreditCardPaymentWithCashSelectedCard()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
