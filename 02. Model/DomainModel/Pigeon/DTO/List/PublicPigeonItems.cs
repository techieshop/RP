using RP.Common.Attribute;

namespace RP.Model
{
	public class PublicPigeonItems : BaseItems<PublicPigeonItem>
	{
		[Metadata(Order = 11)]
		public int OrganizationCount { get; set; }

		[Metadata(Order = 12)]
		public int MemberCount { get; set; }

		[Metadata(Order = 13)]
		public int PigeonCount { get; set; }
	}
}