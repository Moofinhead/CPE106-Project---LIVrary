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
using LibraryMSWF.DAL;
using LibraryMSWF.Entity;
using System.Data;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace LibraryManagementSystem
{
    public partial class UserHome : Window
    {
        private int userId;
        private string userName;
        private Window mainWindow;
        private readonly Storyboard pageLoadAnimation;

        public UserHome(int userId, Window mainWindow = null)
        {
            InitializeComponent();
            this.userId = userId;
            this.mainWindow = mainWindow;
            
            // Update the logo
            if (LogoText != null)
                LogoText.Text = "LIVrary";
            
            // Update the top-right user display
            if (UserNameDisplay != null)
                UserNameDisplay.Text = "Sir JP Cruz";
            
            if (RoleDisplay != null)
                RoleDisplay.Text = "User";
            
            // Update the main content area greeting and date
            foreach (TextBlock tb in FindVisualChildren<TextBlock>(this))
            {
                // Find the greeting text in the main content area (likely doesn't have a name)
                if (tb.Text == "Arafat!")
                {
                    tb.Text = "Sir JP!";
                }
            }
            
            // Update the time display in the main content - check if it exists first
            TextBlock timeBlock = FindName("TimeDisplay") as TextBlock;
            if (timeBlock != null)
                timeBlock.Text = DateTime.Now.ToString("MMM dd, yyyy | dddd, h:mm tt");
            
            // Update the statistics values explicitly - check if they exist first
            TextBlock booksReadValue = FindName("TotalBooksReadValue") as TextBlock;
            if (booksReadValue != null)
                booksReadValue.Text = "3";
            
            if (FindName("BorrowedBooksValue") is TextBlock borrowed)
                borrowed.Text = "1";
            
            if (FindName("OverdueBooksValue") is TextBlock overdue)
                overdue.Text = "22";
            
            if (FindName("FriendsValue") is TextBlock friends)
                friends.Text = "3";
            
            // Load custom data for books
            LoadBooksData();
            
            // Initialize other components
            if (FindResource("PageLoadAnimation") is Storyboard storyboard)
                pageLoadAnimation = storyboard;
            else
                pageLoadAnimation = new Storyboard();
            
            this.Loaded += UserHome_Loaded;
        }

        private void UserHome_Loaded(object sender, RoutedEventArgs e)
        {
            if (pageLoadAnimation != null)
                pageLoadAnimation.Begin();
        }

        private void MenuToggleButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if MainContent exists before trying to access it
            if (MainContent != null)
            {
                bool isExpanded = MainContent.Width == 250;
                MainContent.Width = isExpanded ? 70 : 250;
            }
        }

        private void LoadUserData()
        {
            try
            {
                UserDAL userDAL = new UserDAL();
                string userName = userDAL.TakeUserNameDAL(userId);
                if (UserNameDisplay != null)
                    UserNameDisplay.Text = userName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}");
            }
        }

        private void LoadBorrowedBooks()
        {
            // Implement if needed
        }

        private void UpdateCartCount()
        {
            // Implement if needed
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
            {
                mainWindow.Show();
            }
            this.Close();
        }

        private void BtnBorrow_Click(object sender, RoutedEventArgs e)
        {
            UserBorrow borrowWindow = new UserBorrow();
            borrowWindow.Show();
        }

        private void BtnTransactions_Click(object sender, RoutedEventArgs e)
        {
            Window transactionWindow = new Window();
            transactionWindow.Content = new UserTransaction();
            transactionWindow.WindowState = WindowState.Maximized;
            transactionWindow.WindowStyle = WindowStyle.None;
            transactionWindow.Show();
            this.Close();
        }

        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            Window cartWindow = new Window();
            cartWindow.Content = new UserTransaction();
            cartWindow.WindowState = WindowState.Maximized;
            cartWindow.WindowStyle = WindowStyle.None;
            cartWindow.Show();
            this.Close();
        }

        private void BtnBrowseMore_Click(object sender, RoutedEventArgs e)
        {
            Window borrowWindow = new Window();
            borrowWindow.Content = new UserBorrow();
            borrowWindow.WindowState = WindowState.Maximized;
            borrowWindow.WindowStyle = WindowStyle.None;
            borrowWindow.Show();
            this.Close();
        }

        private void DashboardBtn_Click(object sender, RoutedEventArgs e)
        {
            // Handle dashboard navigation
            if (pageLoadAnimation != null)
                pageLoadAnimation.Begin();
        }

        private void BooksBtn_Click(object sender, RoutedEventArgs e)
        {
            // Hide all content panels first
            HideAllContentPanels();
            
            // Show the books grid panel
            if (BooksPanel != null)
            {
                BooksPanel.Visibility = Visibility.Visible;
            }
            
            // Hide the catalog view
            if (BooksCatalog != null)
            {
                BooksCatalog.Visibility = Visibility.Collapsed;
            }
            
            // Load or refresh the books data
            LoadBooksData();
            
            // Update button states
            UpdateButtonStates(sender as Button);
        }

        private void MyBooksBtn_Click(object sender, RoutedEventArgs e)
        {
            // Hide all content panels first
            HideAllContentPanels();
            
            // Hide the books grid panel
            if (BooksPanel != null)
            {
                BooksPanel.Visibility = Visibility.Collapsed;
            }
            
            // Show and refresh the catalog view
            if (BooksCatalog != null)
            {
                BooksCatalog.Visibility = Visibility.Visible;
                if (BooksCatalog.GetType().GetMethod("RefreshBooks") != null)
                {
                    // Call RefreshBooks only if the method exists
                    BooksCatalog.GetType().GetMethod("RefreshBooks").Invoke(BooksCatalog, null);
                }
            }
            
            // Update button states
            UpdateButtonStates(sender as Button);
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            // Handle settings navigation
            if (pageLoadAnimation != null)
                pageLoadAnimation.Begin();
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            UserLogin.userId = 0;
            UserLogin.userName = string.Empty;
            new MainWindow().Show();
            this.Close();
        }

        private void LoadCustomLists()
        {
            // This method is no longer needed as we're using LoadBooksData instead
        }

        private void LoadBooksData()
        {
            // Create sample books data with more details
            var books = new List<BookListItem>
            {
                // Programming Books
                new BookListItem { BookId = "CS-1001", Title = "Clean Code", Author = "Robert Martin", Available = true, Genre = "Programming", PublishedYear = 2008, Status = "Available" },
                new BookListItem { BookId = "CS-1002", Title = "Design Patterns", Author = "Gang of Four", Available = false, Genre = "Programming", PublishedYear = 1994, Status = "Borrowed" },
                new BookListItem { BookId = "CS-1003", Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Available = true, Genre = "Programming", PublishedYear = 1999, Status = "Available" },
                new BookListItem { BookId = "CS-1004", Title = "Code Complete", Author = "Steve McConnell", Available = true, Genre = "Programming", PublishedYear = 2004, Status = "Available" },
                new BookListItem { BookId = "CS-1005", Title = "Introduction to Algorithms", Author = "Thomas H. Cormen", Available = true, Genre = "Programming", PublishedYear = 2009, Status = "Available" },
                new BookListItem { BookId = "CS-1006", Title = "JavaScript: The Good Parts", Author = "Douglas Crockford", Available = false, Genre = "Programming", PublishedYear = 2008, Status = "Reserved" },
                
                // Fiction Books
                new BookListItem { BookId = "FIC-1001", Title = "To Kill a Mockingbird", Author = "Harper Lee", Available = true, Genre = "Fiction", PublishedYear = 1960, Status = "Available" },
                new BookListItem { BookId = "FIC-1002", Title = "1984", Author = "George Orwell", Available = true, Genre = "Fiction", PublishedYear = 1949, Status = "Available" },
                new BookListItem { BookId = "FIC-1003", Title = "Pride and Prejudice", Author = "Jane Austen", Available = false, Genre = "Fiction", PublishedYear = 1813, Status = "Borrowed" },
                new BookListItem { BookId = "FIC-1004", Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Available = true, Genre = "Fiction", PublishedYear = 1925, Status = "Available" },
                new BookListItem { BookId = "FIC-1005", Title = "The Catcher in the Rye", Author = "J.D. Salinger", Available = true, Genre = "Fiction", PublishedYear = 1951, Status = "Available" },
                new BookListItem { BookId = "FIC-1006", Title = "Lord of the Rings", Author = "J.R.R. Tolkien", Available = false, Genre = "Fiction", PublishedYear = 1954, Status = "Borrowed" },
                
                // Biography Books
                new BookListItem { BookId = "BIO-1001", Title = "Steve Jobs", Author = "Walter Isaacson", Available = true, Genre = "Biography", PublishedYear = 2011, Status = "Available" },
                new BookListItem { BookId = "BIO-1002", Title = "The Diary of a Young Girl", Author = "Anne Frank", Available = true, Genre = "Biography", PublishedYear = 1947, Status = "Available" },
                new BookListItem { BookId = "BIO-1003", Title = "Long Walk to Freedom", Author = "Nelson Mandela", Available = false, Genre = "Biography", PublishedYear = 1994, Status = "Borrowed" },
            };
            
            // Update the BooksGrid columns if needed
            if (BooksGrid != null)
            {
                if (BooksGrid.Columns.Count == 0)
                {
                    BooksGrid.Columns.Add(new DataGridTextColumn { Header = "Title", Binding = new Binding("Title"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                    BooksGrid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("BookId"), Width = new DataGridLength(100) });
                    BooksGrid.Columns.Add(new DataGridTextColumn { Header = "Author", Binding = new Binding("Author"), Width = new DataGridLength(200) });
                }
                
                // Set the books data
                BooksGrid.ItemsSource = books;
            }
        }

        private void HideAllContentPanels()
        {
            // Hide all panels in the content area
            if (ContentArea != null)
            {
                foreach (var element in ContentArea.Children)
                {
                    if (element is UIElement uiElement)
                    {
                        uiElement.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void UpdateButtonStates(Button clickedButton)
        {
            // Reset all navigation button backgrounds
            foreach (var button in FindVisualChildren<Button>(this))
            {
                if (button.Style == FindResource("NavButton") as Style)
                {
                    button.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3498db");
                }
            }

            // Highlight the clicked button
            if (clickedButton != null)
            {
                clickedButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#2980b9");
            }
        }

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

        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new window to host the HistoryAndFriends control
            Window historyWindow = new Window
            {
                Title = "History & Friends",
                Content = new HistoryAndFriends(),
                Width = 1000,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Background = new SolidColorBrush(Colors.Transparent),
                AllowsTransparency = true
            };
            
            // Show the window as a dialog
            historyWindow.ShowDialog();
        }

        private void NotificationButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new window to host the DeadlineWarnings control
            DeadlineWarnings deadlineWindow = new DeadlineWarnings();
            
            // Show the window as a dialog
            deadlineWindow.ShowDialog();
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            // Get the search text
            string searchText = SearchBox.Text.Trim().ToLower();
            
            if (string.IsNullOrEmpty(searchText))
            {
                // If search is empty, show all books
                LoadBooksData();
                return;
            }
            
            // Create dummy search results
            List<BookInfo> searchResults = new List<BookInfo>();
            
            // Dummy book data
            List<BookInfo> allBooks = new List<BookInfo>
            {
                new BookInfo { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Genre = "Classic", Year = "1925", ImagePath = "/Images/TheGreatGatsby.jpg" },
                new BookInfo { Title = "To Kill a Mockingbird", Author = "Harper Lee", Genre = "Fiction", Year = "1960", ImagePath = "/Images/ToKillaMockingbird.jpg" },
                new BookInfo { Title = "1984", Author = "George Orwell", Genre = "Dystopian", Year = "1949", ImagePath = "/Images/1984.jpg" },
                new BookInfo { Title = "Pride and Prejudice", Author = "Jane Austen", Genre = "Romance", Year = "1813", ImagePath = "/Images/PrideAndPrejudice.jpg" },
                new BookInfo { Title = "The Catcher in the Rye", Author = "J.D. Salinger", Genre = "Fiction", Year = "1951", ImagePath = "/Images/CatcherInTheRye.jpg" },
                new BookInfo { Title = "The Hobbit", Author = "J.R.R. Tolkien", Genre = "Fantasy", Year = "1937", ImagePath = "/Images/TheHobbit.jpg" },
                new BookInfo { Title = "Harry Potter and the Sorcerer's Stone", Author = "J.K. Rowling", Genre = "Fantasy", Year = "1997", ImagePath = "/Images/HarryPotter.jpg" },
                new BookInfo { Title = "The Lord of the Rings", Author = "J.R.R. Tolkien", Genre = "Fantasy", Year = "1954", ImagePath = "/Images/LordOfTheRings.jpg" }
            };
            
            // Filter books based on search text
            foreach (var book in allBooks)
            {
                if (book.Title.ToLower().Contains(searchText) || 
                    book.Author.ToLower().Contains(searchText))
                {
                    searchResults.Add(book);
                }
            }
            
            // Display search results
            DisplaySearchResults(searchResults, searchText);
        }

        private void DisplaySearchResults(List<BookInfo> searchResults, string searchText)
        {
            // Clear existing books
            BooksPanel.Children.Clear();
            
            // Create a section title TextBlock if it doesn't exist
            TextBlock sectionTitle = new TextBlock
            {
                Text = $"Search Results for \"{searchText}\"",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(10, 20, 10, 10)
            };
            BooksPanel.Children.Add(sectionTitle);
            
            if (searchResults.Count == 0)
            {
                // No results found
                TextBlock noResults = new TextBlock
                {
                    Text = "No books found matching your search.",
                    FontSize = 16,
                    Margin = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = Brushes.Gray
                };
                BooksPanel.Children.Add(noResults);
                return;
            }
            
            // Create a WrapPanel for the books
            WrapPanel wrapPanel = new WrapPanel
            {
                Margin = new Thickness(10)
            };
            BooksPanel.Children.Add(wrapPanel);
            
            // Add each book to the panel
            foreach (var book in searchResults)
            {
                // Create book card
                Border bookCard = CreateBookCard(book);
                wrapPanel.Children.Add(bookCard);
            }
        }

        private Border CreateBookCard(BookInfo book)
        {
            // Create the book card
            Border card = new Border
            {
                Width = 180,
                Height = 280,
                Margin = new Thickness(10),
                Background = new SolidColorBrush(Colors.White),
                CornerRadius = new CornerRadius(8),
                Effect = new DropShadowEffect
                {
                    ShadowDepth = 3,
                    BlurRadius = 10,
                    Opacity = 0.3
                }
            };
            
            // Create the content
            StackPanel content = new StackPanel();
            card.Child = content;
            
            // Book cover image
            Image coverImage = new Image
            {
                Height = 180,
                Width = 180,
                Stretch = Stretch.UniformToFill
            };
            
            try
            {
                // Try to load the image
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(book.ImagePath, UriKind.Relative);
                bitmap.EndInit();
                coverImage.Source = bitmap;
            }
            catch
            {
                // If image fails to load, use a placeholder
                coverImage.Source = new BitmapImage(new Uri("/Images/book-placeholder.png", UriKind.Relative));
            }
            
            content.Children.Add(coverImage);
            
            // Book info
            StackPanel infoPanel = new StackPanel
            {
                Margin = new Thickness(10)
            };
            content.Children.Add(infoPanel);
            
            // Title
            TextBlock titleText = new TextBlock
            {
                Text = book.Title,
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                MaxHeight = 40,
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            infoPanel.Children.Add(titleText);
            
            // Author
            TextBlock authorText = new TextBlock
            {
                Text = book.Author,
                Foreground = new SolidColorBrush(Colors.Gray),
                Margin = new Thickness(0, 5, 0, 0)
            };
            infoPanel.Children.Add(authorText);
            
            // Year and Genre
            TextBlock yearGenreText = new TextBlock
            {
                Text = $"{book.Year} • {book.Genre}",
                Foreground = new SolidColorBrush(Colors.Gray),
                FontSize = 11,
                Margin = new Thickness(0, 5, 0, 0)
            };
            infoPanel.Children.Add(yearGenreText);
            
            // Add click event to the card
            card.MouseDown += (sender, e) => BookCard_Click(book);
            
            return card;
        }

        private void BookCard_Click(BookInfo book)
        {
            // Show book details
            MessageBox.Show($"Book: {book.Title}\nAuthor: {book.Author}\nGenre: {book.Genre}\nYear: {book.Year}", 
                           "Book Details", 
                           MessageBoxButton.OK, 
                           MessageBoxImage.Information);
        }

        private void Logo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Create and show the BookSuggestions window
            BookSuggestions suggestionsWindow = new BookSuggestions();
            suggestionsWindow.ShowDialog();
        }
    }

    public class UserListItem
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int BooksIssued { get; set; }
        public string Department { get; set; }
    }

    public class BookListItem
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool Available { get; set; }
        public string Genre { get; set; }
        public int PublishedYear { get; set; }
        public string Status { get; set; }
    }

    public class BookInfo
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Year { get; set; }
        public string ImagePath { get; set; }
    }
}
