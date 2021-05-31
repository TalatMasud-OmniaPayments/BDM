using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Windows;
using System.Windows.Input;

namespace Omnia.PIE.VTA.Views.MsgBoxes
{
    /// <summary>
    /// Interaction logic for AnswerIncomingCall.xaml
    /// </summary>
    public partial class AnswerIncomingCall : Window
    {
        /// <summary>
        /// Gets or sets the terminal.
        /// </summary>
        /// <value>
        /// The terminal.
        /// </value>
        public string Terminal { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnswerIncomingCall"/> class.
        /// </summary>
        /// <param name="terminal">The terminal.</param>
        public AnswerIncomingCall()
        {
            InitializeComponent();
            this.Owner = MainWindow.Instance;
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            popInfoAll.Content = "Terminal : " + Terminal + " " + popInfo.Content;
	        MainWindow.Instance.ShowMissedCallAlert = true;
			PlaySound();
        }

        /// <summary>
        /// Handles the MediaEnded event of the SoundRingIn control. Ringing sound
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SoundRingIn_MediaEnded(object sender, RoutedEventArgs e)
        {
            soundRingIn.Position = TimeSpan.Zero;
            PlaySound();
        }

        /// <summary>
        /// Plays the sound.
        /// </summary>
        private void PlaySound()
        {
            soundRingIn.Play();
        }

        /// <summary>
        /// Handles the Click event of the AnswerButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
			Logger.Writer.Info(GetType().ToString() + " AnswerButton_Click.");

            MainWindow.Instance.AnswerIncomingCall();
            this.soundRingIn.Stop();
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the btnClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
			Logger.Writer.Info(GetType().ToString() + " AnswerWindow Close.");
			MainWindow.Instance.ShowMissedCallAlert = false;
			MainWindow.Instance.ReleaseCall();
            this.Close();
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
        /// Handles the MediaOpened event of the soundRingIn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void soundRingIn_MediaOpened(object sender, RoutedEventArgs e)
        {
            PlaySound();
        }
    }
}