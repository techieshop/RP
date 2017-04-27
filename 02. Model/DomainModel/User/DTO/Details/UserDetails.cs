using RP.Common.Attribute;
using System.Collections.Generic;

namespace RP.Model
{
	public class UserDetails : BaseDetails<UserDetail>
	{
		[Metadata(Order = 2)]
		public Address Address { get; set; }

		[Metadata(Order = 3)]
		public ICollection<UserRoleRef> UserRoles { get; set; }
	}
}