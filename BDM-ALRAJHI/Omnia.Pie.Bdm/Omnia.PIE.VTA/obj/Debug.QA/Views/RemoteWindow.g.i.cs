﻿#pragma checksum "..\..\..\Views\RemoteWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C5F43608DD1CEE0CA6669FF211BC6EA1C18CB3D4"
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
using System.Windows.Forms;
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
    /// RemoteWindow
    /// </summary>
    public partial class RemoteWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\Views\RemoteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border titleBorder;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\Views\RemoteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel dplMainHead;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Views\RemoteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txt3;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Views\RemoteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtTitle;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\Views\RemoteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnClose;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\Views\RemoteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMaximize;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\Views\RemoteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMinimize;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\Views\RemoteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Forms.Panel videoPanel;
        
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
            System.Uri resourceLocater = new System.Uri("/Omnia.PIE.VTA;component/views/remotewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\RemoteWindow.xaml"
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
            
            #line 16 "..\..\..\Views\RemoteWindow.xaml"
            ((Omnia.PIE.VTA.Views.RemoteWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 20 "..\..\..\Views\RemoteWindow.xaml"
            ((Omnia.PIE.VTA.Views.RemoteWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.titleBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.dplMainHead = ((System.Windows.Controls.DockPanel)(target));
            
            #line 33 "..\..\..\Views\RemoteWindow.xaml"
            this.dplMainHead.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.DplMainHead_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txt3 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.txtTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.BtnClose = ((System.Windows.Controls.Button)(target));
            
            #line 57 "..\..\..\Views\RemoteWindow.xaml"
            this.BtnClose.Click += new System.Windows.RoutedEventHandler(this.BtnClose_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnMaximize = ((System.Windows.Controls.Button)(target));
            
            #line 62 "..\..\..\Views\RemoteWindow.xaml"
            this.btnMaximize.Click += new System.Windows.RoutedEventHandler(this.btnMaximize_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnMinimize = ((System.Windows.Controls.Button)(target));
            
            #line 68 "..\..\..\Views\RemoteWindow.xaml"
            this.btnMinimize.Click += new System.Windows.RoutedEventHandler(this.btnMinimize_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.videoPanel = ((System.Windows.Forms.Panel)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

