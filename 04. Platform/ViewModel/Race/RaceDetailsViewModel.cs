using RP.Model;
using RP.Platform.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class RaceDetailsViewModel : BaseDetailsViewModel<RaceDetailViewModel>
	{
		public AddressViewModel Address { get; set; }

		public bool CanCalculateResults { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Season.StatementLabel)]
		public ICollection<int> StatementIds { get; set; }

		public IDictionary<string, ICollection<SelectListItem>> StatementItems { get; set; }
	}
}