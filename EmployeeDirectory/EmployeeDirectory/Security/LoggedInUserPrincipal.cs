using System.Linq;
using System.Security.Principal;
using System.Web.Security;

namespace EmployeeDirectory.Security
{
	public class LoggedInUserPrincipal : IPrincipal
	{
	    public IIdentity Identity { get; private set; }
        public string[] Roles { get; private set; }
		public bool IsInRole(string role)
		{
		    return Roles.Contains(role);
		}

		public LoggedInUserPrincipal(FormsAuthenticationTicket ticket)
		{
		    Identity = new FormsIdentity(ticket);
		    Roles = ticket.UserData.Split(',');

        }
	}
}