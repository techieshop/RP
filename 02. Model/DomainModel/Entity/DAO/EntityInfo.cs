using System;
using System.Collections.Generic;

namespace RP.Model
{
	public class EntityInfo : BaseModel
	{
		public virtual Guid Guid { get; set; }
		public virtual DateTime CreatedDate { get; set; }
		public virtual int EntityTypeId { get; set; }

		public virtual ICollection<EntityProgress> EntityProgress { get; set; }
		public virtual ICollection<EntityOrganization> EntityOrganizations { get; set; }

		public static EntityInfo Empty(int entityTypeId)
		{
			return new EntityInfo
			{
				Guid = Guid.NewGuid(),
				EntityTypeId = entityTypeId,
				CreatedDate = DateTime.UtcNow,
				EntityOrganizations = new List<EntityOrganization>(),
				EntityProgress = new List<EntityProgress>()
			};
		}
	}
}