using Omnia.PIE.VTA.Common;
using System;
using System.Windows;
using System.Windows.Input;


namespace Omnia.PIE.VTA.Views.MsgBoxes
{
    public enum WpfMessageBoxButton
    {
        OK = 0,
        OKCancel = 1,
        YesNo = 2,
        YesNoCancel = 3
    }

    public enum WpfMessageBoxResult
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Yes = 3,
        No = 4
    }

    public enum WpfMessageBoxImage
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Information = 3,
        Question = 4
    }

    public partial class WpfMessageBox : Window
    {
        static WpfMessageBoxResult mWpfMessageBoxResult;
        private WpfMessageBoxButton mWpfMessageBoxButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfMessageBox"/> class.
        /// </summary>
        public WpfMessageBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the MainHead control. Drag the window
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void MainHead_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static WpfMessageBoxResult Show(string text)
        {
            WpfMessageBox msg = new WpfMessageBox();

            msg.MessageTitle.Text = string.Empty;
            msg.MessageBlock.Text = text;
            msg.SetVisibility(WpfMessageBoxButton.OK);
            msg.SetImage(WpfMessageBoxImage.None);
            msg.ShowDialog();

            return mWpfMessageBoxResult;
        }

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <returns></returns>
        public static WpfMessageBoxResult Show(string text, string caption)
        {
            WpfMessageBox msg = new WpfMessageBox();

            msg.MessageTitle.Text = caption;
            msg.MessageBlock.Text = text;
            msg.SetVisibility(WpfMessageBoxButton.OK);
            msg.SetImage(WpfMessageBoxImage.None);
            msg.ShowDialog();

            return mWpfMessageBoxResult;
        }

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="button">The button.</param>
        /// <returns></returns>
        public static WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button)
        {
            WpfMessageBox msg = new WpfMessageBox();

            msg.MessageTitle.Text = caption;
            msg.MessageBlock.Text = text;
            msg.SetVisibility(button);
            msg.SetImage(WpfMessageBoxImage.None);
            msg.ShowDialog();

            return mWpfMessageBoxResult;
        }

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="button">The button.</param>
        /// <param name="icon">The icon.</param>
        /// <returns></returns>
        public static WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage icon)
        {
            WpfMessageBox msg = new WpfMessageBox();

            msg.MessageTitle.Text = caption;
            msg.MessageBlock.Text = text;
            msg.SetVisibility(button);
            msg.SetImage(icon);
            msg.ShowDialog();

            return mWpfMessageBoxResult;
        }

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="button">The button.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="defaultResult">The default result.</param>
        /// <returns></returns>
        public static WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage icon, WpfMessageBoxResult defaultResult)
        {
            WpfMessageBox msg = new WpfMessageBox();

            msg.MessageTitle.Text = caption;
            msg.MessageBlock.Text = text;
            msg.SetVisibility(button);
            msg.SetImage(icon);
            msg.SetDefaultResult(defaultResult);
            msg.ShowDialog();

            return mWpfMessageBoxResult;
        }

        /// <summary>
        /// Sets the visibility.
        /// </summary>
        /// <param name="button">The button.</param>
        private void SetVisibility(WpfMessageBoxButton button)
        {
            mWpfMessageBoxButton = button;

            if (button == WpfMessageBoxButton.OK)
            {
                btnOk.IsDefault = true;
                btnCancel.Visibility = System.Windows.Visibility.Collapsed;
                btnYes.Visibility = System.Windows.Visibility.Collapsed;
                btnNo.Visibility = System.Windows.Visibility.Collapsed;
                btnOk.Visibility = System.Windows.Visibility.Visible;
            }
            else if (button == WpfMessageBoxButton.OKCancel)
            {
                btnOk.IsDefault = true;
                btnCancel.Visibility = System.Windows.Visibility.Visible;
                btnYes.Visibility = System.Windows.Visibility.Collapsed;
                btnNo.Visibility = System.Windows.Visibility.Collapsed;
                btnOk.Visibility = System.Windows.Visibility.Visible;
            }
            else if (button == WpfMessageBoxButton.YesNo)
            {
                btnYes.IsDefault = true;
                btnCancel.Visibility = System.Windows.Visibility.Collapsed;
                btnYes.Visibility = System.Windows.Visibility.Visible;
                btnNo.Visibility = System.Windows.Visibility.Visible;
                btnOk.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (button == WpfMessageBoxButton.YesNoCancel)
            {
                btnYes.IsDefault = true;
                btnCancel.Visibility = System.Windows.Visibility.Visible;
                btnYes.Visibility = System.Windows.Visibility.Visible;
                btnNo.Visibility = System.Windows.Visibility.Visible;
                btnOk.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Sets the default result.
        /// </summary>
        /// <param name="defaultResult">The default result.</param>
        private void SetDefaultResult(WpfMessageBoxResult defaultResult)
        {
            if (defaultResult == WpfMessageBoxResult.Yes)
            {
                btnYes.IsDefault = true;
                btnYes.Focus();
                btnNo.IsDefault = false;
                btnCancel.IsDefault = false;
                btnOk.IsDefault = false;
            }
            else if (defaultResult == WpfMessageBoxResult.No)
            {
                btnNo.IsDefault = true;
                btnNo.Focus();
                btnYes.IsDefault = false;
                btnCancel.IsDefault = false;
                btnOk.IsDefault = false;
            }
            else if (defaultResult == WpfMessageBoxResult.Cancel)
            {
                btnCancel.IsDefault = true;
                btnCancel.Focus();
                btnYes.IsDefault = false;
                btnNo.IsDefault = false;
                btnOk.IsDefault = false;
            }
            else if (defaultResult == WpfMessageBoxResult.OK)
            {
                btnOk.IsDefault = true;
                btnOk.Focus();
                btnYes.IsDefault = false;
                btnNo.IsDefault = false;
                btnCancel.IsDefault = false;
            }
        }

        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="image">The image.</param>
        private void SetImage(WpfMessageBoxImage image)
        {
            //if (image == WpfMessageBoxImage.Error)
            //    MessageImage.Source = new BitmapImage(new Uri("Images\\Error.png", UriKind.Relative));
            //else if (image == WpfMessageBoxImage.Warning)
            //    MessageImage.Source = new BitmapImage(new Uri("Images\\Warning.png", UriKind.Relative));
            //else if (image == WpfMessageBoxImage.Question)
            //    MessageImage.Source = new BitmapImage(new Uri("Images\\Question.png", UriKind.Relative));
            //else if (image == WpfMessageBoxImage.Information)
            //    MessageImage.Source = new BitmapImage(new Uri("Images\\Information.png", UriKind.Relative));
        }

        /// <summary>
        /// Handles the Click event of the btnOk control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //mWpfMessageBoxResult = WpfMessageBoxResult.OK;
            this.DialogResult = true;
        }

        /// <summary>
        /// Handles the Click event of the btnYes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnYes_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mWpfMessageBoxResult = WpfMessageBoxResult.Yes;
            DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the btnNo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnNo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mWpfMessageBoxResult = WpfMessageBoxResult.No;
            DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mWpfMessageBoxResult = WpfMessageBoxResult.Cancel;
            DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Handles the Closing event of the WpfMessageBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void WpfMessageBox_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //If mWpfMessageBoxButton = WpfMessageBoxButton.YesNo Then
            //    If mWpfMessageBoxResult <> WpfMessageBoxResult.Yes And mWpfMessageBoxResult <> WpfMessageBoxResult.No Then
            //        e.Cancel = True
            //    End If
            //End If
        }

        /// <summary>
        /// Handles the Loaded event of the WpfMessageBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void WpfMessageBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            mWpfMessageBoxResult = WpfMessageBoxResult.None;

            //Dim res new ResourceDictionary()
            //If mSkin = DisplaySkin.Black Then
            //    res.Source = new Uri("BlackSkin.xaml", UriKind.Relative)
            //ElseIf mSkin = DisplaySkin.Blue Then
            //    res.Source = new Uri("BlueSkin.xaml", UriKind.Relative)
            //End If

            //Me.Resources.Clear()
            //Me.Resources = res
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
