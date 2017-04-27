using RP.Model;

namespace RP.DAL.Mapping
{
	public class RoleMap : BaseEntityMap<Role>
	{
		public RoleMap()
		{
			ToTable("Role", "acc");

			Property(t => t.Name).HasMaxLength(256).IsRequired();
			Property(t => t.Description).IsOptional();

			HasMany(t => t.RoleEntityTypeAccess).WithRequired().HasForeignKey(t => t.RoleId);
			HasMany(t => t.RoleEntityStateAccess).WithRequired().HasForeignKey(t => t.RoleId);
			HasMany(t => t.RoleEntityStateTransitionAccess).WithRequired().HasForeignKey(t => t.RoleId);

			HasMany(t => t.UserRoles).WithRequired().HasForeignKey(t => t.RoleId);
		}
	}
}