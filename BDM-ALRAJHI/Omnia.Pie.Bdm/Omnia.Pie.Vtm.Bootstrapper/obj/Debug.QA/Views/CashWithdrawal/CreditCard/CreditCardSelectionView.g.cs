﻿#pragma checksum "..\..\..\..\..\Views\CashWithdrawal\CreditCard\CreditCardSelectionView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6AD8D24DCAB0B4A2816019339622AA8A08A45ACB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Omnia.Pie.Vtm.Bootstrapper.Properties;
using Omnia.Pie.Vtm.Framework.ControlExtenders;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Omnia.Pie.Vtm.Bootstrapper.Views.CashWithdrawal.CreditCard {
    
    
    /// <summary>
    /// CreditCardSelectionView
    /// </summary>
    public partial class CreditCardSelectionView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 144 "..\..\..\..\..\Views\CashWithdrawal\CreditCard\CreditCardSelectionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lstAccounts;
        
        #line default
        #line hidden
        
        
        #line 205 "..\..\..\..\..\Views\CashWithdrawal\CreditCard\CreditCardSelectionView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtAccountNumber;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Omnia.Pie.Vtm.Bootstrapper;component/views/cashwithdrawal/creditcard/creditcards" +
                    "electionview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\CashWithdrawal\CreditCard\CreditCardSelectionView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 142 "..\..\..\..\..\Views\CashWithdrawal\CreditCard\CreditCardSelectionView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lstAccounts = ((System.Windows.Controls.ListView)(target));
            
            #line 144 "..\..\..\..\..\Views\CashWithdrawal\CreditCard\CreditCardSelectionView.xaml"
            this.lstAccounts.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lstAccounts_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtAccountNumber = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

