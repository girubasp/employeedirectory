using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Service
{
    public interface IAuthenticationService
    {
        bool ValidateCredentials(string email, string password, bool rememberMe);
        bool IsUserAdmin(string email);
    }
}
