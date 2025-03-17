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
    public class UserDAL
    {
        SqlConnection conn = new SqlConnection("Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True");

        //RETURN COMPLETE USERS FROM USER TABLE =>DAL
        public DataSet GetAllUsersDAL()
        {
            using var da = new SqlDataAdapter("GetAllUsers", conn);
            var ds = new DataSet("Users");
            da.Fill(ds);
            return ds;
        }
        //ADD USER INTO USER TABLE =>DAL
        public string AddUserDAL(string userName, string userEmail, string userPass)
        {
            SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM UserTable WHERE Email=@email", conn);
            cmd2.Parameters.Add(new SqlParameter("@email", userEmail));
            conn.Open();
            int count = (int)cmd2.ExecuteScalar();
            conn.Close();
            if (count > 0)
            {
                return "User already exists, ";
            }
            else
            {
                try
                {
                    // Begin transaction to ensure both operations succeed or fail together
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    
                    try
                    {
                        // First, insert into Entity table with all required fields
                        SqlCommand entityCmd = new SqlCommand(
                            "INSERT INTO Entity (EntityType, Email, Password) VALUES ('User', @email, @pass); SELECT SCOPE_IDENTITY();", 
                            conn, 
                            transaction);
                        entityCmd.Parameters.Add(new SqlParameter("@email", userEmail));
                        entityCmd.Parameters.Add(new SqlParameter("@pass", userPass));
                        
                        // Get the newly created EntityId
                        int entityId = Convert.ToInt32(entityCmd.ExecuteScalar());
                        
                        // Now insert into UserTable with the correct EntityId
                        SqlCommand userCmd = new SqlCommand(
                            "INSERT INTO UserTable (EntityId, UserName, Email, Password, IsVerified) " +
                            "VALUES (@entityId, @name, @email, @pass, @isVerified)", 
                            conn, 
                            transaction);
                        
                        userCmd.Parameters.Add(new SqlParameter("@entityId", entityId));
                        userCmd.Parameters.Add(new SqlParameter("@name", userName));
                        userCmd.Parameters.Add(new SqlParameter("@email", userEmail));
                        userCmd.Parameters.Add(new SqlParameter("@pass", userPass));
                        userCmd.Parameters.Add(new SqlParameter("@isVerified", true));
                        
                        int rowAffected = userCmd.ExecuteNonQuery();
                        
                        // Commit the transaction
                        transaction.Commit();
                        
                        if (rowAffected > 0)
                        {
                            return "true";
                        }
                        else
                        {
                            return "false";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction on error
                        transaction.Rollback();
                        return $"Database error: {ex.Message}";
                    }
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        //UPDATE THE USER FROM USER TABLE =>DAL
        public bool UpdateUserDAL(int userId, string userName, string userEmail, string userPass)
        {
            SqlCommand cmd = new SqlCommand("UPDATE UserTable SET UserName=@name, Email=@email, Password=@pass WHERE UserId=@userId", conn);
            cmd.Parameters.Add(new SqlParameter("@userId", userId));
            cmd.Parameters.Add(new SqlParameter("@name", userName));
            cmd.Parameters.Add(new SqlParameter("@email", userEmail));
            cmd.Parameters.Add(new SqlParameter("@pass", userPass));
            conn.Open();
            int rowAffected = cmd.ExecuteNonQuery();
            conn.Close();
            return rowAffected > 0;
        }
        //DELETE THE USER FROM USER TABLE =>DAL
        public bool DeleteUserDAL(int userId)
        {
            SqlCommand cmd = new SqlCommand("DeleteUser @id", conn);
            cmd.Parameters.Add(new SqlParameter("@id", userId));
            conn.Open();
            int rowAffected = cmd.ExecuteNonQuery();
            conn.Close();
            return rowAffected > 0;
        }
        //RETURN USER NAME =>DAL
        public string TakeUserNameDAL(int userId)
        {
            SqlCommand cmd = new SqlCommand("TakeUserName @userId", conn);
            cmd.Parameters.Add(new SqlParameter("@userId", userId));
            conn.Open();
            string userName = (string)cmd.ExecuteScalar();
            conn.Close();
            return userName;
        }
        //CHECK THE USER LOGIN CREDENTIALS RETURN USER ID =>DAL
        public int UserLoginDAL(string userEmail, string userPass)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UserLogin @email, @pass", conn);
                cmd.Parameters.Add(new SqlParameter("@email", userEmail));
                cmd.Parameters.Add(new SqlParameter("@pass", userPass));
                conn.Open();
                
                // Safely handle null return values
                object result = cmd.ExecuteScalar();
                int userId = result != null ? Convert.ToInt32(result) : 0;
                
                conn.Close();
                return userId;
            }
            catch (Exception)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                return 0;
            }
        }
    }
}
