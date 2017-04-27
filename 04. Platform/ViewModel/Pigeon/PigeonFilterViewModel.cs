using RP.Model;
using RP.Platform.DataAnnotations;

namespace RP.Platform.ViewModel
{
	public class PigeonFilterViewModel : BaseEntityFilterViewModel
	{
		[RPDisplay(PlaceholderCode = Dom.Translation.Common.Title)]
		public string Search { get; set; }
	}
}