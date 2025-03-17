using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementSystem
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                await mainWindow.SlideToPage(new AdminLogin(), false); // Slide right
            }
        }

        private async void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                await mainWindow.SlideToPage(new UserLogin(), true); // Slide left
            }
        }
    }
} 