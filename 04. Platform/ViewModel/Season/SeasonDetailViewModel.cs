using RP.Model;
using RP.Platform.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class SeasonDetailViewModel : BaseDetailViewModel
	{
		public int MemberCount { get; set; }
		public string OrganizationName { get; set; }
		public int PigeonCount { get; set; }

		[RPRequired]
		[RPDisplay(LabelCode = Dom.Translation.Common.Type)]
		public int SeasonTypeId { get; set; }

		public ICollection<SelectListItem> SeasonTypeItems { get; set; }
		public string SeasonTypeName { get; set; }

		[RPRequired]
		[RPDisplay(LabelCode = Dom.Translation.Pigeon.Year)]
		public int Year { get; set; }
	}
}