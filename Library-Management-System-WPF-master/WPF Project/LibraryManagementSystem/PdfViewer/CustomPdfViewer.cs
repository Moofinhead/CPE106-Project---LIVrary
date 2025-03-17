using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows;
namespace LibraryManagementSystem.PdfViewer
{
    public class CustomPdfViewer : UserControl
    {
        private readonly Microsoft.Web.WebView2.Wpf.WebView2 _webView;
        private const string PdfJsPath = "pdfjs/web/viewer.html";
        private bool _isInitialized;

        public CustomPdfViewer()
        {
            MessageBox.Show("CustomPdfViewer constructor called");
            _webView = new Microsoft.Web.WebView2.Wpf.WebView2();
            Content = _webView;
            Loaded += CustomPdfViewer_Loaded;
        }

        private async void CustomPdfViewer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("CustomPdfViewer_Loaded called");
            if (!_isInitialized)
            {
                await InitializeAsync();
                _isInitialized = true;
            }
        }

        private async Task InitializeAsync()
        {
            try
            {
                MessageBox.Show("InitializeAsync started");
                
                var webView2Environment = await CoreWebView2Environment.CreateAsync(null, null, new CoreWebView2EnvironmentOptions
                {
                    AllowSingleSignOnUsingOSPrimaryAccount = false,
                    AdditionalBrowserArguments = "--allow-file-access-from-files"
                });
                
                await _webView.EnsureCoreWebView2Async(webView2Environment);
                
                // Configure WebView2 settings
                _webView.CoreWebView2.Settings.IsZoomControlEnabled = true;
                _webView.CoreWebView2.Settings.IsStatusBarEnabled = false;
                _webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
                _webView.CoreWebView2.Settings.AreHostObjectsAllowed = true;
                _webView.CoreWebView2.Settings.IsWebMessageEnabled = true;
                
                // Allow local file access
                _webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    "pdfjs", 
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdfjs"),
                    CoreWebView2HostResourceAccessKind.Allow);

                await _webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
                    window.addEventListener('DOMContentLoaded', () => {
                        console.log('DOMContentLoaded event fired');
                        const viewer = document.querySelector('#viewer');
                        if (viewer) {
                            console.log('Viewer element found');
                            viewer.style.backgroundColor = '#f5f5f5';
                            viewer.style.overflow = 'auto';
                            viewer.style.height = '100%';
                        } else {
                            console.log('Viewer element not found');
                        }
                    });
                ");
                MessageBox.Show("InitializeAsync completed successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing PDF viewer: {ex.Message}\nStack trace: {ex.StackTrace}");
            }
        }

        public async Task LoadPdfAsync(string pdfPath)
        {
            MessageBox.Show($"LoadPdfAsync called with path: {pdfPath}");
            
            if (!_isInitialized)
            {
                MessageBox.Show("PDF viewer not initialized, initializing now...");
                await InitializeAsync();
                _isInitialized = true;
            }

            if (!File.Exists(pdfPath))
            {
                MessageBox.Show($"PDF file does not exist at: {pdfPath}");
                return;
            }

            try
            {
                // Use Path.Combine for proper path handling
                string viewerPath = Path.GetFullPath(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "pdfjs",
                    "web",
                    "viewer.html"
                ));
                
                MessageBox.Show($"Viewer path: {viewerPath}");
                
                if (!File.Exists(viewerPath))
                {
                    MessageBox.Show($"PDF viewer not found at: {viewerPath}");
                    return;
                }

                // For URLs, always use forward slashes
                string viewerUrl = viewerPath.Replace("\\", "/");
                string pdfUrl = pdfPath.Replace("\\", "/");
                
                MessageBox.Show($"Viewer URL: file:///{viewerUrl}");
                MessageBox.Show($"PDF URL: {pdfUrl}");
                
                string fullUrl = $"file:///{viewerUrl}?file={Uri.EscapeDataString(pdfUrl)}";
                MessageBox.Show($"Full URL: {fullUrl}");
                
                _webView.CoreWebView2.Navigate(fullUrl);
                MessageBox.Show("Navigation initiated");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading PDF: {ex.Message}\nStack trace: {ex.StackTrace}");
            }
        }
    }
} 