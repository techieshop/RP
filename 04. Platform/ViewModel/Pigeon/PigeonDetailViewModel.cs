using RP.Model;
using RP.Platform.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class PigeonDetailViewModel : BaseDetailViewModel
	{
		[RPMaxLength(16)]
		[RPDisplay(LabelCode = Dom.Translation.Pigeon.Code)]
		public string Code { get; set; }

		[RPRequired]
		[RPMaxLength(16)]
		[RPDisplay(LabelCode = Dom.Translation.Pigeon.Number)]
		public string Number { get; set; }

		public string OrganizationName { get; set; }
		public string OwnerFormattedName { get; set; }
		public int PrizeCount { get; set; }
		public string Ring { get; set; }
		public string Sex { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Sex.Name)]
		public int? SexId { get; set; }

		public ICollection<SelectListItem> SexItems { get; set; }

		[RPRequired]
		[RPMaxLength(4)]
		[RPDisplay(LabelCode = Dom.Translation.Pigeon.Year)]
		public string Year { get; set; }
	}
}