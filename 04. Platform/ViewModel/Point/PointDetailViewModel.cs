using RP.Model;
using RP.Platform.DataAnnotations;

namespace RP.Platform.ViewModel
{
	public class PointDetailViewModel : BaseDetailViewModel
	{
		[RPRequired]
		[RPMaxLength(128)]
		[RPDisplay(LabelCode = Dom.Translation.Common.Name)]
		public string Name { get; set; }

		public string OrganizationName { get; set; }
	}
}