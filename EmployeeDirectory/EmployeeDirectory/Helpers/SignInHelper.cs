using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using EmployeeDirectory.Security;

namespace EmployeeDirectory.Helpers
{
	public static class SignInHelper
	{
		public static bool ValidateCredentials(string email, string password, bool rememberMe)
		{
			using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
			{
				conn.Open();

				using (var command = new SqlCommand(string.Format("SELECT Password FROM [dbo].[User] WHERE Email = '{0}'", email), conn))
				{
					var pwd = command.ExecuteScalar();
					if (pwd == null) return false;


					if (password != pwd.ToString()) return false;
					//var ticket = new FormsAuthenticationTicket(1,
					//	email,
					//	DateTime.Now,
					//	DateTime.Now.Add(FormsAuthentication.Timeout),
					//	rememberMe,A
					//	);

					//var encryptedTicket = FormsAuthentication.Encrypt(ticket);
					//var principle = new LoggedInUserPrincipal(ticket);

					//HttpContext.Current.User = principle;
					//System.Threading.Thread.CurrentPrincipal = principle;

					//var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
					//HttpContext.Current.Response.Cookies.Add(cookie);

					return true;
				}
			}
		}
	}
}