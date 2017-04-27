using System.Web.Routing;

namespace RP.Platform.Routing
{
	public interface IRouteResolver
	{
		void RegisterRoute(RouteCollection routes);
	}
}