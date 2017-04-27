using System.Collections.Generic;

namespace RP.Model
{
	public class EntityType : BaseModel
	{
		public virtual int? DescriptionCode { get; set; }
		public virtual string Name { get; set; }

		public virtual ICollection<EntityState> EntityStates { get; set; }
	}
}