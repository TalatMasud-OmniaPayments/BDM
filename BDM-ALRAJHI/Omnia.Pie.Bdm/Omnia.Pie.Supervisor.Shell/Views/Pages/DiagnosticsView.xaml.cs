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

namespace Omnia.Pie.Supervisor.Shell.Views.Pages {
	/// <summary>
	/// Interaction logic for DiagnosticsView.xaml
	/// </summary>
	public partial class DiagnosticsView : UserControl {
		public DiagnosticsView() {
			InitializeComponent();
		}
        
        private void dpStartDate_Click(object sender, RoutedEventArgs e)
        {
          
            dpStartDate.IsDropDownOpen = true; 
        }
        private void dpEndDate_Click(object sender, RoutedEventArgs e)
        {

            dpEndDate.IsDropDownOpen = true; 
        }
    }
}
