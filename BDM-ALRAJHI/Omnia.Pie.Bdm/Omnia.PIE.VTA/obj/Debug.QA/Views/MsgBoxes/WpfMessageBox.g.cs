﻿#pragma checksum "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A8321C21EC61D40BF5B05909CD3B04389BF6C120"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Omnia.PIE.VTA.Views.MsgBoxes;
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


namespace Omnia.PIE.VTA.Views.MsgBoxes {
    
    
    /// <summary>
    /// WpfMessageBox
    /// </summary>
    public partial class WpfMessageBox : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel MainHead;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MessageTitle;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnClose;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image MessageImage;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MessageBlock;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnYes;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnNo;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
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
            System.Uri resourceLocater = new System.Uri("/Omnia.PIE.VTA;component/views/msgboxes/wpfmessagebox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
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
            this.MainHead = ((System.Windows.Controls.DockPanel)(target));
            
            #line 31 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
            this.MainHead.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.MainHead_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MessageTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.BtnClose = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
            this.BtnClose.Click += new System.Windows.RoutedEventHandler(this.BtnClose_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.MessageImage = ((System.Windows.Controls.Image)(target));
            return;
            case 5:
            this.MessageBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.btnOk_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnYes = ((System.Windows.Controls.Button)(target));
            
            #line 48 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
            this.btnYes.Click += new System.Windows.RoutedEventHandler(this.btnYes_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnNo = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
            this.btnNo.Click += new System.Windows.RoutedEventHandler(this.btnNo_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\..\Views\MsgBoxes\WpfMessageBox.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.btnCancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

