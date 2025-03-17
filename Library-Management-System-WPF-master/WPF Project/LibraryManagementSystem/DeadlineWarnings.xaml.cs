using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryManagementSystem
{
    public partial class DeadlineWarnings : Window
    {
        public DeadlineWarnings()
        {
            InitializeComponent();
            
            // Load dummy data
            LoadDummyData();
            
            // Add close button functionality
            AddCloseButton();
        }
        
        private void LoadDummyData()
        {
            // Create dummy data for the user (Sir JP)
            List<BookDueInfo> dueBooks = new List<BookDueInfo>
            {
                new BookDueInfo
                {
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    BorrowDate = DateTime.Now.AddDays(-14),
                    DueDate = DateTime.Now,
                    Status = DueStatus.DueToday
                },
                new BookDueInfo
                {
                    Title = "1984",
                    Author = "George Orwell",
                    BorrowDate = DateTime.Now.AddDays(-16),
                    DueDate = DateTime.Now.AddDays(-2),
                    Status = DueStatus.Overdue
                },
                new BookDueInfo
                {
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    BorrowDate = DateTime.Now.AddDays(-10),
                    DueDate = DateTime.Now.AddDays(2),
                    Status = DueStatus.DueThisWeek
                },
                new BookDueInfo
                {
                    Title = "The Catcher in the Rye",
                    Author = "J.D. Salinger",
                    BorrowDate = DateTime.Now.AddDays(-8),
                    DueDate = DateTime.Now.AddDays(6),
                    Status = DueStatus.DueThisWeek
                },
                new BookDueInfo
                {
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    BorrowDate = DateTime.Now.AddDays(-12),
                    DueDate = DateTime.Now.AddDays(2),
                    Status = DueStatus.DueThisWeek
                }
            };
            
            // Clear existing items
            ListView listView = FindName("ListView") as ListView;
            if (listView != null)
            {
                listView.Items.Clear();
                
                // Add new items
                foreach (var book in dueBooks)
                {
                    ListViewItem item = CreateBookDueItem(book);
                    listView.Items.Add(item);
                }
            }
            
            // Update the stats
            UpdateStats(dueBooks);
        }
        
        private void UpdateStats(List<BookDueInfo> books)
        {
            int dueToday = 0;
            int dueThisWeek = 0;
            int overdue = 0;
            
            // Count manually instead of using LINQ
            foreach (var book in books)
            {
                if (book.Status == DueStatus.DueToday)
                    dueToday++;
                else if (book.Status == DueStatus.DueThisWeek)
                    dueThisWeek++;
                else if (book.Status == DueStatus.Overdue)
                    overdue++;
            }
            
            // Find the TextBlocks and update them
            var textBlocks = FindVisualChildren<TextBlock>(this);
            foreach (var tb in textBlocks)
            {
                if (tb.Text == "3")
                    tb.Text = dueToday.ToString();
                else if (tb.Text == "8")
                    tb.Text = dueThisWeek.ToString();
                else if (tb.Text == "2")
                    tb.Text = overdue.ToString();
            }
        }
        
        private ListViewItem CreateBookDueItem(BookDueInfo book)
        {
            ListViewItem item = new ListViewItem();
            
            // Set background color based on status
            switch (book.Status)
            {
                case DueStatus.DueToday:
                    item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF8E6"));
                    break;
                case DueStatus.Overdue:
                    item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEBEE"));
                    break;
                case DueStatus.DueThisWeek:
                    item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8F5E9"));
                    break;
            }
            
            item.Margin = new Thickness(0, 0, 0, 10);
            item.Padding = new Thickness(20);
            
            // Create grid layout
            Grid grid = new Grid();
            grid.Width = 1000;
            
            // Define columns
            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = GridLength.Auto;
            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition col3 = new ColumnDefinition();
            col3.Width = GridLength.Auto;
            
            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);
            grid.ColumnDefinitions.Add(col3);
            
            // Book icon
            Border iconBorder = new Border();
            iconBorder.Width = 50;
            iconBorder.Height = 70;
            iconBorder.CornerRadius = new CornerRadius(5);
            
            // Set icon background color based on status
            switch (book.Status)
            {
                case DueStatus.DueToday:
                    iconBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFECB3"));
                    break;
                case DueStatus.Overdue:
                    iconBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCDD2"));
                    break;
                case DueStatus.DueThisWeek:
                    iconBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C8E6C9"));
                    break;
            }
            
            TextBlock iconText = new TextBlock();
            iconText.Text = "📚";
            iconText.HorizontalAlignment = HorizontalAlignment.Center;
            iconText.VerticalAlignment = VerticalAlignment.Center;
            
            iconBorder.Child = iconText;
            
            // Book info
            StackPanel infoPanel = new StackPanel();
            infoPanel.Margin = new Thickness(20, 0, 0, 0);
            
            TextBlock titleText = new TextBlock();
            titleText.Text = book.Title;
            titleText.FontSize = 16;
            titleText.FontWeight = FontWeights.SemiBold;
            
            TextBlock authorText = new TextBlock();
            authorText.Text = book.Author;
            authorText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
            
            TextBlock borrowedText = new TextBlock();
            borrowedText.Text = $"Borrowed: {book.BorrowDate:MMM d, yyyy}";
            borrowedText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
            
            infoPanel.Children.Add(titleText);
            infoPanel.Children.Add(authorText);
            infoPanel.Children.Add(borrowedText);
            
            Grid.SetColumn(infoPanel, 1);
            
            // Status
            Border statusBorder = new Border();
            statusBorder.CornerRadius = new CornerRadius(5);
            statusBorder.Padding = new Thickness(15, 8, 15, 8);
            
            TextBlock statusText = new TextBlock();
            statusText.Foreground = Brushes.White;
            
            // Set status text and color based on status
            switch (book.Status)
            {
                case DueStatus.DueToday:
                    statusText.Text = "Due Today!";
                    statusBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA726"));
                    break;
                case DueStatus.Overdue:
                    int daysOverdue = (int)(DateTime.Now - book.DueDate).TotalDays;
                    statusText.Text = $"{daysOverdue} Days Overdue!";
                    statusBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336"));
                    break;
                case DueStatus.DueThisWeek:
                    int daysLeft = (int)(book.DueDate - DateTime.Now).TotalDays;
                    statusText.Text = $"Due in {daysLeft} days";
                    statusBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
                    break;
            }
            
            statusBorder.Child = statusText;
            Grid.SetColumn(statusBorder, 2);
            
            // Add elements to grid
            grid.Children.Add(iconBorder);
            grid.Children.Add(infoPanel);
            grid.Children.Add(statusBorder);
            
            item.Content = grid;
            
            return item;
        }
        
        private void AddCloseButton()
        {
            // Find the menu button and replace it with a close button
            var buttons = FindVisualChildren<Button>(this);
            Button menuButton = null;
            
            // Find the menu button manually
            foreach (var button in buttons)
            {
                if (button.Content is TextBlock tb && tb.Text == "☰")
                {
                    menuButton = button;
                    break;
                }
            }
            
            if (menuButton != null)
            {
                menuButton.Click += (sender, e) => this.Close();
                
                // Change the icon to X
                if (menuButton.Content is TextBlock tb)
                {
                    tb.Text = "✕";
                }
                
                // Add tooltip
                menuButton.ToolTip = "Close";
            }
        }
        
        // Helper method to find visual children
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
    
    // Helper classes for the dummy data
    public enum DueStatus
    {
        DueToday,
        DueThisWeek,
        Overdue
    }
    
    public class BookDueInfo
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DueStatus Status { get; set; }
    }
} 