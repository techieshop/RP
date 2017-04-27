using System.Web;

namespace RP.Platform.Manager
{
	public static partial class WebUrlManager
	{
		public static string Host
		{
			get
			{
				return HttpContext.Current.Request.Url.Host;
			}
		}

		public static string Path
		{
			get
			{
				return HttpContext.Current.Request.Url.AbsolutePath;
			}
		}
	}
}