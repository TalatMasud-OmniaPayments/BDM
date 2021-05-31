using Omnia.PIE.VTA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for CashDepositCreditCard02.xaml
    /// </summary>
    public partial class CashDepositCreditCard02 : Page
    {
        public WorkFlowViewModel ViewModel = new WorkFlowViewModel();

        public CashDepositCreditCard02()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void SetInitializeStage()
        {
            ViewModel = new WorkFlowViewModel();
        }
    }
}
