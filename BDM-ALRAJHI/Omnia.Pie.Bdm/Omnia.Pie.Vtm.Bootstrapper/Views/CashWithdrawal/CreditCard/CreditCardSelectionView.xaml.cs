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

namespace Omnia.Pie.Vtm.Bootstrapper.Views.CashWithdrawal.CreditCard
{
	/// <summary>
	/// Interaction logic for CreditCardSelectionView.xaml
	/// </summary>
	public partial class CreditCardSelectionView : UserControl
	{
		public CreditCardSelectionView()
		{
			InitializeComponent();
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void lstAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//togglePopup.IsChecked = false;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			//togglePopup.IsChecked = false;
		}
	}
}
