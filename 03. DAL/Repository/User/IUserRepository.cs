using RP.DAL.DBContext;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public interface IUserRepository : IRepository<EntityDbContext>
	{
		bool Exists(string email, int? userId = null);

		User GetUser(int userId);

		User GetUser(string email, string password);

		UserAuth GetUserAuth(int? userId);

		UserDetails GetUserDetails(int userId, int contextOrganizationId, int id);

		UserItems GetUserItems(int userId, int contextOrganizationId, UserFilter filter);

		UserProfile GetUserProfile(int userId);

		ICollection<SelectListItemCount> GetUsers(int userId, int contextOrganizationId);

		bool HasUserLoginAccess(int userId);
	}
}