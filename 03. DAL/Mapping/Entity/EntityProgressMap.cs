using RP.Model;

namespace RP.DAL.Mapping
{
	internal class EntityProgressMap : BaseEntityMap<EntityProgress>
	{
		public EntityProgressMap()
		{
			ToTable("EntityProgress", "plt");

			Property(t => t.DateTime).IsRequired();
			Property(t => t.OrganizationId).IsOptional();
			Property(t => t.UserId).IsOptional();
			Property(t => t.EntityStateBeforeId).IsOptional();
			Property(t => t.EntityStateAfterId).IsRequired();
			Property(t => t.Remarks).HasMaxLength(256).IsOptional();
			Property(t => t.IpAddress).HasMaxLength(32).IsOptional();

			HasOptional(t => t.Organization).WithMany().HasForeignKey(t => t.OrganizationId);
			HasOptional(t => t.User).WithMany().HasForeignKey(t => t.UserId);
			HasOptional(t => t.EntityStateBefore).WithMany().HasForeignKey(t => t.EntityStateBeforeId);
			HasRequired(t => t.EntityStateAfter).WithMany().HasForeignKey(t => t.EntityStateAfterId);
		}
	}
}