using System.Web.Mvc;

namespace RP.Platform.Filtering
{
	public class DefaultFilterResolver : IFilterResolver
	{
		public void RegisterFilter(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}