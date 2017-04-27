using RP.Model;
using RP.Platform.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class SeasonAddViewModel
	{
		[RPDisplay(LabelCode = Dom.Translation.Organization.Singular)]
		[RPRequired]
		public int OrganizationId { get; set; }

		public ICollection<SelectListItem> OrganizationItems { get; set; }

		[RPRequired]
		[RPDisplay(LabelCode = Dom.Translation.Common.Type)]
		public int SeasonTypeId { get; set; }

		public ICollection<SelectListItem> SeasonTypeItems { get; set; }

		[RPRequired]
		[RPDisplay(LabelCode = Dom.Translation.Pigeon.Year)]
		public int Year { get; set; }
	}
}