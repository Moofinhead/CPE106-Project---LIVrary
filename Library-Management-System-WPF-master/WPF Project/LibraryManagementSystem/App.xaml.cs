using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;

namespace LibraryManagementSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Enable modern browser emulation
            SetBrowserFeatureControl();
        }

        private void SetBrowserFeatureControl()
        {
            // Feature control settings are per-process
            var fileName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            // make the control emulate IE11
            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, 11001);
        }

        private void SetBrowserFeatureControlKey(string feature, string appName, uint value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Internet Explorer\Main\FeatureControl\" + feature,
                RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key.SetValue(appName, value, RegistryValueKind.DWord);
            }
        }
    }
}
