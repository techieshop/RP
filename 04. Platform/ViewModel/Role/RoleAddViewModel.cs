using RP.Model;
using RP.Platform.DataAnnotations;

namespace RP.Platform.ViewModel
{
	public class RoleAddViewModel
	{
		[RPDisplay(LabelCode = Dom.Translation.Common.Description)]
		public string Description { get; set; }

		[RPRequired]
		[RPMaxLength(256)]
		[RPDisplay(LabelCode = Dom.Translation.Common.Name)]
		public string Name { get; set; }
	}
}