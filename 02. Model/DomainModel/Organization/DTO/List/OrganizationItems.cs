using RP.Common.Attribute;
using System.Collections.Generic;

namespace RP.Model
{
	public class OrganizationItems : BaseItems<OrganizationItem>
	{
		[Metadata(Order = 11)]
		public ICollection<OrganizationMemberTypeRef> Members { get; set; }

		[Metadata(Order = 12)]
		public int MemberCount { get; set; }

		[Metadata(Order = 13)]
		public int PigeonCount { get; set; }
	}
}