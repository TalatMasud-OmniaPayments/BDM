﻿#pragma checksum "..\..\..\..\Views\Pages\DiagnosticsView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6CAAD7C80697BB3948BF6E65385A2C444AF18AC8D29DCAC7AE2FC9BCD420A306"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Omnia.Pie.Supervisor.Shell.ViewModels.Devices;
using Omnia.Pie.Supervisor.Shell.Views.Devices;
using Omnia.Pie.Supervisor.Shell.Views.Pages;
using Omnia.Pie.Supervisor.UI.Themes.Controls;
using Omnia.Pie.Supervisor.UI.Themes.Controls.DateHelper;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
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


namespace Omnia.Pie.Supervisor.Shell.Views.Pages {
    
    
    /// <summary>
    /// DiagnosticsView
    /// </summary>
    public partial class DiagnosticsView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 95 "..\..\..\..\Views\Pages\DiagnosticsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView operations;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\..\Views\Pages\DiagnosticsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtStartDate;
        
        #line default
        #line hidden
        
        
        #line 139 "..\..\..\..\Views\Pages\DiagnosticsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpStartDate;
        
        #line default
        #line hidden
        
        
        #line 179 "..\..\..\..\Views\Pages\DiagnosticsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtEndDate;
        
        #line default
        #line hidden
        
        
        #line 190 "..\..\..\..\Views\Pages\DiagnosticsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpEndDate;
        
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
            System.Uri resourceLocater = new System.Uri("/Omnia.Pie.Supervisor.Shell;component/views/pages/diagnosticsview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Pages\DiagnosticsView.xaml"
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
            this.operations = ((System.Windows.Controls.ListView)(target));
            return;
            case 2:
            this.txtStartDate = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.dpStartDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 4:
            
            #line 174 "..\..\..\..\Views\Pages\DiagnosticsView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.dpStartDate_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.txtEndDate = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.dpEndDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 7:
            
            #line 232 "..\..\..\..\Views\Pages\DiagnosticsView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.dpEndDate_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
