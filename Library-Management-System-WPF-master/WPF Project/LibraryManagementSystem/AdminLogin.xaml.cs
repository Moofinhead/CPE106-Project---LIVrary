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
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LibraryMSWF.BL;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LibraryManagementSystem
{
    /// <summary>
    /// Interaction logic for AdminLogin.xaml
    /// </summary>
    public partial class AdminLogin : Page
    {
        public AdminLogin()
        {
            InitializeComponent();
        }
        //CHECK THE ADMIN LOGIN CREDENTIALS AND OPEN ADMIN HOME =>PL
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = tbAdminEmail.Text;
            string password = tbAdminPass.Password;

            SqlConnection con = new SqlConnection("Data Source=MIKEMIKE\\SQLEXPRESS;Initial Catalog=LibraryManagement;Integrated Security=True;TrustServerCertificate=True");
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                String query = "SELECT COUNT(1) FROM AdminTable WHERE Email=@Email AND Password=@Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 1)
                {
                    AdminHome adminHome = new AdminHome();
                    adminHome.Show();
                    Window.GetWindow(this).Close();
                }
                else
                {
                    alertAdmin.Text = "Username or password is incorrect.";
                }
            }
            catch (Exception ex)
            {
                alertAdmin.Text = ex.Message;
            }
            finally
            {
                con.Close();
            }
        }

        private async void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                await mainWindow.SlideToPage(new MainPage(), true);
            }
        }
    }
}
