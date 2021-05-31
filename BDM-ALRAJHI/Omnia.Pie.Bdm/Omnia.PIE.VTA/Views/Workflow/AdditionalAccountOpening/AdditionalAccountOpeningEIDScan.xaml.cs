using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for AdditionalAccountOpeningEIDScan.xaml
    /// </summary>
    public partial class AdditionalAccountOpeningEIDScan : Page
    {
        public AdditionalAccountOpeningEIDScan()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
