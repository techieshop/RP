using RP.Common.Attribute;
using System.Collections.Generic;

namespace RP.Model
{
	public class PublicOrganizationDetails : BaseDetails<PublicOrganizationDetail>
	{
		[Metadata(Order = 2)]
		public Address Address { get; set; }

		[Metadata(Order = 3)]
		public Website Website { get; set; }

		[Metadata(Order = 4)]
		public ICollection<OrganizationMemberType> OrganizationMemberTypes { get; set; }
	}
}