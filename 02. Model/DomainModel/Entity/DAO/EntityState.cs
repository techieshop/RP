using System.Collections.Generic;

namespace RP.Model
{
	public class EntityState : BaseModel
	{
		public virtual int NameCode { get; set; }
		public virtual int? DescriptionCode { get; set; }
		public virtual byte Order { get; set; }
		public virtual bool IsStart { get; set; }
		public virtual bool IsFinish { get; set; }
		public virtual bool IsActive { get; set; }
		public virtual int EntityTypeId { get; set; }

		public virtual ICollection<EntityStateTransition> TransitionsFrom { get; set; }
		public virtual ICollection<EntityStateTransition> TransitionsTo { get; set; }
	}
}