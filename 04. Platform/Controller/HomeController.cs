using System.Web.Mvc;
using RP.Platform.Manager;

namespace RP.Platform.Controller
{
	public class HomeController : BaseController
	{
		public ActionResult Dashboard()
		{
			return View(Mvc.View.Home.Dashboard);
		}
	}
}