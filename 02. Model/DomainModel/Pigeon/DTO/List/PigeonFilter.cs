using System.Collections.Generic;

namespace RP.Model
{
	public class PigeonFilter : BaseEntityFilter
	{
		public string Search { get; set; }
		public ICollection<int> SexIds { get; set; }
	}
}