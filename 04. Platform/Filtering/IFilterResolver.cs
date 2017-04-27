using System.Web.Mvc;

namespace RP.Platform.Filtering
{
	public interface IFilterResolver
	{
		void RegisterFilter(GlobalFilterCollection filters);
	}
}