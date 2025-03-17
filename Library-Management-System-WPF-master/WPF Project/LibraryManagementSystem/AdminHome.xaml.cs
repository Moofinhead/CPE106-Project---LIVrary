using System;
using System.Collections.Generic;
using System.Data;
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

namespace LibraryManagementSystem
{
    /// <summary>
    /// Interaction logic for AdminHome.xaml
    /// </summary>
    public partial class AdminHome : Window
    {
        private readonly AdminBL adminBL = new AdminBL();
        
        //SHOW BOOK MENU USER CONTROLLER ONLY =>PL
        public AdminHome()
        {
            InitializeComponent();
            LoadBooksData(); // Load books data by default
        }
        
        //SHOW BOOK MENU USER CONTROLLER ONLY =>PL
        private void BtnBooks_Click(object sender, RoutedEventArgs e)
        {
            LoadBooksData();
        }
        
        //SHOW USER MENU USER CONTROLLER ONLY =>PL
        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            LoadUsersData();
        }
        
        //SHOW REQUEST MENU USER CONTROLLER ONLY =>PL
        private void BtnRequests_Click(object sender, RoutedEventArgs e)
        {
            LoadRequestsData();
        }
        
        //SHOW ACCEPTED MENU USER CONTROLLER ONLY =>PL
        private void BtnAccepted_Click(object sender, RoutedEventArgs e)
        {
            LoadAcceptedData();
        }
        
        //LOGOUT ADMIN HOME =>PL
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        //SHOW RETURB MENU USER CONTROLLER ONLY =>PL
        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            LoadReturnData();
        }
        
        // Helper methods to load data
        private void LoadBooksData()
        {
            try
            {
                // Clear the stack panel
                adminStackPanel.Children.Clear();
                
                // Get books data
                DataTable booksData = adminBL.GetAllBooksBL();
                
                // Create a DataGrid
                DataGrid booksGrid = new DataGrid
                {
                    AutoGenerateColumns = true,
                    IsReadOnly = true,
                    ItemsSource = booksData.DefaultView,
                    Margin = new Thickness(10),
                    Height = 300,
                    VerticalAlignment = VerticalAlignment.Top
                };
                
                // Add to stack panel
                adminStackPanel.Children.Add(booksGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
                // Add a fallback UI element
                TextBlock errorText = new TextBlock
                {
                    Text = "Could not load books data. Please try again later.",
                    Margin = new Thickness(20),
                    FontSize = 16,
                    TextWrapping = TextWrapping.Wrap
                };
                adminStackPanel.Children.Add(errorText);
            }
        }
        
        private void LoadUsersData()
        {
            try
            {
                // Clear the stack panel
                adminStackPanel.Children.Clear();
                
                // Get users data
                DataTable usersData = adminBL.GetAllUsersBL();
                
                // Create a DataGrid
                DataGrid usersGrid = new DataGrid
                {
                    AutoGenerateColumns = true,
                    IsReadOnly = true,
                    ItemsSource = usersData.DefaultView,
                    Margin = new Thickness(10),
                    Height = 300,
                    VerticalAlignment = VerticalAlignment.Top
                };
                
                // Add to stack panel
                adminStackPanel.Children.Add(usersGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
                // Add a fallback UI element
                TextBlock errorText = new TextBlock
                {
                    Text = "Could not load users data. Please try again later.",
                    Margin = new Thickness(20),
                    FontSize = 16,
                    TextWrapping = TextWrapping.Wrap
                };
                adminStackPanel.Children.Add(errorText);
            }
        }
        
        private void LoadRequestsData()
        {
            try
            {
                // Clear the stack panel
                adminStackPanel.Children.Clear();
                
                // Get requests data
                DataTable requestsData = adminBL.GetAllRequestsBL();
                
                // Create a DataGrid
                DataGrid requestsGrid = new DataGrid
                {
                    AutoGenerateColumns = true,
                    IsReadOnly = true,
                    ItemsSource = requestsData.DefaultView,
                    Margin = new Thickness(10),
                    Height = 300,
                    VerticalAlignment = VerticalAlignment.Top
                };
                
                // Add to stack panel
                adminStackPanel.Children.Add(requestsGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading requests: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
                // Add a fallback UI element
                TextBlock errorText = new TextBlock
                {
                    Text = "Could not load requests data. Please try again later.",
                    Margin = new Thickness(20),
                    FontSize = 16,
                    TextWrapping = TextWrapping.Wrap
                };
                adminStackPanel.Children.Add(errorText);
            }
        }
        
        private void LoadAcceptedData()
        {
            try
            {
                // Clear the stack panel
                adminStackPanel.Children.Clear();
                
                // Get accepted requests data
                DataTable acceptedData = adminBL.GetAllAcceptedRequestsBL();
                
                // Create a DataGrid
                DataGrid acceptedGrid = new DataGrid
                {
                    AutoGenerateColumns = true,
                    IsReadOnly = true,
                    ItemsSource = acceptedData.DefaultView,
                    Margin = new Thickness(10),
                    Height = 300,
                    VerticalAlignment = VerticalAlignment.Top
                };
                
                // Add to stack panel
                adminStackPanel.Children.Add(acceptedGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading accepted requests: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
                // Add a fallback UI element
                TextBlock errorText = new TextBlock
                {
                    Text = "Could not load accepted requests data. Please try again later.",
                    Margin = new Thickness(20),
                    FontSize = 16,
                    TextWrapping = TextWrapping.Wrap
                };
                adminStackPanel.Children.Add(errorText);
            }
        }
        
        private void LoadReturnData()
        {
            try
            {
                // Clear the stack panel
                adminStackPanel.Children.Clear();
                
                // For now, just show a message
                TextBlock infoText = new TextBlock
                {
                    Text = "Return functionality is under development.",
                    Margin = new Thickness(20),
                    FontSize = 16,
                    TextWrapping = TextWrapping.Wrap
                };
                adminStackPanel.Children.Add(infoText);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
