using RP.Model;

namespace RP.DAL.Mapping
{
	public class UserRoleMap : BaseModelMap<UserRole>
	{
		public UserRoleMap()
		{
			ToTable("UserRole", "acc");

			Property(t => t.UserId).IsRequired();
			Property(t => t.RoleId).IsRequired();
			Property(t => t.OrganizationId).IsRequired();

			HasRequired(t => t.User).WithMany().HasForeignKey(t => t.UserId);
			HasRequired(t => t.Role).WithMany().HasForeignKey(t => t.RoleId);
			HasRequired(t => t.Organization).WithMany().HasForeignKey(t => t.OrganizationId);
		}
	}
}