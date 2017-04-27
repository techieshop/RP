using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class TabsWithDragViewModel : BaseInputViewModel
	{
		public TabsWithDragViewModel(ModelMetadata metadata)
			: base(metadata)
		{
		}

		public IDictionary<string, ICollection<SelectListItem>> Items { get; set; }
		public string Name { get; set; }
	}
}