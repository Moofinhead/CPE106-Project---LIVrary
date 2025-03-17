using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using LibraryMSWF.Entity;
using System.IO;

namespace LibraryMSWF.DAL
{
    public class BookDAL
    {
        private readonly string connectionString = "Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True";
        private const string BookStoragePath = "Books";

        public BookDAL()
        {
            // Ensure Books directory exists
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BookStoragePath);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        //RETURN THE COMPLETE BOOKS FROM BOOK TABLE =>DAL
        public DataSet GetAllBooksDAL()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                        BookId,
                        Title,
                        Author,
                        ISBN,
                        Price,
                        Quantity,
                        Available,
                        BookImage,
                        PublicationYear,
                        Genre,
                        Synopsis,
                        PdfPath,
                        PdfSize
                    FROM BookTable";
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Books");
                    return ds;
                }
            }
            catch (SqlException ex)
            {
                // Log or display the SQL error message
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw; // Throw the exception to see the actual error
            }
            catch (Exception ex)
            {
                // Log or display the general error message
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Throw the exception to see the actual error
            }
        }
        //ADD BOOK INTO BOOK TABLE => DAL
        public bool AddBookDAL(string bookName, string bookAuthor, string bookISBN, double bookPrice, int bookCopies)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddBook", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", bookName);
                cmd.Parameters.AddWithValue("@Author", bookAuthor);
                cmd.Parameters.AddWithValue("@ISBN", bookISBN);
                cmd.Parameters.AddWithValue("@Price", (int)bookPrice);
                cmd.Parameters.AddWithValue("@Quantity", bookCopies);
                cmd.Parameters.AddWithValue("@Available", bookCopies);
                cmd.Parameters.AddWithValue("@BookImage", $"/Images/{bookName.Replace(" ", "")}.png");
                
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
            }
        }
        //UPDATE THE BOOK FROM BOOK TABLE =>DAL
        public bool UpdateBookDAL(int bookId, string bookName, string bookAuthor, string bookISBN, double bookPrice, int bookCopies)
        {
            SqlConnection conn = new SqlConnection("Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True");
            SqlCommand cmd = new SqlCommand("UpdateBook @id, @name, @author,@isbn,@price,@copy", conn);
            cmd.Parameters.Add(new SqlParameter("@id", bookId));
            cmd.Parameters.Add(new SqlParameter("@name", bookName));
            cmd.Parameters.Add(new SqlParameter("@author", bookAuthor));
            cmd.Parameters.Add(new SqlParameter("@isbn", bookISBN));
            cmd.Parameters.Add(new SqlParameter("@price", bookPrice));
            cmd.Parameters.Add(new SqlParameter("@copy", bookCopies));
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
        //DELETE THE BOOK FROM BOOK TABLE =>DAL
        public bool DeleteBookDAL(int bookId)
        {
            SqlConnection conn = new SqlConnection("Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True");
            SqlCommand cmd = new SqlCommand("DeleteBook @id", conn);
            cmd.Parameters.Add(new SqlParameter("@id", bookId));
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
        //INCREMENT THE BOOK COPIES OF A BOOK BY 1 =>DAL
        public bool IncBookCopyDAL(int bookId)
        {
            SqlConnection conn = new SqlConnection("Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True");
            SqlCommand cmd = new SqlCommand("IncBookCopy @id", conn);
            cmd.Parameters.Add(new SqlParameter("@id", bookId));
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

        // Add method to upload PDF
        public bool UploadPdf(int bookId, string pdfPath)
        {
            try
            {
                if (!File.Exists(pdfPath))
                    return false;

                var fileInfo = new FileInfo(pdfPath);
                string fileName = $"{bookId}_{Path.GetFileName(pdfPath)}";
                string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BookStoragePath, fileName);

                File.Copy(pdfPath, destinationPath, true);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE BookTable SET PdfPath = @PdfPath, PdfSize = @PdfSize WHERE BookId = @BookId", 
                        conn);
                    
                    cmd.Parameters.AddWithValue("@PdfPath", destinationPath);
                    cmd.Parameters.AddWithValue("@PdfSize", fileInfo.Length);
                    cmd.Parameters.AddWithValue("@BookId", bookId);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdatePdfSize(int bookId, string pdfPath)
        {
            try
            {
                if (!File.Exists(pdfPath))
                    return false;

                var fileInfo = new FileInfo(pdfPath);
                long fileSize = fileInfo.Length;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE BookTable SET PdfSize = @PdfSize WHERE BookId = @BookId", 
                        conn);
                    
                    cmd.Parameters.AddWithValue("@PdfSize", fileSize);
                    cmd.Parameters.AddWithValue("@BookId", bookId);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataSet GetBookById(int bookId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Debug the query
                string query = @"SELECT 
                    BookId,
                    Title,
                    Author,
                    ISBN,
                    Price,
                    Quantity,
                    BookImage,
                    PublicationYear,
                    Genre,
                    Synopsis,
                    PdfPath,    -- Make sure these columns are included
                    PdfSize     -- in the single book query
                FROM BookTable 
                WHERE BookId = @BookId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BookId", bookId);
                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
    }
}
