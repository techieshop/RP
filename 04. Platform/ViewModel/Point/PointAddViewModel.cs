using RP.Model;
using RP.Platform.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class PointAddViewModel
	{
		public AddressViewModel Address { get; set; }

		[RPRequired]
		[RPMaxLength(128)]
		[RPDisplay(LabelCode = Dom.Translation.Common.Name)]
		public string Name { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Organization.Singular)]
		[RPRequired]
		public int OrganizationId { get; set; }

		public ICollection<SelectListItem> OrganizationItems { get; set; }
	}
}