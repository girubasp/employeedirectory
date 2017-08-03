using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EmployeeDirectory.Models;
using UserManagement.Service;

namespace EmployeeDirectory.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
			if (User.Identity.IsAuthenticated)
			{
				if (returnUrl != null)
					return Redirect(returnUrl);
				return RedirectToAction("Index", "Home");
			}
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
		
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

	        if (_authenticationService.ValidateCredentials(model.Email, model.Password, model.RememberMe))
	        {
	            var role = _authenticationService.IsUserAdmin(model.Email) ? "Admin" : "User";

                var ticket = new FormsAuthenticationTicket(1,
	                model.Email,
	                DateTime.Now,
	                DateTime.Now.Add(FormsAuthentication.Timeout),
	                model.RememberMe,
                    role);
	            
                var encryptedTicket = FormsAuthentication.Encrypt(ticket);
	           
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
	            HttpContext.Response.Cookies.Add(cookie);

                if (returnUrl != null && !string.IsNullOrWhiteSpace(returnUrl))
					return Redirect(returnUrl);
				return RedirectToAction("Index", "Home");
	        }

			ModelState.AddModelError("Email", "Invalid email/password combination");
	        return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
			FormsAuthentication.SignOut();
			Session.Abandon();
			return RedirectToAction("Login", "Account");
        }
    }
}