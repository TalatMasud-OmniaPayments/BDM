﻿using System;
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

namespace Omnia.PIE.VTA.Views
{
    /// <summary>
    /// Interaction logic for AccountsAndDeposits.xaml
    /// </summary>
    public partial class AccountsAndDeposits : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsAndDeposits"/> class.
        /// </summary>
        public AccountsAndDeposits()
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
            lstBoxAccountsAndDeposits.ItemsSource = MainWindow.Instance.AccountsAndDepositsViewModel;
        }
    }
}
