﻿#pragma checksum "..\..\..\..\Views\UserControls\Devices.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "702B2D60CF768654A49DC7B6C342CDFCB2959A21"
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
using Omnia.PIE.VTA.Views;
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


namespace Omnia.PIE.VTA.Views {
    
    
    /// <summary>
    /// Devices
    /// </summary>
    public partial class Devices : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 71 "..\..\..\..\Views\UserControls\Devices.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button InterNet;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\..\Views\UserControls\Devices.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button VPN;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\..\Views\UserControls\Devices.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPopout;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\..\..\Views\UserControls\Devices.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lstCashCassettes;
        
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
            System.Uri resourceLocater = new System.Uri("/Omnia.PIE.VTA;component/views/usercontrols/devices.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\UserControls\Devices.xaml"
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
            
            #line 12 "..\..\..\..\Views\UserControls\Devices.xaml"
            ((Omnia.PIE.VTA.Views.Devices)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.InterNet = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.VPN = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.btnPopout = ((System.Windows.Controls.Button)(target));
            
            #line 82 "..\..\..\..\Views\UserControls\Devices.xaml"
            this.btnPopout.Click += new System.Windows.RoutedEventHandler(this.btnPopout_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.lstCashCassettes = ((System.Windows.Controls.ListBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

