using RP.Common.Attribute;
using System.Collections.Generic;

namespace RP.Model
{
	public class ResultTimeItems
	{
		[Metadata(Order = 1)]
		public ResultDetailCommon Detail { get; set; }

		[Metadata(Order = 2)]
		public ICollection<ResultTimeItem> Items { get; set; }
	}
}