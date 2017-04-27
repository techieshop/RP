using System.Collections.Generic;
using RP.Model;
using RP.Platform.DataAnnotations;

namespace RP.Platform.ViewModel
{
	public class RaceResultReturnTimesViewModel : BaseDetailsViewModel<RaceDetailViewModel>
	{
		public IDictionary<string, ICollection<DateTimeSelectListItem>> PigeonReturnItems { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Race.PigeonReturnTimeLabel)]
		public string PigeonReturnTimesJson { get; set; }
	}
}