namespace RP.Model
{
	public class EntityOrganization : BaseEntity
	{
		public virtual int EntityTypeId { get; set; }
		public virtual int OrganizationId { get; set; }
		public virtual int EntityStateId { get; set; }

		public virtual Organization Organization { get; set; }
		public virtual EntityState EntityState { get; set; }
	}
}