using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for AdditionalAccountOpeningEndSession.xaml
    /// </summary>
    public partial class AdditionalAccountOpeningEndSession : Page
    {
        public AdditionalAccountOpeningEndSession()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
