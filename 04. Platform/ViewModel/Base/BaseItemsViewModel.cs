using System.Collections.Generic;

namespace RP.Platform.ViewModel
{
	public class BaseItemsViewModel<TFilterViewModel, TItemViewModel>
		where TFilterViewModel : BaseFilterViewModel
		where TItemViewModel : BaseItemViewModel
	{
		public int Count { get; set; }
		public TFilterViewModel Filter { get; set; }
		public ICollection<TItemViewModel> Items { get; set; }
	}
}