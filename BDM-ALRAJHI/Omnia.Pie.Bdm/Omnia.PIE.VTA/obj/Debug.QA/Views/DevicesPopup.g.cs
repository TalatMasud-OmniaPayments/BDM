﻿#pragma checksum "..\..\..\Views\DevicesPopup.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "89563734B747B7461BDE41CC8410F5D8D1D69EB8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
    /// DevicesPopup
    /// </summary>
    public partial class DevicesPopup : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\Views\DevicesPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border titleBorder;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Views\DevicesPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel dplMainHead;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Views\DevicesPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtTitle;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Views\DevicesPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPopIn;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Views\DevicesPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Omnia.PIE.VTA.Views.Devices DevicesControl;
        
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
            System.Uri resourceLocater = new System.Uri("/Omnia.PIE.VTA;component/views/devicespopup.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\DevicesPopup.xaml"
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
            this.titleBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.dplMainHead = ((System.Windows.Controls.DockPanel)(target));
            
            #line 27 "..\..\..\Views\DevicesPopup.xaml"
            this.dplMainHead.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.DplMainHead_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.btnPopIn = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\Views\DevicesPopup.xaml"
            this.btnPopIn.Click += new System.Windows.RoutedEventHandler(this.btnPopIn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.DevicesControl = ((Omnia.PIE.VTA.Views.Devices)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
