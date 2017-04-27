using RP.DAL.DBContext;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public interface ISeasonRepository : IRepository<EntityDbContext>
	{
		SeasonDetails GetSeasonDetails(int userId, int contextOrganizationId, int id);

		SeasonItems GetSeasonItems(int userId, int contextOrganizationId, SeasonFilter filter);

		ICollection<SelectListItemCount> GetSeasons(int userId, int contextOrganizationId, int? organizationId = null);
	}
}