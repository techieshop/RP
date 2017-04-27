using RP.Common.Attribute;

namespace RP.Model
{
	public class PointDetails : BaseDetails<PointDetail>
	{
		[Metadata(Order = 11)]
		public AddressRef Address { get; set; }
	}
}