using System;

namespace RP.Model
{
	public class EntityProgress : BaseEntity
	{
		public virtual DateTime DateTime { get; set; }
		public virtual int? OrganizationId { get; set; }
		public virtual int? UserId { get; set; }
		public virtual int? EntityStateBeforeId { get; set; }
		public virtual int EntityStateAfterId { get; set; }
		public virtual string Remarks { get; set; }
		public virtual string IpAddress { get; set; }

		public virtual Organization Organization { get; set; }
		public virtual User User { get; set; }
		public virtual EntityState EntityStateBefore { get; set; }
		public virtual EntityState EntityStateAfter { get; set; }
	}
}