using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for AdditionalAccountOpeningSelectedAccount.xaml
    /// </summary>
    public partial class AdditionalAccountOpeningSelectedAccount : Page
    {
        public AdditionalAccountOpeningSelectedAccount()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
