using System.Web.Optimization;

namespace RP.Platform.Bundling
{
	public interface IBundleResolver
	{
		void RegisterBundle(BundleCollection bundles);
	}
}