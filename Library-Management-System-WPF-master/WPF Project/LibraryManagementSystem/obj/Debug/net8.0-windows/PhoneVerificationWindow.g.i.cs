﻿#pragma checksum "..\..\..\PhoneVerificationWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6A411976F961D15E02191C11614EF780FAD1A4B0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace LibraryManagementSystem {
    
    
    /// <summary>
    /// PhoneVerificationWindow
    /// </summary>
    public partial class PhoneVerificationWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 47 "..\..\..\PhoneVerificationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock debugText;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\PhoneVerificationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtVerificationCode;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\PhoneVerificationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock timerText;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\PhoneVerificationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnResend;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\PhoneVerificationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock alertText;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\PhoneVerificationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnVerify;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\PhoneVerificationWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/LibraryManagementSystem;component/phoneverificationwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\PhoneVerificationWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.debugText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.txtVerificationCode = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.timerText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.btnResend = ((System.Windows.Controls.Button)(target));
            
            #line 76 "..\..\..\PhoneVerificationWindow.xaml"
            this.btnResend.Click += new System.Windows.RoutedEventHandler(this.BtnResend_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.alertText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.btnVerify = ((System.Windows.Controls.Button)(target));
            
            #line 97 "..\..\..\PhoneVerificationWindow.xaml"
            this.btnVerify.Click += new System.Windows.RoutedEventHandler(this.BtnVerify_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 105 "..\..\..\PhoneVerificationWindow.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.BtnCancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

