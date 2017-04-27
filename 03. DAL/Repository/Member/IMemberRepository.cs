using RP.DAL.DBContext;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public interface IMemberRepository : IRepository<EntityDbContext>
	{
		MemberDetails GetMemberDetails(int userId, int contextOrganizationId, int id);

		MemberItems GetMemberItems(int userId, int contextOrganizationId, MemberFilter filter);

		ICollection<SelectListItemCount> GetMembers(int userId, int contextOrganizationId);
	}
}