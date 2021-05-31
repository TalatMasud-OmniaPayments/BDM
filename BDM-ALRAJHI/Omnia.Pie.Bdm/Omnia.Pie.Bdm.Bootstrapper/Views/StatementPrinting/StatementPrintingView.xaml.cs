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

namespace Omnia.Pie.Bdm.Bootstrapper.Views.StatementPrinting
{
	/// <summary>
	/// Interaction logic for StatementPrintingView.xaml
	/// </summary>
	public partial class StatementPrintingView : UserControl
	{
		public StatementPrintingView()
		{
			InitializeComponent();
		}
        private void DatePickerStartTextBox_OnMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            dpStartDate.IsDropDownOpen = true;
        }
        private void DatePickerEndTextBox_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dpEndDate.IsDropDownOpen = true;
        }
        private void dpStartDate_Click(object sender, RoutedEventArgs e)
        {

            dpStartDate.IsDropDownOpen = true;
        }
        private void dpEndDate_Click(object sender, RoutedEventArgs e)
        {
            dpEndDate.IsDropDownOpen = true;
        }
        private void DatePicker_SelectedDateChanged(object sender,
            SelectionChangedEventArgs e)
        {
            // ... Get DatePicker reference.
            var picker = sender as DatePicker;

            // ... Get nullable DateTime from SelectedDate.
            DateTime date = picker.SelectedDate ?? DateTime.Today;
            if (date == null)
            {
                // ... A null object.
                //this.Title = "No date";
            }
            else
            {
                // ... No need to display the time.
                //this.Title = date.Value.ToShortDateString();

                //StartDate = date;
            }
        }
    }
}
