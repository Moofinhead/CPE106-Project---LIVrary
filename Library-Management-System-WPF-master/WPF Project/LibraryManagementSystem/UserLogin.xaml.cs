using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LibraryMSWF.BL;
using LibraryMSWF.DAL;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http;
using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Permissions;
using Microsoft.Web.WebView2.Core;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Navigation;

namespace LibraryManagementSystem
{
    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Page
    {
        public static int userId;
        public static string userName = string.Empty;
        private string currentCaptchaText;
        private readonly Random random = new Random();
        private UserBL userBL = new UserBL();

        public UserLogin()
        {
            InitializeComponent();
            
            // Initially hide the signup panel
            SignupPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
            
            // Generate initial CAPTCHA
            GenerateNewCaptcha();
        }
        
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserDAL userDal = new UserDAL();
                int userId = userDal.UserLoginDAL(tbUserEmail.Text, tbUserPass.Password);
                if (userId > 0)
                {
                    UserLogin.userId = userId;
                    UserLogin.userName = userDal.TakeUserNameDAL(userId);

                    // Since we're in a Page, get the parent Window
                    Window parentWindow = Window.GetWindow(this);
                    
                    // Create and show UserHome window correctly
                    UserHome userHome = new UserHome(userId, parentWindow);
                    
                    // Close/hide the parent window
                    if (parentWindow != null)
                    {
                        parentWindow.Hide();
                    }
                    
                    userHome.Show();
                }
                else
                {
                    MessageBox.Show("Invalid email or password!", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
        
        private void BtnShowSignup_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            SignupPanel.Visibility = Visibility.Visible;
            GenerateNewCaptcha();
        }
        
        private void BtnShowLogin_Click(object sender, RoutedEventArgs e)
        {
            SignupPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
        }
        
        private bool IsEmailAlreadyRegistered(string email)
        {
            using (SqlConnection con = new SqlConnection("Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True"))
            {
                con.Open();
                String query = "SELECT COUNT(*) FROM UserTable WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        
        private void BtnRefreshCaptcha_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewCaptcha();
        }
        
        private void GenerateNewCaptcha()
        {
            // Clear the canvas
            captchaCanvas.Children.Clear();
            
            // Generate random text (5-6 characters)
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            int length = random.Next(5, 7);
            
            StringBuilder captchaBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                captchaBuilder.Append(chars[random.Next(chars.Length)]);
            }
            
            currentCaptchaText = captchaBuilder.ToString();
            
            // Draw noisy background
            for (int i = 0; i < 30; i++)
            {
                Line line = new Line
                {
                    X1 = random.Next(280),
                    Y1 = random.Next(60),
                    X2 = random.Next(280),
                    Y2 = random.Next(60),
                    Stroke = new SolidColorBrush(Color.FromArgb(
                        (byte)random.Next(100, 150),
                        (byte)random.Next(150, 255),
                        (byte)random.Next(150, 255),
                        (byte)random.Next(150, 255))),
                    StrokeThickness = random.Next(1, 3)
                };
                captchaCanvas.Children.Add(line);
            }
            
            // Draw text
            TextBlock textBlock = new TextBlock
            {
                Text = currentCaptchaText,
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White
            };
            
            // Apply transformations to make it harder to read by bots
            TransformGroup transformGroup = new TransformGroup();
            
            // Add a slight rotation
            RotateTransform rotateTransform = new RotateTransform
            {
                Angle = random.Next(-5, 5)
            };
            transformGroup.Children.Add(rotateTransform);
            
            textBlock.RenderTransform = transformGroup;
            
            // Add a small blur effect
            textBlock.Effect = new BlurEffect
            {
                Radius = 0.5
            };
            
            // Position the text in the center
            Canvas.SetLeft(textBlock, random.Next(10, 50));
            Canvas.SetTop(textBlock, random.Next(10, 20));
            
            captchaCanvas.Children.Add(textBlock);
        }
        
        private bool ValidateCaptcha()
        {
            return string.Equals(tbCaptchaInput.Text.Trim(), currentCaptchaText, 
                                StringComparison.OrdinalIgnoreCase);
        }
        
        private void BtnSignup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(tbSignupUsername.Text) ||
                    string.IsNullOrWhiteSpace(tbSignupEmail.Text) ||
                    string.IsNullOrWhiteSpace(tbSignupPass.Password) ||
                    string.IsNullOrWhiteSpace(tbSignupPassConfirm.Password))
                {
                    alertSignup.Text = "Please fill in all fields.";
                    return;
                }
                
                // Check if passwords match
                if (tbSignupPass.Password != tbSignupPassConfirm.Password)
                {
                    alertSignup.Text = "Passwords do not match.";
                    return;
                }
                
                // Validate CAPTCHA
                if (!ValidateCaptcha())
                {
                    alertSignup.Text = "Incorrect CAPTCHA. Please try again.";
                    GenerateNewCaptcha();
                    return;
                }
                
                // Check if email already exists
                string email = tbSignupEmail.Text;
                if (IsEmailAlreadyRegistered(email))
                {
                    alertSignup.Text = "Email already registered. Please use a different email.";
                    return;
                }
                
                // Send verification code and verify email
                PhoneVerificationWindow verificationWindow = new PhoneVerificationWindow(email);
                verificationWindow.ShowDialog();
                
                // If verification is successful, add the user to the database
                if (verificationWindow.IsVerified)
                {
                    string userName = tbSignupUsername.Text;
                    string userEmail = tbSignupEmail.Text;
                    string userPass = tbSignupPass.Password;
                    
                    string result = userBL.AddUserBL(userName, userEmail, userPass);
                    
                    if (result == "true")
                    {
                        // Signup successful
                        MessageBox.Show("Account created successfully. You can now login.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        // Switch to login panel instead of trying to navigate elsewhere
                        BtnShowLogin_Click(sender, e);
                        
                        // Pre-fill the login fields with the registration information for convenience
                        tbUserEmail.Text = userEmail;
                        tbUserPass.Password = string.Empty; // For security, don't pre-fill password
                    }
                    else
                    {
                        // Signup failed
                        alertSignup.Text = result;
                    }
                }
                else
                {
                    alertSignup.Text = "Email verification failed. Please try again.";
                }
            }
            catch (Exception ex)
            {
                alertSignup.Text = "Registration error: " + ex.Message;
            }
        }
    }
}
