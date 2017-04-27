using System.Collections.Generic;

namespace RP.Model
{
	public class Season : BaseEntity
	{
		public virtual int SeasonTypeId { get; set; }
		public virtual int Year { get; set; }

		public virtual DomainValue SeasonType { get; set; }

		public virtual ICollection<Race> Races { get; set; }
		public virtual ICollection<Statement> Statements { get; set; }

		public static Season Empty()
		{
			return new Season
			{
				EntityInfo = EntityInfo.Empty(Dom.EntityType.Season.Id)
			};
		}
	}
}