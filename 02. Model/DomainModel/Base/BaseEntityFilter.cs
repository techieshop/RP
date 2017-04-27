using System.Collections.Generic;

namespace RP.Model
{
	public class BaseEntityFilter : BaseFilter
	{
		public ICollection<int> EntityStateIds { get; set; }
	}
}