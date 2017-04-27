using System.Web.Optimization;

namespace RP.Platform.Bundling
{
	public class DefaultBundleResolver : IBundleResolver
	{
		public void RegisterBundle(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/js").Include(
				"~/bower_components/jquery/dist/jquery.min.js",
				"~/bower_components/jquery-ui/jquery-ui.min.js",
				"~/Scripts/jquery.sortable.js",
				"~/Scripts/jquery.unobtrusive-ajax.js",
				"~/bower_components/jquery-validation/dist/jquery.validate.min.js",
				"~/bower_components/moment/min/moment-with-locales.min.js",
				"~/bower_components/bootstrap/dist/js/bootstrap.min.js",
				"~/bower_components/eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js",
				"~/Scripts/jquery.slimscroll.js",
				"~/Scripts/common.js"));

			bundles.Add(new StyleBundle("~/css").Include(
				"~/bower_components/bootstrap/dist/css/bootstrap.min.css",
				"~/bower_components/bootstrap/dist/css/bootstrap-theme.min.css",
				"~/bower_components/eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.min.css",
				"~/Styles/common.css"));
		}
	}
}