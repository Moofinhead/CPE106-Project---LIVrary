using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LibraryMSWF.DAL
{
    public class AdminDAL
    {
        private readonly string connectionString = "Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True";

        //CHECK THE ADMIN LOGIN CREDENTIALS =>DAL
        public bool AdminLoginDAL(string adminEmail, string adminPass)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Admins WHERE Email = @email AND Password = @pwd", conn);
                    cmd.Parameters.Add(new SqlParameter("@email", adminEmail));
                    cmd.Parameters.Add(new SqlParameter("@pwd", adminPass));
                    conn.Open();
                    int rowAffected = (int)cmd.ExecuteScalar();
                    return rowAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }
        
        // Get all books for admin view
        public DataTable GetAllBooksDAL()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Books", conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    conn.Open();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {
                // For demo purposes, return dummy data if database fails
                DataTable dummyData = new DataTable();
                dummyData.Columns.Add("BookId", typeof(int));
                dummyData.Columns.Add("Title", typeof(string));
                dummyData.Columns.Add("Author", typeof(string));
                dummyData.Columns.Add("ISBN", typeof(string));
                dummyData.Columns.Add("Quantity", typeof(int));
                dummyData.Columns.Add("Available", typeof(int));
                dummyData.Columns.Add("Price", typeof(decimal));
                dummyData.Columns.Add("BookImage", typeof(string));
                dummyData.Columns.Add("PublicationYear", typeof(string));
                dummyData.Columns.Add("Genre", typeof(string));
                dummyData.Columns.Add("Synopsis", typeof(string));
                dummyData.Columns.Add("PdfPath", typeof(string));
                dummyData.Columns.Add("PdfSize", typeof(string));
                
                // Add sample data
                dummyData.Rows.Add(1, "The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", 2, 5, 15.00, "/Images/TheGreatGatsby.jpg", "NULL", "NULL", "NULL", "Books/TheGreatGatsby.pdf", "NULL");
                dummyData.Rows.Add(2, "To Kill a Mockingbird", "Harper Lee", "9780446310789", 1, 3, 15.00, "/Images/ToKillaMockingbird.jpg", "NULL", "NULL", "NULL", "Books/ToKillaMockingbird.pdf", "NULL");
                dummyData.Rows.Add(3, "1984", "George Orwell", "9780451524935", 4, 4, 14.67, "/Images/1984.jpg", "NULL", "NULL", "NULL", "Books/1984.pdf", "NULL");
                
                return dummyData;
            }
        }
        
        // Get all users for admin view
        public DataTable GetAllUsersDAL()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Users", conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    conn.Open();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {
                // For demo purposes, return dummy data if database fails
                DataTable dummyData = new DataTable();
                dummyData.Columns.Add("UserId", typeof(int));
                dummyData.Columns.Add("Name", typeof(string));
                dummyData.Columns.Add("Email", typeof(string));
                dummyData.Columns.Add("Password", typeof(string));
                dummyData.Columns.Add("Role", typeof(string));
                
                // Add sample data
                dummyData.Rows.Add(1, "John Paul T. Cruz", "jptcruz@gmail.com", "password123", "User");
                dummyData.Rows.Add(2, "Maria Garcia", "maria@example.com", "pass456", "User");
                dummyData.Rows.Add(3, "David Chen", "david@example.com", "secure789", "User");
                
                return dummyData;
            }
        }
        
        // Get all borrow requests
        public DataTable GetAllRequestsDAL()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM BorrowRequests WHERE Status = 'Pending'", conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    conn.Open();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {
                // For demo purposes, return dummy data if database fails
                DataTable dummyData = new DataTable();
                dummyData.Columns.Add("RequestId", typeof(int));
                dummyData.Columns.Add("UserId", typeof(int));
                dummyData.Columns.Add("UserName", typeof(string));
                dummyData.Columns.Add("BookId", typeof(int));
                dummyData.Columns.Add("BookTitle", typeof(string));
                dummyData.Columns.Add("RequestDate", typeof(DateTime));
                dummyData.Columns.Add("Status", typeof(string));
                
                // Add sample data
                dummyData.Rows.Add(1, 1, "John Paul T. Cruz", 2, "To Kill a Mockingbird", DateTime.Now.AddDays(-2), "Pending");
                dummyData.Rows.Add(2, 3, "David Chen", 1, "The Great Gatsby", DateTime.Now.AddDays(-1), "Pending");
                
                return dummyData;
            }
        }
        
        // Get all accepted requests
        public DataTable GetAllAcceptedRequestsDAL()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM BorrowRequests WHERE Status = 'Accepted'", conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    conn.Open();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {
                // For demo purposes, return dummy data if database fails
                DataTable dummyData = new DataTable();
                dummyData.Columns.Add("RequestId", typeof(int));
                dummyData.Columns.Add("UserId", typeof(int));
                dummyData.Columns.Add("UserName", typeof(string));
                dummyData.Columns.Add("BookId", typeof(int));
                dummyData.Columns.Add("BookTitle", typeof(string));
                dummyData.Columns.Add("RequestDate", typeof(DateTime));
                dummyData.Columns.Add("Status", typeof(string));
                
                // Add sample data
                dummyData.Rows.Add(1, 1, "John Paul T. Cruz", 2, "To Kill a Mockingbird", DateTime.Now.AddDays(-2), "Accepted");
                dummyData.Rows.Add(2, 3, "David Chen", 1, "The Great Gatsby", DateTime.Now.AddDays(-1), "Accepted");
                
                return dummyData;
            }
        }
    }
}
