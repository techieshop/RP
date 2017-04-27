using System.Collections.Generic;

namespace RP.Model
{
	public class Point : BaseEntity
	{
		public virtual string Name { get; set; }
		public virtual int AddressId { get; set; }

		public virtual Address Address { get; set; }
		public virtual ICollection<Race> Races { get; set; }

		public static Point Empty()
		{
			return new Point
			{
				EntityInfo = EntityInfo.Empty(Dom.EntityType.Point.Id)
			};
		}
	}
}