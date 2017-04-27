using System.Collections.Generic;
using System.Web.Mvc;
using RP.Model;
using RP.Platform.DataAnnotations;

namespace RP.Platform.ViewModel
{
	public class SeasonDetailsViewModel : BaseDetailsViewModel<SeasonDetailViewModel>
	{
		[RPDisplay(LabelCode = Dom.Translation.Season.StatementLabel)]
		public ICollection<int> StatementIds { get; set; }
		public IDictionary<string, ICollection<SelectListItem>> StatementItems { get; set; }
	}
}