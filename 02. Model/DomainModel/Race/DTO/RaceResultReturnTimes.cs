using RP.Common.Attribute;
using System.Collections.Generic;

namespace RP.Model
{
	public class RaceResultReturnTimes : BaseDetails<RaceDetail>
	{
		[Metadata(Order = 2)]
		public ICollection<MemberRef> Members { get; set; }

		[Metadata(Order = 3)]
		public ICollection<PigeonRef> Pigeons { get; set; }

		[Metadata(Order = 4)]
		public ICollection<PigeonReturnTimeRef> PigeonReturnTimes { get; set; }
	}
}