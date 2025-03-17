using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LibraryMSWF.DAL 
{ 
    public static class DatabaseConnection 
    { 
        private static readonly string connectionString = "Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True";
        
        public static string ConnectionString => connectionString;
        
        public static SqlConnection GetConnection() 
        { 
            return new SqlConnection(connectionString); 
        } 
    } 
}