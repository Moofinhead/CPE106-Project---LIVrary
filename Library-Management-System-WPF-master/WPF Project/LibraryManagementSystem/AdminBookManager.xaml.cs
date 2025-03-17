using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LibraryMSWF.Entity;
using LibraryMSWF.DAL;
using LibraryMSWF.BL;
using LibraryManagementSystem.PdfViewer;

namespace LibraryManagementSystem
{
    public partial class AdminBookManager : Window
    {
        private readonly BookDAL _bookDAL;
        private ObservableCollection<Book> _books;

        public AdminBookManager()
        {
            InitializeComponent();
            _bookDAL = new BookDAL();
            LoadBooks();
        }

        private void LoadBooks()
        {
            try
            {
                var ds = _bookDAL.GetAllBooksDAL();
                _books = new ObservableCollection<Book>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var book = new Book
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
                    };
                    
                    // Add debug logging
                    MessageBox.Show($"Loading book: {book.BookId}\n" +
                                   $"PDF Path from DB: {dr["PdfPath"]}\n" +
                                   $"PDF Path in Book: {book.PdfPath}");
                    
                    _books.Add(book);
                }

                BooksGrid.ItemsSource = _books;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}");
            }
        }

        private void ManagePdf_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var book = button.DataContext as Book;

            if (book != null)
            {
                var pdfManager = new PdfManagerDialog(book);
                pdfManager.Owner = this;
                
                if (pdfManager.ShowDialog() == true)
                {
                    LoadBooks(); // Refresh the grid
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();
            
            var filteredBooks = _books.Where(b => 
                b.BookName.ToLower().Contains(searchText) ||
                b.BookAuthor.ToLower().Contains(searchText) ||
                b.BookISBN.ToLower().Contains(searchText)
            );

            BooksGrid.ItemsSource = filteredBooks;
        }

        private void AddNewBook_Click(object sender, RoutedEventArgs e)
        {
            // Implement new book addition logic
            MessageBox.Show("Add New Book functionality to be implemented");
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var book = button.DataContext as Book;

            if (book != null)
            {
                // Implement edit book logic
                MessageBox.Show($"Edit functionality for book: {book.BookName}");
            }
        }
    }
} 