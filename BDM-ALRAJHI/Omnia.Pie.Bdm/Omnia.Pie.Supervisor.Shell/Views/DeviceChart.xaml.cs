using Omnia.Pie.Supervisor.Shell.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Omnia.Pie.Supervisor.Shell.Views
{
    /// <summary>
    /// Interaction logic for DeviceChart.xaml
    /// </summary>
    public partial class DeviceChart : UserControl
    {
        public DashboardConfiguration DeviceData
        {
            get { return (DashboardConfiguration)GetValue(DeviceDataProperty); }
            set { SetValue(DeviceDataProperty, value); }
        }

        public static readonly DependencyProperty DeviceDataProperty =
            DependencyProperty.Register("DeviceData",
                typeof(DashboardConfiguration),
                typeof(DeviceChart),
                new PropertyMetadata(null));

        public DeviceChart()
        {
            InitializeComponent();
            this.Loaded += DeviceChart_Loaded;
        }

        private void DeviceChart_Loaded(object sender, RoutedEventArgs e)
        {
            int width = 90;
            StkCharts.Width = MainGrid.Width = MainBorder.Width = DeviceData.DeviceData.Length * width;
            DeviceTitle.Text = DeviceData.DeviceTitle;
            DeviceStatus.Text = DeviceData.Status;

            foreach (var item in DeviceData.DeviceData)
            {

                var title = item.Title ?? "-";
                var del = "CASSETTE";

                if (title.Contains(del))
                {
                    title = title.Remove(title.IndexOf(del), del.Length);
                }

                StackPanel stkDeviceData = new StackPanel()
                {
                    Width = width
                };

                // Textblock for Title
                TextBlock txblkTitle = new TextBlock()
                {
                    Style = FindResource("TextBlock.Chart.DeviceTitle") as Style,
                    Text = title
                };

                // Textblock for Max Count
                TextBlock txblkMaxCount = new TextBlock()
                {
                    Style = FindResource("TextBlock.Chart.MaxCount") as Style,
                    Text = item.MaxCount.ToString() ?? "-"
                };

                // Custom Progress Bar for Chart
                ProgressBar pbChart = new ProgressBar()
                {
                    Style = FindResource("DeviceStatusProgressBar") as Style,
                    Value = item.CurrentCount,
                    Maximum = item.MaxCount
                };

                TextBlock txblkStatus= new TextBlock()
                {
                    Style = FindResource("TextBlock.Chart.DeviceStatus") as Style,
                    Text = item.Status
                };

                stkDeviceData.Children.Add(txblkTitle);
                stkDeviceData.Children.Add(txblkStatus);
                stkDeviceData.Children.Add(txblkMaxCount);
                stkDeviceData.Children.Add(pbChart);
                

                StkCharts.Children.Add(stkDeviceData);
            }
        }
    }
}
