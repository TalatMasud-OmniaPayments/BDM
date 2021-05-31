using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for AdditionalAccountOpeningCaptureSignature.xaml
    /// </summary>
    public partial class AdditionalAccountOpeningCaptureSignature : Page
    {
        public AdditionalAccountOpeningCaptureSignature()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
