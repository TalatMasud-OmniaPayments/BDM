using System.Windows;
using System.Windows.Controls;

namespace Omnia.Pie.Supervisor.Shell.Views.Pages
{
	/// <summary>
	/// Interaction logic for Animation.xaml
	/// </summary>
	public partial class Animation : UserControl
	{
		public Animation()
		{
			InitializeComponent();
		}

		public double Size
		{
			get { return (double)GetValue(SizeProperty); }
			set { SetValue(SizeProperty, value); }
		}

		public static readonly DependencyProperty SizeProperty =
			DependencyProperty.Register("Size", typeof(double), typeof(Animation), new PropertyMetadata(15.0));
	}
}
