using RP.Common.Attribute;
using System.Collections.Generic;

namespace RP.Model
{
	public class OrganizationDetails : BaseDetails<OrganizationDetail>
	{
		[Metadata(Order = 2)]
		public AddressRef Address { get; set; }

		[Metadata(Order = 3)]
		public WebsiteRef Website { get; set; }

		[Metadata(Order = 4)]
		public ICollection<MemberRef> Members { get; set; }

		[Metadata(Order = 5)]
		public ICollection<OrganizationMemberTypeRef> OrganizationMemberTypes { get; set; }
	}
}