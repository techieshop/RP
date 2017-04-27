using RP.Model;
using RP.Platform.DataAnnotations;
using System;

namespace RP.Platform.ViewModel
{
	public class OrganizationDetailViewModel : BaseDetailViewModel
	{
		[RPRequired]
		[RPMaxLength(256)]
		[RPDisplay(LabelCode = Dom.Translation.Common.Name, PlaceholderCode = Dom.Translation.Organization.EnterOrganizationName)]
		public string Name { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Common.Description)]
		public string Description { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Common.CreatedDate)]
		public DateTime? CreateDate { get; set; }

		public int MemberCount { get; set; }
	}
}