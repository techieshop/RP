using System.Linq;
using System.Web.Routing;

namespace RP.Platform.Routing
{
	public static class RouteConfig
	{
		public static void RegisterRoute(params IRouteResolver[] routeResolvers)
		{
			if (routeResolvers != null && routeResolvers.Any())
			{
				foreach (var routeResolver in routeResolvers)
				{
					routeResolver.RegisterRoute(RouteTable.Routes);
				}
			}
		}
	}
}