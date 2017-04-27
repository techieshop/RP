using RP.Platform.App_Start;
using RP.Platform.Manager;
using System;
using System.Web;

namespace RP.Web
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AppStart.Initialize();
		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
			AuthManager.AuthenticateRequest();
		}
	}
}