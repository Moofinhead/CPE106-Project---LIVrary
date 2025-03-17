using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LibraryManagementSystem
{
    public partial class BookSuggestions : Window
    {
        public BookSuggestions()
        {
            InitializeComponent();
            
            // Load dummy data
            LoadDummyData();
            
            // Add close button
            AddCloseButton();
        }
        
        private void LoadDummyData()
        {
            // Load friend-based recommendations
            LoadFriendRecommendations();
            
            // Load genre-based recommendations
            LoadGenreRecommendations();
        }
        
        private void LoadFriendRecommendations()
        {
            // Dummy data for friend recommendations
            List<FriendRecommendation> friendBooks = new List<FriendRecommendation>
            {
                new FriendRecommendation
                {
                    Title = "1984",
                    Author = "George Orwell",
                    FriendsReading = 3,
                    CoverImage = "/Images/1984.jpg"
                },
                new FriendRecommendation
                {
                    Title = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    FriendsReading = 2,
                    CoverImage = "/Images/TheHobbit.jpg"
                },
                new FriendRecommendation
                {
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    FriendsReading = 1,
                    CoverImage = "/Images/PrideAndPrejudice.jpg"
                },
                new FriendRecommendation
                {
                    Title = "The Alchemist",
                    Author = "Paulo Coelho",
                    FriendsReading = 4,
                    CoverImage = "/Images/TheAlchemist.jpg"
                }
            };
            
            // Find the WrapPanel for friend recommendations
            WrapPanel friendPanel = FindName("FriendRecommendationsPanel") as WrapPanel;
            if (friendPanel == null)
            {
                // If the panel doesn't exist in XAML, find the parent container and create it
                Border friendBorder = FindVisualChildren<Border>(this)
                    .FirstOrDefault(b => b.Child is StackPanel sp && 
                                        sp.Children.OfType<TextBlock>().Any(tb => 
                                            tb.Text == "Because your friends are reading..."));
                
                if (friendBorder != null && friendBorder.Child is StackPanel stackPanel)
                {
                    // Find or create the WrapPanel
                    friendPanel = stackPanel.Children.OfType<WrapPanel>().FirstOrDefault();
                    if (friendPanel == null)
                    {
                        friendPanel = new WrapPanel();
                        stackPanel.Children.Add(friendPanel);
                    }
                    else
                    {
                        friendPanel.Children.Clear();
                    }
                    
                    // Add book cards to the panel
                    foreach (var book in friendBooks)
                    {
                        Border bookCard = CreateFriendBookCard(book);
                        friendPanel.Children.Add(bookCard);
                    }
                }
            }
            else
            {
                // If the panel exists, clear it and add book cards
                friendPanel.Children.Clear();
                foreach (var book in friendBooks)
                {
                    Border bookCard = CreateFriendBookCard(book);
                    friendPanel.Children.Add(bookCard);
                }
            }
        }
        
        private void LoadGenreRecommendations()
        {
            // Dummy data for genre recommendations
            List<GenreRecommendation> genreBooks = new List<GenreRecommendation>
            {
                new GenreRecommendation
                {
                    Title = "Dune",
                    Author = "Frank Herbert",
                    MatchPercentage = 98,
                    CoverImage = "/Images/Dune.jpg"
                },
                new GenreRecommendation
                {
                    Title = "Foundation",
                    Author = "Isaac Asimov",
                    MatchPercentage = 95,
                    CoverImage = "/Images/Foundation.jpg"
                },
                new GenreRecommendation
                {
                    Title = "Neuromancer",
                    Author = "William Gibson",
                    MatchPercentage = 92,
                    CoverImage = "/Images/Neuromancer.jpg"
                },
                new GenreRecommendation
                {
                    Title = "Hyperion",
                    Author = "Dan Simmons",
                    MatchPercentage = 90,
                    CoverImage = "/Images/Hyperion.jpg"
                }
            };
            
            // Find the WrapPanel for genre recommendations
            WrapPanel genrePanel = FindName("GenreRecommendationsPanel") as WrapPanel;
            if (genrePanel == null)
            {
                // If the panel doesn't exist in XAML, find the parent container and create it
                Border genreBorder = FindVisualChildren<Border>(this)
                    .FirstOrDefault(b => b.Child is StackPanel sp && 
                                        sp.Children.OfType<TextBlock>().Any(tb => 
                                            tb.Text == "Because you like Science Fiction..."));
                
                if (genreBorder != null && genreBorder.Child is StackPanel stackPanel)
                {
                    // Find or create the WrapPanel
                    genrePanel = stackPanel.Children.OfType<WrapPanel>().FirstOrDefault();
                    if (genrePanel == null)
                    {
                        genrePanel = new WrapPanel();
                        stackPanel.Children.Add(genrePanel);
                    }
                    else
                    {
                        genrePanel.Children.Clear();
                    }
                    
                    // Add book cards to the panel
                    foreach (var book in genreBooks)
                    {
                        Border bookCard = CreateGenreBookCard(book);
                        genrePanel.Children.Add(bookCard);
                    }
                }
            }
            else
            {
                // If the panel exists, clear it and add book cards
                genrePanel.Children.Clear();
                foreach (var book in genreBooks)
                {
                    Border bookCard = CreateGenreBookCard(book);
                    genrePanel.Children.Add(bookCard);
                }
            }
        }
        
        private Border CreateFriendBookCard(FriendRecommendation book)
        {
            // Create the book card
            Border card = new Border
            {
                Width = 200,
                Height = 300,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5F5F5")),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(10)
            };
            
            // Create the content
            StackPanel content = new StackPanel { Margin = new Thickness(15) };
            card.Child = content;
            
            // Book cover
            Border coverBorder = new Border
            {
                Width = 120,
                Height = 180,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDDDDD")),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 0, 0, 10)
            };
            
            try
            {
                // Try to load the image
                Image coverImage = new Image
                {
                    Stretch = Stretch.UniformToFill
                };
                
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(book.CoverImage, UriKind.Relative);
                bitmap.EndInit();
                coverImage.Source = bitmap;
                
                coverBorder.Child = coverImage;
            }
            catch
            {
                // If image fails to load, leave the placeholder
            }
            
            content.Children.Add(coverBorder);
            
            // Book title
            TextBlock titleText = new TextBlock
            {
                Text = book.Title,
                FontWeight = FontWeights.SemiBold,
                TextWrapping = TextWrapping.Wrap
            };
            content.Children.Add(titleText);
            
            // Book author
            TextBlock authorText = new TextBlock
            {
                Text = book.Author,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666")),
                TextWrapping = TextWrapping.Wrap
            };
            content.Children.Add(authorText);
            
            // Friends reading
            TextBlock friendsText = new TextBlock
            {
                Text = $"{book.FriendsReading} friends reading",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498db")),
                Margin = new Thickness(0, 5, 0, 0)
            };
            content.Children.Add(friendsText);
            
            // Add click event
            card.MouseDown += (sender, e) => BookCard_Click(book.Title, book.Author);
            card.Cursor = Cursors.Hand;
            
            return card;
        }
        
        private Border CreateGenreBookCard(GenreRecommendation book)
        {
            // Create the book card
            Border card = new Border
            {
                Width = 200,
                Height = 300,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5F5F5")),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(10)
            };
            
            // Create the content
            StackPanel content = new StackPanel { Margin = new Thickness(15) };
            card.Child = content;
            
            // Book cover
            Border coverBorder = new Border
            {
                Width = 120,
                Height = 180,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDDDDD")),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 0, 0, 10)
            };
            
            try
            {
                // Try to load the image
                Image coverImage = new Image
                {
                    Stretch = Stretch.UniformToFill
                };
                
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(book.CoverImage, UriKind.Relative);
                bitmap.EndInit();
                coverImage.Source = bitmap;
                
                coverBorder.Child = coverImage;
            }
            catch
            {
                // If image fails to load, leave the placeholder
            }
            
            content.Children.Add(coverBorder);
            
            // Book title
            TextBlock titleText = new TextBlock
            {
                Text = book.Title,
                FontWeight = FontWeights.SemiBold,
                TextWrapping = TextWrapping.Wrap
            };
            content.Children.Add(titleText);
            
            // Book author
            TextBlock authorText = new TextBlock
            {
                Text = book.Author,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666")),
                TextWrapping = TextWrapping.Wrap
            };
            content.Children.Add(authorText);
            
            // Match percentage
            TextBlock matchText = new TextBlock
            {
                Text = $"{book.MatchPercentage}% Match",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ecc71")),
                Margin = new Thickness(0, 5, 0, 0)
            };
            content.Children.Add(matchText);
            
            // Add click event
            card.MouseDown += (sender, e) => BookCard_Click(book.Title, book.Author);
            card.Cursor = Cursors.Hand;
            
            return card;
        }
        
        private void BookCard_Click(string title, string author)
        {
            MessageBox.Show($"You selected '{title}' by {author}.\n\nThis book would be added to your reading list or borrowing queue in a full implementation.", 
                           "Book Selected", 
                           MessageBoxButton.OK, 
                           MessageBoxImage.Information);
        }
        
        private void AddCloseButton()
        {
            // Add a close button to the top bar
            Grid topBar = FindVisualChildren<Grid>(this).FirstOrDefault(g => g.Background != null && 
                                                                           g.Background.ToString().Contains("#FFFFFFFF"));
            
            if (topBar != null)
            {
                Button closeButton = new Button
                {
                    Content = "✕",
                    FontSize = 20,
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(15, 0),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    Cursor = Cursors.Hand,
                    Margin = new Thickness(0, 0, 20, 0)
                };
                
                closeButton.Click += (sender, e) => this.Close();
                topBar.Children.Add(closeButton);
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
    public class FriendRecommendation
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int FriendsReading { get; set; }
        public string CoverImage { get; set; }
    }
    
    public class GenreRecommendation
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int MatchPercentage { get; set; }
        public string CoverImage { get; set; }
    }
} 