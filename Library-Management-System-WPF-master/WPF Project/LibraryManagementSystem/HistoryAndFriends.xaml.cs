using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryManagementSystem
{
    /// <summary>
    /// Interaction logic for HistoryAndFriends.xaml
    /// </summary>
    public partial class HistoryAndFriends : UserControl
    {
        public HistoryAndFriends()
        {
            InitializeComponent();
            
            // You could load data here if needed
            // LoadHistoryData();
            // LoadFriendsData();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Find the parent Window and close it
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }
        
        private void AddFriend_Click(object sender, RoutedEventArgs e)
        {
            // Show a simple dialog to add a friend
            string friendName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter friend's username:", 
                "Add Friend", 
                "", 
                -1, 
                -1);
                
            if (!string.IsNullOrWhiteSpace(friendName))
            {
                // Create a new friend activity item
                Border friendBorder = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF383838")),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 0, 0, 10),
                    CornerRadius = new CornerRadius(5)
                };
                
                StackPanel friendPanel = new StackPanel();
                
                TextBlock nameText = new TextBlock
                {
                    Text = $"{friendName} was added:",
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF666666"))
                };
                
                TextBlock statusText = new TextBlock
                {
                    Text = "New Friend Added",
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 16,
                    Margin = new Thickness(0, 5, 0, 0)
                };
                
                TextBlock dateText = new TextBlock
                {
                    Text = DateTime.Now.ToString("MMMM dd, yyyy"),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF4081"))
                };
                
                friendPanel.Children.Add(nameText);
                friendPanel.Children.Add(statusText);
                friendPanel.Children.Add(dateText);
                
                friendBorder.Child = friendPanel;
                
                // Add to the friends panel
                FriendsPanel.Children.Insert(0, friendBorder);
                
                MessageBox.Show($"Friend request sent to {friendName}!", "Friend Added", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private void AddToReadingList_Click(object sender, RoutedEventArgs e)
        {
            // Add the recommended book to reading list
            Button button = sender as Button;
            if (button != null && button.Parent is StackPanel panel)
            {
                // Find the book title in the panel
                string bookTitle = "";
                foreach (var child in panel.Children)
                {
                    if (child is TextBlock tb && tb.FontSize == 16)
                    {
                        bookTitle = tb.Text;
                        break;
                    }
                }
                
                if (!string.IsNullOrEmpty(bookTitle))
                {
                    // Add to history list
                    ListViewItem newItem = new ListViewItem
                    {
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF383838")),
                        Margin = new Thickness(0, 0, 0, 10)
                    };
                    
                    Grid itemGrid = new Grid { Margin = new Thickness(10) };
                    
                    // Add columns
                    itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                    itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    
                    // Book icon
                    Border iconBorder = new Border
                    {
                        Width = 40,
                        Height = 50,
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9b59b6"))
                    };
                    
                    TextBlock iconText = new TextBlock
                    {
                        Text = "REC",
                        Foreground = new SolidColorBrush(Colors.White),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    
                    iconBorder.Child = iconText;
                    
                    // Book info
                    StackPanel infoPanel = new StackPanel { Margin = new Thickness(10, 0, 0, 0) };
                    
                    TextBlock titleText = new TextBlock
                    {
                        Text = bookTitle,
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    
                    TextBlock dateText = new TextBlock
                    {
                        Text = $"Added: {DateTime.Now.ToString("MMM dd, yyyy")}",
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF666666"))
                    };
                    
                    infoPanel.Children.Add(titleText);
                    infoPanel.Children.Add(dateText);
                    
                    // Status
                    Border statusBorder = new Border
                    {
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9b59b6")),
                        CornerRadius = new CornerRadius(3),
                        Padding = new Thickness(8, 4, 8, 4)
                    };
                    
                    TextBlock statusText = new TextBlock
                    {
                        Text = "Added to List",
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    
                    statusBorder.Child = statusText;
                    
                    // Add elements to grid
                    Grid.SetColumn(iconBorder, 0);
                    Grid.SetColumn(infoPanel, 1);
                    Grid.SetColumn(statusBorder, 2);
                    
                    itemGrid.Children.Add(iconBorder);
                    itemGrid.Children.Add(infoPanel);
                    itemGrid.Children.Add(statusBorder);
                    
                    newItem.Content = itemGrid;
                    
                    // Add to list
                    HistoryListView.Items.Insert(0, newItem);
                    
                    // Update the button
                    button.Content = "Added to List";
                    button.IsEnabled = false;
                    button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7f8c8d"));
                }
            }
        }
    }
}