using System.Web.Mvc;

namespace RP.Platform.Controller
{
	public class ErrorController : BaseController
	{
		public ActionResult PageNotFound()
		{
			return NotFoundResult();
		}
	}
}