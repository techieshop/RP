using RP.Common.Attribute;
using System.Collections.Generic;

namespace RP.Model
{
	public class MemberDetails : BaseDetails<MemberDetail>
	{
		[Metadata(Order = 11)]
		public AddressRef Address { get; set; }

		[Metadata(Order = 12)]
		public WebsiteRef Website { get; set; }

		[Metadata(Order = 13)]
		public ICollection<OrganizationMemberTypeRef> OrganizationMemberTypes { get; set; }
	}
}