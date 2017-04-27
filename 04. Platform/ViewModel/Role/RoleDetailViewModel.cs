using RP.Model;
using RP.Platform.DataAnnotations;

namespace RP.Platform.ViewModel
{
	public class RoleDetailViewModel : BaseDetailViewModel
	{
		[RPDisplay(LabelCode = Dom.Translation.Common.Description)]
		public string Description { get; set; }

		[RPRequired]
		[RPMaxLength(32)]
		[RPDisplay(LabelCode = Dom.Translation.Common.Name)]
		public string Name { get; set; }
	}
}