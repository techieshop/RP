using RP.Common.Attribute;
using System.Collections.Generic;

namespace RP.Model
{
	public class BaseItems<TItem> where TItem : BaseItem
	{
		[Metadata(Order = 1)]
		public ICollection<TItem> Items { get; set; }

		[Metadata(Order = 10)]
		public int Count { get; set; }
	}
}