using RP.Common.Attribute;
using System.Collections.Generic;

namespace RP.Model
{
	public class RaceDetails : BaseDetails<RaceDetail>
	{
		[Metadata(Order = 2)]
		public AddressRef Address { get; set; }

		[Metadata(Order = 3)]
		public ICollection<MemberRef> Members { get; set; }

		[Metadata(Order = 4)]
		public ICollection<PigeonRef> Pigeons { get; set; }

		[Metadata(Order = 5)]
		public ICollection<int> StatementPigeonIds { get; set; }
	}
}