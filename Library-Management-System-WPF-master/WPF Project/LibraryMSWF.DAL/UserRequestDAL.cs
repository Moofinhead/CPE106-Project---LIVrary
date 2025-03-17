using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using LibraryMSWF.Entity;
using System.Windows;

namespace LibraryMSWF.DAL
{
    public class UserRequestDAL
    {
        private readonly string connectionString = "Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True";

        //ADD THE BOOK REQUEST INTO REQUEST TABLE =>DAL
        public bool AddRequestDAL(int bookId, string bookName, int userId, string userName)
        {
            SqlConnection conn = new SqlConnection("Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True");
            SqlCommand cmd = new SqlCommand("AddRequest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            
            try
            {
                conn.Open();
                int rowAffected = cmd.ExecuteNonQuery();
                return rowAffected > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        //RETURN THE PERTICULAR USER REQUESTED BOOKS FROM REQUEST TABLE  =>DAL
        public DataSet GetAllRequestUserDAL(int userId)
        {
            SqlConnection conn = new SqlConnection("Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True");
            SqlDataAdapter da = new SqlDataAdapter("GetAllRequestUser "+userId+"", conn);
            DataSet ds = new DataSet("UsersRequests");
            da.Fill(ds);
            return ds;
        }
        //RETURN THE COMPLETE REQUESTED BOOKS FROM REQUEST TABLE  =>DAL
        public DataSet GetAllRequestDAL()
        {
            SqlConnection conn = new SqlConnection("Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True");
            SqlDataAdapter da = new SqlDataAdapter("GetAllRequest", conn);
            DataSet ds = new DataSet("Requests");
            da.Fill(ds);
            return ds;
        }
        //DELETE THE BOOK REQUEST FROM REQUEST TABLE =>DAL
        public bool DeleteRequestDAL(int bookId, int userId)
        {
            SqlConnection conn = new SqlConnection("Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True");
            SqlCommand cmd = new SqlCommand("DeleteRequest @bId, @uId", conn);
            cmd.Parameters.Add(new SqlParameter("@bId", bookId));
            cmd.Parameters.Add(new SqlParameter("@uId", userId));
            conn.Open();
            int rowAffected = cmd.ExecuteNonQuery();
            conn.Close();
            if (rowAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ObservableCollection<Book> GetBorrowedBooks(int userId)
        {
            ObservableCollection<Book> borrowedBooks = new ObservableCollection<Book>();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT b.BookId, b.Title as BookName, b.Author as BookAuthor, b.BookImage
                    FROM BookTable b
                    INNER JOIN UserRequest ur ON b.BookId = ur.BookId
                    WHERE ur.UserId = @UserId AND ur.Status = 'Approved'", conn);
                    
                cmd.Parameters.AddWithValue("@UserId", userId);
                
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        borrowedBooks.Add(new Book
                        {
                            BookId = reader.GetInt32(0),
                            BookName = reader.GetString(1),
                            BookAuthor = reader.GetString(2),
                            BookImage = reader.GetString(3)
                        });
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error loading borrowed books.");
                }
            }
            
            return borrowedBooks;
        }
        public DataSet GetBorrowedBooksDAL(int userId)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(@"
                        SELECT b.BookId, b.BookName, b.BookAuthor, b.BookImage 
                        FROM BookTable b 
                        INNER JOIN UserRequest ur ON b.BookId = ur.BookId 
                        WHERE ur.UserId = @UserId AND ur.Status = 'Approved'", conn);
                    da.SelectCommand.Parameters.AddWithValue("@UserId", userId);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public DataSet GetPendingRequestsCountDAL(int userId)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT COUNT(*) FROM UserRequest WHERE UserId = @UserId AND Status = 'Pending'", conn);
                    da.SelectCommand.Parameters.AddWithValue("@UserId", userId);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
