using RP.Platform.Context;
using RP.Platform.Manager;
using System.Web.Mvc;

namespace RP.Platform.Controller
{
	public class SettingController : BaseController
	{
		[HttpGet]
		[Auth]
		public ActionResult List()
		{
			return View(Mvc.View.Setting.List);
		}
	}
}