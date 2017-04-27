using RP.Model;
using RP.Platform.DataAnnotations;

namespace RP.Platform.ViewModel
{
	public class WebsiteViewModel
	{
		[RPUrl]
		[RPMaxLength(256)]
		[RPDisplay(LabelCode = Dom.Translation.Website.Url)]
		public string Url { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Website.Description)]
		public string Description { get; set; }
	}
}