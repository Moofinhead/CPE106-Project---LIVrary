using System;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using LibraryMSWF.Entity;
using LibraryMSWF.BL;
using Microsoft.Web.WebView2.Core;
using LibraryManagementSystem.PdfViewer;

namespace LibraryManagementSystem
{
    public partial class BookView : Window
    {
        private readonly Book _book;
        private const long MaxPdfSize = 52428800; // 50MB in bytes
        private readonly string _baseDirectory;
        private readonly CustomPdfViewer _pdfViewer;

        public BookView(Book book)
        {
            InitializeComponent();
            
            // Debug before assigning _book
            MessageBox.Show($"Incoming book details:\n" +
                           $"Book ID: {book?.BookId}\n" +
                           $"Book Name: {book?.BookName}\n" +
                           $"Raw PDF Path: '{book?.PdfPath}'\n" +  // Added quotes to see if it's empty or whitespace
                           $"PDF Size: {book?.PdfSize}");

            _book = book;
            
            // Debug after assigning _book
            MessageBox.Show($"_book details:\n" +
                           $"Book ID: {_book?.BookId}\n" +
                           $"Book Name: {_book?.BookName}\n" +
                           $"Raw PDF Path: '{_book?.PdfPath}'\n" +
                           $"PDF Size: {_book?.PdfSize}");

            _baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Books");
            _pdfViewer = new CustomPdfViewer();
            PdfViewerContainer.Content = _pdfViewer;
            LoadBookDetails();
            InitializePdfViewer();
        }

        private void LoadBookDetails()
        {
            BookTitle.Text = _book.BookName;
            BookAuthor.Text = _book.BookAuthor;
            BookISBN.Text = _book.BookISBN;
            PublicationYear.Text = _book.PublicationYear.ToString();
            Genre.Text = _book.Genre;
            Availability.Text = _book.IsAvailable ? "Available" : "Not Available";
            Synopsis.Text = _book.Synopsis;
            
            if (!string.IsNullOrEmpty(_book.BookImage))
            {
                try
                {
                    BookCover.Source = new System.Windows.Media.Imaging.BitmapImage(
                        new Uri(_book.BookImage, UriKind.RelativeOrAbsolute));
                }
                catch (Exception)
                {
                    // Use default image if book cover fails to load
                    BookCover.Source = new System.Windows.Media.Imaging.BitmapImage(
                        new Uri("/Images/default-book.png", UriKind.RelativeOrAbsolute));
                }
            }

            BorrowButton.IsEnabled = _book.IsAvailable;
        }

        private async void InitializePdfViewer()
        {
            try
            {
                if (string.IsNullOrEmpty(_book.PdfPath))
                {
                    ShowPdfError("PDF is not available for this book.");
                    return;
                }

                // Debug logging
                MessageBox.Show($"PDF Path from database: {_book.PdfPath}");
                
                // Ensure we're using the correct base directory and combining paths properly
                string fullPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _book.PdfPath));
                MessageBox.Show($"Full PDF Path: {fullPath}");

                if (!File.Exists(fullPath))
                {
                    ShowPdfError($"PDF file not found at: {fullPath}");
                    return;
                }

                var fileInfo = new FileInfo(fullPath);
                if (fileInfo.Length > MaxPdfSize)
                {
                    ShowPdfError("PDF file is too large to display (max 50MB).");
                    return;
                }

                await Task.Run(async () => 
                {
                    await Dispatcher.InvokeAsync(async () => 
                    {
                        try
                        {
                            await _pdfViewer.LoadPdfAsync(fullPath);
                        }
                        catch (Exception ex)
                        {
                            ShowPdfError($"Error loading PDF: {ex.Message}");
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                ShowPdfError($"Error loading PDF: {ex.Message}");
                MessageBox.Show($"Exception details: {ex}");
            }
        }

        private void ShowPdfError(string message)
        {
            PdfViewerContainer.Visibility = Visibility.Collapsed;
            PdfErrorMessage.Text = message;
            PdfErrorMessage.Visibility = Visibility.Visible;
        }

        private void BorrowButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserRequestBL userRequestBL = new UserRequestBL();
                bool isDone = userRequestBL.AddRequestBL(_book.BookId, _book.BookName, UserLogin.userId);
                
                if (isDone)
                {
                    MessageBox.Show("Book requested successfully!");
                    BorrowButton.IsEnabled = false;
                    Availability.Text = "Not Available";
                }
                else
                {
                    MessageBox.Show("Failed to request book. Please try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error requesting book: {ex.Message}");
            }
        }
    }
} 