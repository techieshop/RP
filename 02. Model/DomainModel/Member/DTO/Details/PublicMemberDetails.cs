using RP.Common.Attribute;
using System.Collections.Generic;

namespace RP.Model
{
	public class PublicMemberDetails : BaseDetails<PublicMemberDetail>
	{
		[Metadata(Order = 11)]
		public Address Address { get; set; }

		[Metadata(Order = 12)]
		public Website Website { get; set; }

		[Metadata(Order = 13)]
		public ICollection<OrganizationMemberType> OrganizationMemberTypes { get; set; }
	}
}