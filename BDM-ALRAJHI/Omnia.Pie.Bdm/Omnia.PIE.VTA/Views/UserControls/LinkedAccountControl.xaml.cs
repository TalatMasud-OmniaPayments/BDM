using Omnia.PIE.VTA.Core.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
	/// <summary>
	/// Interaction logic for LinkedAccountControl.xaml
	/// </summary>
	public partial class LinkedAccountControl : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedAccountControl"/> class.
		/// </summary>
		public LinkedAccountControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Handles the Loaded event of the UserControl control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			lstLinkedAccounts.ItemsSource = MainWindow.WorkFlowViewModel.LinkedAccounts;
		}

		public void ResetItemSource()
		{
			lstLinkedAccounts.ItemsSource = null;
		}
	}
}