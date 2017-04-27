using RP.Model;
using RP.Platform.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class OrganizationAddViewModel
	{
		[RPDisplay(LabelCode = Dom.Translation.Common.Name, PlaceholderCode = Dom.Translation.Organization.EnterOrganizationName)]
		[RPMaxLength(256)]
		[RPRequired]
		public string Name { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Organization.ParentOrganization)]
		[RPRequired]
		public int OrganizationId { get; set; }

		public ICollection<SelectListItem> OrganizationItems { get; set; }
		[RPDisplay(LabelCode = Dom.Translation.Organization.OrganizationType)]
		[RPRequired]
		public int OrganizationTypeId { get; set; }

		public ICollection<SelectListItem> OrganizationTypeItems { get; set; }
	}
}