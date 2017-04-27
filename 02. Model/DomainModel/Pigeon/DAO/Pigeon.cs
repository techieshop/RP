using System.Collections.Generic;

namespace RP.Model
{
	public class Pigeon : BaseEntity
	{
		public virtual string Code { get; set; }
		public virtual Member Member { get; set; }
		public virtual int MemberId { get; set; }
		public virtual string Name { get; set; }
		public virtual string Number { get; set; }
		public virtual ICollection<PigeonReturnTime> PigeonReturnTimes { get; set; }
		public virtual ICollection<RacePigeon> RacePigeons { get; set; }
		public virtual ICollection<RaceResult> RaceResults { get; set; }
		public virtual DomainValue Sex { get; set; }
		public virtual int? SexId { get; set; }
		public virtual ICollection<Statement> Statements { get; set; }
		public virtual int Year { get; set; }

		public static Pigeon Empty()
		{
			return new Pigeon
			{
				EntityInfo = EntityInfo.Empty(Dom.EntityType.Pigeon.Id)
			};
		}
	}
}