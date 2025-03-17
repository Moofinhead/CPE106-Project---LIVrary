using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Fade in welcome text
            var welcomeStoryboard = new Storyboard();
            var welcomeFadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(2)
            };
            Storyboard.SetTarget(welcomeFadeIn, WelcomeText);
            Storyboard.SetTargetProperty(welcomeFadeIn, new PropertyPath(OpacityProperty));
            welcomeStoryboard.Children.Add(welcomeFadeIn);
            welcomeStoryboard.Begin();

            await Task.Delay(3000); // Wait 3 seconds

            // Move welcome text up
            var moveUpStoryboard = new Storyboard();
            var moveUp = new DoubleAnimation
            {
                From = 0,
                To = -100,
                Duration = TimeSpan.FromSeconds(1)
            };
            Storyboard.SetTarget(moveUp, WelcomeText);
            Storyboard.SetTargetProperty(moveUp, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            moveUpStoryboard.Children.Add(moveUp);
            moveUpStoryboard.Begin();

            // Fade in logo
            var logoStoryboard = new Storyboard();
            var logoFadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(2)
            };
            Storyboard.SetTarget(logoFadeIn, AnimationLogo);
            Storyboard.SetTargetProperty(logoFadeIn, new PropertyPath(OpacityProperty));
            logoStoryboard.Children.Add(logoFadeIn);
            logoStoryboard.Begin();

            await Task.Delay(3000); // Wait 3 more seconds

            // Fade out animation layer
            var finalStoryboard = new Storyboard();
            
            var animationLayerFadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(2)
            };
            Storyboard.SetTarget(animationLayerFadeOut, AnimationLayer);
            Storyboard.SetTargetProperty(animationLayerFadeOut, new PropertyPath(OpacityProperty));
            finalStoryboard.Children.Add(animationLayerFadeOut);
            finalStoryboard.Begin();

            // Make sure animation layer doesn't block clicks after fade out
            AnimationLayer.IsHitTestVisible = false;

            // Navigate to MainPage
            MainFrame.Navigate(new MainPage());
        }

        public async Task SlideToPage(Page nextPage, bool slideLeft)
        {
            var currentPage = MainFrame.Content as Page;
            if (currentPage == null) return;

            // Create transform for current page
            var currentTransform = new TranslateTransform();
            currentPage.RenderTransform = currentTransform;

            // Set up next page
            nextPage.RenderTransform = new TranslateTransform(slideLeft ? this.ActualWidth : -this.ActualWidth, 0);
            MainFrame.NavigationService.Navigate(nextPage);

            // Animate both pages
            var storyboard = new Storyboard();
            
            var currentAnimation = new DoubleAnimation
            {
                From = 0,
                To = slideLeft ? -this.ActualWidth : this.ActualWidth,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            Storyboard.SetTarget(currentAnimation, currentPage);
            Storyboard.SetTargetProperty(currentAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            
            var nextAnimation = new DoubleAnimation
            {
                From = slideLeft ? this.ActualWidth : -this.ActualWidth,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            Storyboard.SetTarget(nextAnimation, nextPage);
            Storyboard.SetTargetProperty(nextAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            storyboard.Children.Add(currentAnimation);
            storyboard.Children.Add(nextAnimation);

            storyboard.Begin();
            await Task.Delay(500); // Wait for animation to complete
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
