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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using LibraryMSWF.BL;
using LibraryMSWF.Entity;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace LibraryManagementSystem
{
    public partial class UserBorrow : Window
    {
        public int userId;
        private BookBL bookBL;
        private UserRequestBL requestBL;

        public UserBorrow()
        {
            InitializeComponent();
            bookBL = new BookBL();
            requestBL = new UserRequestBL();
            userId = UserLogin.userId;
            InitializeUserBorrow();
        }

        public void InitializeUserBorrow()
        {
            try
            {
                DataSet ds = bookBL.GetAllBooksBL();
                userId = UserLogin.userId;

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ObservableCollection<Book> lst = new ObservableCollection<Book>();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lst.Add(new Book
                            {
                                BookName = Convert.ToString(dr["BookName"]),
                                BookId = Convert.ToInt32(dr["BookId"]),
                                BookAuthor = Convert.ToString(dr["BookAuthor"]),
                                BookISBN = Convert.ToString(dr["BookISBN"]),
                                BookCopies = Convert.ToInt32(dr["BookCopies"]),
                                BookPrice = Convert.ToInt32(dr["BookPrice"])
                            });
                        }
                        dgBorrow.ItemsSource = lst;
                    }
                    else
                    {
                        MessageBox.Show("No books found in the database.");
                    }
                }
                else
                {
                    MessageBox.Show("Error retrieving books from the database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void BtnRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Book book = dgBorrow.SelectedItem as Book;
                if (book != null)
                {
                    if (!book.IsAvailable)
                    {
                        MessageBox.Show("Sorry, this book is not available for borrowing.");
                        return;
                    }

                    int BookCopy = book.BookCopies - 1;
                    string isDone1 = bookBL.UpdateBookBL(book.BookId, book.BookName,
                        book.BookAuthor, book.BookISBN, book.BookPrice, BookCopy);

                    bool isDone2 = requestBL.AddRequestBL(book.BookId, book.BookName, userId);

                    if (isDone1.ToLower() == "true" || isDone2)
                    {
                        MessageBox.Show("Book requested successfully!");
                        InitializeUserBorrow();
                    }
                    else
                    {
                        string errorMessage = !isDone2 ? "Failed to add request. " : "";
                        errorMessage += isDone1.ToLower() != "true" ? isDone1 : "";
                        MessageBox.Show($"Failed to request book. {errorMessage}Please try again.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a book to borrow.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error requesting book: {ex.Message}");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
