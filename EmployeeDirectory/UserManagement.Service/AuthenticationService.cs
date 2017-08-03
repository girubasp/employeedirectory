using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        public bool ValidateCredentials(string email, string password, bool rememberMe)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                conn.Open();

                using (var command = new SqlCommand(string.Format("SELECT Password FROM [dbo].[User] WHERE Email = '{0}'", email), conn))
                {
                    var pwd = command.ExecuteScalar();
                    if (pwd == null) return false;


                    if (password != pwd.ToString()) return false;

                    return true;
                }
            }
        }

        public bool IsUserAdmin(string email)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                conn.Open();

                using (var command = new SqlCommand(string.Format("SELECT isAdmin FROM [dbo].[User] WHERE Email = '{0}'", email), conn))
                {
                    var isAdmin = command.ExecuteScalar();
                    if (isAdmin == null) throw new Exception("User not found");


                    return (bool) isAdmin;
                }
            }
        }
    }
}
