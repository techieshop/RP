using RP.DAL.DBContext;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public interface IRoleRepository : IRepository<EntityDbContext>
	{
		RoleDetails GetRoleDetails(int userId, int contextOrganizationId, int id);

		RoleItems GetRoleItems(int userId, int contextOrganizationId, RoleFilter filter);

		ICollection<SelectListItemCount> GetRoles(int userId, int contextOrganizationId);
	}
}