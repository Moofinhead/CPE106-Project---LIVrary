using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LibraryMSWF.BL;
using LibraryMSWF.DAL;
using LibraryMSWF.Entity;
using System.ComponentModel;
using System.Windows.Data;

namespace LibraryManagementSystem
{
    public partial class CatalogView : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Book> _books;
        private ICollectionView _booksView;
        private string _searchText = "";
        private string _selectedGenre = "All Genres";
        private string _selectedSortOption = "Title (A-Z)";
        private ObservableCollection<BookCatalogItem> _booksCollection;
        private readonly BookDAL bookDAL;
        private const int MaxBooks = 10000;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<BookCatalogItem> BooksCollection
        {
            get { return _booksCollection; }
            set
            {
                _booksCollection = value;
                OnPropertyChanged(nameof(BooksCollection));
            }
        }

        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                _booksView = CollectionViewSource.GetDefaultView(_books);
                _booksView.Filter = BookFilter;
                ApplySorting();
                OnPropertyChanged(nameof(Books));
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                _booksView?.Refresh();
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public string SelectedGenre
        {
            get { return _selectedGenre; }
            set
            {
                _selectedGenre = value;
                _booksView?.Refresh();
                OnPropertyChanged(nameof(SelectedGenre));
            }
        }

        public string SelectedSortOption
        {
            get { return _selectedSortOption; }
            set
            {
                _selectedSortOption = value;
                ApplySorting();
                OnPropertyChanged(nameof(SelectedSortOption));
            }
        }

        public List<string> GenreOptions { get; set; }
        public List<string> SortOptions { get; set; }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CatalogView()
        {
            InitializeComponent();
            DataContext = this;
            bookDAL = new BookDAL();
            LoadBooks();
            InitializeFilters();

            // Initialize collections
            GenreOptions = new List<string> { "All Genres", "Fiction", "Programming", "Science", "History", "Biography" };
            SortOptions = new List<string> 
            { 
                "Title (A-Z)", 
                "Title (Z-A)", 
                "Author (A-Z)", 
                "Author (Z-A)",
                "Year (Newest)",
                "Year (Oldest)"
            };
            Books = new ObservableCollection<Book>();
            BooksCollection = new ObservableCollection<BookCatalogItem>();
        }

        private async void LoadBooks()
        {
            try
            {
                var ds = bookDAL.GetAllBooksDAL();
                Books = new ObservableCollection<Book>();

                if (ds?.Tables[0]?.Rows.Count > MaxBooks)
                {
                    MessageBox.Show("Warning: The catalog contains more than 10,000 books. Performance may be affected.");
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Books.Add(new Book
                    {
                        BookId = Convert.ToInt32(dr["BookId"]),
                        BookName = Convert.ToString(dr["Title"]),
                        BookAuthor = Convert.ToString(dr["Author"]),
                        BookISBN = Convert.ToString(dr["ISBN"]),
                        BookCopies = Convert.ToInt32(dr["Quantity"]),
                        BookPrice = Convert.ToInt32(dr["Price"]),
                        BookImage = Convert.ToString(dr["BookImage"]),
                        PublicationYear = dr["PublicationYear"] != DBNull.Value ? Convert.ToInt32(dr["PublicationYear"]) : 0,
                        Genre = dr["Genre"] != DBNull.Value ? Convert.ToString(dr["Genre"]) : "Uncategorized",
                        Synopsis = dr["Synopsis"] != DBNull.Value ? Convert.ToString(dr["Synopsis"]) : "No synopsis available.",
                        PdfPath = dr["PdfPath"] != DBNull.Value ? Convert.ToString(dr["PdfPath"]) : null,
                        PdfSize = dr["PdfSize"] != DBNull.Value ? Convert.ToInt64(dr["PdfSize"]) : 0
                    });
                }

                BooksPanel.ItemsSource = Books;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}");
            }
        }

        private void InitializeFilters()
        {
            // Populate genre filter
            var genres = Books.Select(b => b.Genre).Distinct().OrderBy(g => g);
            GenreFilter.ItemsSource = new[] { "All Genres" }.Concat(genres);
            GenreFilter.SelectedIndex = 0;

            // Populate year filter
            var years = Books.Select(b => b.PublicationYear)
                              .Where(y => y > 0)
                              .Distinct()
                              .OrderByDescending(y => y);
            YearFilter.ItemsSource = new[] { "All Years" }.Concat(years.Select(y => y.ToString()));
            YearFilter.SelectedIndex = 0;
        }

        private void ApplyFilters()
        {
            var filteredBooks = Books.AsEnumerable();

            // Apply genre filter
            if (GenreFilter.SelectedItem.ToString() != "All Genres")
            {
                filteredBooks = filteredBooks.Where(b => b.Genre == GenreFilter.SelectedItem.ToString());
            }

            // Apply year filter
            if (YearFilter.SelectedItem.ToString() != "All Years")
            {
                int year = int.Parse(YearFilter.SelectedItem.ToString());
                filteredBooks = filteredBooks.Where(b => b.PublicationYear == year);
            }

            // Apply search
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                string searchTerm = SearchBox.Text.ToLower();
                filteredBooks = filteredBooks.Where(b => 
                    b.BookName.ToLower().Contains(searchTerm) ||
                    b.BookAuthor.ToLower().Contains(searchTerm) ||
                    b.Synopsis.ToLower().Contains(searchTerm));
            }

            BooksPanel.ItemsSource = new ObservableCollection<Book>(filteredBooks);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void GenreFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) ApplyFilters();
        }

        private void YearFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) ApplyFilters();
        }

        private void BorrowButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var book = button.DataContext as Book;
            
            if (book != null)
            {
                UserRequestBL userRequestBL = new UserRequestBL();
                bool isDone = userRequestBL.AddRequestBL(book.BookId, book.BookName, UserLogin.userId);
                
                if (isDone)
                {
                    MessageBox.Show("Book requested successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to request book. Please try again.");
                }
            }
        }

        private void BookCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border?.DataContext is Book book)
            {
                var bookView = new BookView(book);
                bookView.ShowDialog();
            }
        }

        private void ApplySorting()
        {
            // Implementation of ApplySorting method
        }

        private bool BookFilter(object item)
        {
            // Implementation of BookFilter method
            return true; // Placeholder return, actual implementation needed
        }
    }
} 