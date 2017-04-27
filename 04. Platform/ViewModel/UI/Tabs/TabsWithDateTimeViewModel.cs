using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class TabsWithDateTimeViewModel : BaseInputViewModel
	{
		public TabsWithDateTimeViewModel(ModelMetadata metadata)
			: base(metadata)
		{
		}

		public IDictionary<string, ICollection<DateTimeSelectListItem>> Items { get; set; }
		public string Name { get; set; }
	}
}