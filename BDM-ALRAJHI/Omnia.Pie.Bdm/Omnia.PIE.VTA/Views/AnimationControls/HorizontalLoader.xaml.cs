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

namespace Omnia.PIE.VTA.Views.AnimationControls
{
    /// <summary>
    /// Interaction logic for HorizontalLoader.xaml
    /// </summary>
    public partial class HorizontalLoader : UserControl
    {
        private double _DefaultSize = 10;
        public double DefaultSize
        {
            get
            {
                return _DefaultSize;
            }
            set
            {
                _DefaultSize = value;
            }
        }

        public HorizontalLoader()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ellipse.Width = DefaultSize;
            ellipse.Height = DefaultSize;

            ellipse1.Width = DefaultSize;
            ellipse1.Height = DefaultSize;

            ellipse2.Width = DefaultSize;
            ellipse2.Height = DefaultSize;

            ellipse3.Width = DefaultSize;
            ellipse3.Height = DefaultSize;

            ellipse4.Width = DefaultSize;
            ellipse4.Height = DefaultSize;

            ellipse5.Width = DefaultSize;
            ellipse5.Height = DefaultSize;

            ellipse6.Width = DefaultSize;
            ellipse6.Height = DefaultSize;

            ellipse7.Width = DefaultSize;
            ellipse7.Height = DefaultSize;


            ellipse8.Width = DefaultSize;
            ellipse8.Height = DefaultSize;

            ellipse9.Width = DefaultSize;
            ellipse9.Height = DefaultSize;

        }
    }
}
