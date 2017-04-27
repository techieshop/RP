using System.Linq;
using System.Web.Mvc;

namespace RP.Platform.Filtering
{
	public static class FilterConfig
	{
		public static void RegisterFilter(params IFilterResolver[] filterResolvers)
		{
			if (filterResolvers != null && filterResolvers.Any())
			{
				foreach (var filterResolver in filterResolvers)
				{
					filterResolver.RegisterFilter(GlobalFilters.Filters);
				}
			}
		}
	}
}