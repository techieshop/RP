using RP.Common.Attribute;

namespace RP.Model
{
	public class PigeonItems : BaseItems<PigeonItem>
	{
		[Metadata(Order = 11)]
		public int OrganizationCount { get; set; }

		[Metadata(Order = 12)]
		public int MemberCount { get; set; }
	}
}