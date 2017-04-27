using RP.Model;

namespace RP.DAL.Mapping
{
	internal class EntityOrganizationMap : BaseEntityMap<EntityOrganization>
	{
		public EntityOrganizationMap()
		{
			ToTable("EntityOrganization", "plt");

			Property(t => t.EntityTypeId).IsRequired();
			Property(t => t.OrganizationId).IsRequired();
			Property(t => t.EntityStateId).IsRequired();

			HasRequired(t => t.Organization).WithMany().HasForeignKey(t => t.OrganizationId);
			HasRequired(t => t.EntityState).WithMany().HasForeignKey(t => t.EntityStateId);
		}
	}
}