using System.Collections.Generic;

namespace RP.Model
{
	public class UserAuth
	{
		public UserData UserData { get; set; }
		public ICollection<EntityTypeAccess> EntityTypeAccess { get; set; }
		public ICollection<EntityStateAccess> EntityStateAccess { get; set; }
		public ICollection<EntityStateTransitionAccess> EntityStateTransitionAccess { get; set; }
	}
}