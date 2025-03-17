using System;
using System.Data;
using LibraryMSWF.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMSWF.BL
{
    //CHECK THE ADMIN LOGIN CREDENTIALS =>BL
    public class AdminBL
    {
        private readonly AdminDAL adminDal = new AdminDAL();
        
        public bool AdminLoginBL(string adminEmail, string adminPass)
        {
            bool isDone = adminDal.AdminLoginDAL(adminEmail, adminPass);
            return isDone;
        }
        
        public DataTable GetAllBooksBL()
        {
            return adminDal.GetAllBooksDAL();
        }
        
        public DataTable GetAllUsersBL()
        {
            return adminDal.GetAllUsersDAL();
        }
        
        public DataTable GetAllRequestsBL()
        {
            return adminDal.GetAllRequestsDAL();
        }
        
        public DataTable GetAllAcceptedRequestsBL()
        {
            return adminDal.GetAllAcceptedRequestsDAL();
        }
    }
}
