﻿#pragma checksum "..\..\..\..\..\Views\Workflow\CashDepositAccount\CashDepositAccountConfirmDeposit.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B085CB27183D47C9545DD05BEDD164B83C29D976"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Omnia.PIE.VTA.Converters;
using Omnia.PIE.VTA.Views.Workflow;
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


namespace Omnia.PIE.VTA.Views.Workflow {
    
    
    /// <summary>
    /// CashDepositAccountConfirmDeposit
    /// </summary>
    public partial class CashDepositAccountConfirmDeposit : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\..\..\Views\Workflow\CashDepositAccount\CashDepositAccountConfirmDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdAccounts;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\..\..\Views\Workflow\CashDepositAccount\CashDepositAccountConfirmDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddMore;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\..\..\Views\Workflow\CashDepositAccount\CashDepositAccountConfirmDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnConfirmDeposit;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\..\..\Views\Workflow\CashDepositAccount\CashDepositAccountConfirmDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancelDeposit;
        
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
            System.Uri resourceLocater = new System.Uri("/Omnia.PIE.VTA;component/views/workflow/cashdepositaccount/cashdepositaccountconf" +
                    "irmdeposit.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\Workflow\CashDepositAccount\CashDepositAccountConfirmDeposit.xaml"
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
            
            #line 12 "..\..\..\..\..\Views\Workflow\CashDepositAccount\CashDepositAccountConfirmDeposit.xaml"
            ((Omnia.PIE.VTA.Views.Workflow.CashDepositAccountConfirmDeposit)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.grdAccounts = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.btnAddMore = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.btnConfirmDeposit = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.btnCancelDeposit = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

