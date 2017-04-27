using RP.Model;
using RP.Platform.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class PigeonAddViewModel
	{
		[RPDisplay(LabelCode = Dom.Translation.Pigeon.Code)]
		[RPMaxLength(16)]
		public string Code { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Pigeon.Number)]
		[RPMaxLength(16)]
		[RPRequired]
		public string Number { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Pigeon.Trainer)]
		[RPRequired]
		public int? OwnerId { get; set; }

		public ICollection<SelectListItem> OwnerItems { get; set; }
		public string Ring { get; set; }
		public string Sex { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Sex.Name)]
		public int? SexId { get; set; }

		public ICollection<SelectListItem> SexItems { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Pigeon.Year)]
		[RPMaxLength(4)]
		[RPRequired]
		public string Year { get; set; }
	}
}