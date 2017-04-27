using System.Collections.Generic;

namespace RP.Model
{
	public class Role : BaseEntity
	{
		public virtual string Name { get; set; }
		public virtual string Description { get; set; }

		public virtual ICollection<RoleEntityTypeAccess> RoleEntityTypeAccess { get; set; }
		public virtual ICollection<RoleEntityStateAccess> RoleEntityStateAccess { get; set; }
		public virtual ICollection<RoleEntityStateTransitionAccess> RoleEntityStateTransitionAccess { get; set; }
		public virtual ICollection<UserRole> UserRoles { get; set; }
	}
}