using System.Collections.Generic;

namespace RP.Platform.ViewModel
{
	public class BaseEntityFilterViewModel : BaseFilterViewModel
	{
		public ICollection<int> EntityStateIds { get; set; }
		public IDictionary<int, string> EntityStateItems { get; set; }
	}
}