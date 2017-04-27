using System.Linq;
using System.Web.Optimization;

namespace RP.Platform.Bundling
{
	public static class BundleConfig
	{
		public static void RegisterBundle(params IBundleResolver[] bundleResolvers)
		{
			if (bundleResolvers != null && bundleResolvers.Any())
			{
				foreach (var bundleResolver in bundleResolvers)
				{
					bundleResolver.RegisterBundle(BundleTable.Bundles);
				}
			}
		}
	}
}