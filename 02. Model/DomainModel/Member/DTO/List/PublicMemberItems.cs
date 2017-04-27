using RP.Common.Attribute;
using System.Collections.Generic;

namespace RP.Model
{
	public class PublicMemberItems : BaseItems<PublicMemberItem>
	{
		[Metadata(Order = 11)]
		public int OrganizationCount { get; set; }

		[Metadata(Order = 12)]
		public int MemberCount { get; set; }

		[Metadata(Order = 13)]
		public int PigeonCount { get; set; }

		[Metadata(Order = 14)]
		public ICollection<OrganizationMemberType> OrganizationMemberTypes { get; set; }
	}
}