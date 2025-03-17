using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LibraryManagementSystem.Controls
{
    public partial class CaptchaControl : UserControl
    {
        private string _captchaText;
        
        public string CaptchaText => _captchaText;

        public CaptchaControl()
        {
            InitializeComponent();
            GenerateNewCaptcha();
        }

        public void GenerateNewCaptcha()
        {
            // Generate random captcha text (mix of numbers and letters)
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            _captchaText = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            // Create the captcha image
            var drawingVisual = new DrawingVisual();
            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                // Draw distorted background
                for (int i = 0; i < 30; i++)
                {
                    dc.DrawLine(
                        new Pen(new SolidColorBrush(Color.FromRgb((byte)random.Next(255), 
                            (byte)random.Next(255), (byte)random.Next(255))), 1),
                        new Point(random.Next(0, 150), random.Next(0, 50)),
                        new Point(random.Next(0, 150), random.Next(0, 50))
                    );
                }

                // Draw the text with random transformations
                FormattedText formattedText = new FormattedText(
                    _captchaText,
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    24,
                    Brushes.Black,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip);

                // Apply random transformations to each character
                for (int i = 0; i < _captchaText.Length; i++)
                {
                    var charText = new FormattedText(
                        _captchaText[i].ToString(),
                        CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        24,
                        new SolidColorBrush(Color.FromRgb((byte)random.Next(0, 100), 
                            (byte)random.Next(0, 100), (byte)random.Next(0, 100))),
                        VisualTreeHelper.GetDpi(this).PixelsPerDip);

                    dc.PushTransform(new RotateTransform(random.Next(-30, 30)));
                    dc.DrawText(charText, new Point(20 * i + 10, random.Next(5, 15)));
                    dc.Pop();
                }
            }

            // Convert to image
            var renderTarget = new RenderTargetBitmap(
                150, 50, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(drawingVisual);

            captchaImage.Source = renderTarget;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewCaptcha();
        }
    }
}