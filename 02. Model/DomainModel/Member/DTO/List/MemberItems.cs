using RP.Common.Attribute;

namespace RP.Model
{
	public class MemberItems : BaseItems<MemberItem>
	{
		[Metadata(Order = 11)]
		public int OrganizationCount { get; set; }

		[Metadata(Order = 12)]
		public int PigeonCount { get; set; }
	}
}