using RP.Common.Attribute;
using System.Collections.Generic;

namespace RP.Model
{
	public class ResultMasterItems
	{
		[Metadata(Order = 1)]
		public ResultDetail Detail { get; set; }

		[Metadata(Order = 2)]
		public ICollection<ResultMasterItem> Items { get; set; }
	}
}