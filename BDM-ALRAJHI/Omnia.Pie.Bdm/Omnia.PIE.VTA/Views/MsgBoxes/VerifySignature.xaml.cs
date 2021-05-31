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
using System.Windows.Shapes;

namespace Omnia.PIE.VTA.Views.MsgBoxes
{
	/// <summary>
	/// Interaction logic for VerifySignature.xaml
	/// </summary>
	public partial class VerifySignature : Window
	{
		public VerifySignature()
		{
			InitializeComponent();
		}

		private void BtnClose_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void MainHead_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void btnMaximize_Click(object sender, RoutedEventArgs e)
		{
			if (this.WindowState == WindowState.Maximized)
				this.WindowState = WindowState.Normal;
			else
				this.WindowState = WindowState.Maximized;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Maximized;
		}
	}
}
