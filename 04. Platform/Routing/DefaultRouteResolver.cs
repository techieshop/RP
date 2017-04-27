using RP.Platform.Manager;
using System.Web.Mvc;
using System.Web.Routing;

namespace RP.Platform.Routing
{
	public class DefaultRouteResolver : IRouteResolver
	{
		public void RegisterRoute(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("Default", "{controller}/{action}/{id}", new
			{
				controller = Mvc.Controller.Home.Name,
				action = Mvc.Controller.Home.Dashboard,
				id = UrlParameter.Optional
			}
			);
		}
	}
}