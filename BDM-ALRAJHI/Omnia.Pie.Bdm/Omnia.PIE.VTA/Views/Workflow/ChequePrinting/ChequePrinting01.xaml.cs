using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Omnia.PIE.VTA.Views.Workflow
{
	/// <summary>
	/// Interaction logic for ChequePrinting01.xaml
	/// </summary>
	public partial class ChequePrinting01 : Page
	{
		public ChequePrinting01()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			this.DataContext = MainWindow.WorkFlowViewModel;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start(ConfigurationManager.AppSettings["CentralBankUrl"].ToString());
				e.Handled = true;
			}
			catch (Exception ex)
			{
				Logger.Writer.Exception(ex);
			}
		}
	}
}