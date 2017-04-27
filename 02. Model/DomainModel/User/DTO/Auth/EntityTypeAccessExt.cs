using System.Collections.Generic;

namespace RP.Model
{
	public class EntityTypeAccessExt
	{
		public int AccessTypeId { get; set; }
		public IDictionary<int, int> EntityStateAccess { get; set; }
		public ISet<int> EntityStateTransitionAccess { get; set; }
	}
}