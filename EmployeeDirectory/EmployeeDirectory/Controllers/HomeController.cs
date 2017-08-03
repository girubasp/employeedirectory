using System.Web.Mvc;

namespace EmployeeDirectory.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}