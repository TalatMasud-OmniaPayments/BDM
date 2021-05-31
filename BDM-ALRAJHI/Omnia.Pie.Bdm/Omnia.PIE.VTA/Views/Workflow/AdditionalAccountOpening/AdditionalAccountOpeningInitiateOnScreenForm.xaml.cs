using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
    /// <summary>
    /// Interaction logic for AdditionalAccountOpeningInitiateOnScreenForm.xaml
    /// </summary>
    public partial class AdditionalAccountOpeningInitiateOnScreenForm : Page
    {
        public AdditionalAccountOpeningInitiateOnScreenForm()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainWindow.WorkFlowViewModel;
        }
    }
}
