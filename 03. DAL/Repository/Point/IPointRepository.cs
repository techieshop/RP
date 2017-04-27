using RP.DAL.DBContext;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public interface IPointRepository : IRepository<EntityDbContext>
	{
		PointDetails GetPointDetails(int userId, int contextOrganizationId, int id);

		PointItems GetPointItems(int userId, int contextOrganizationId, PointFilter filter);

		ICollection<SelectListItemCount> GetPoints(int userId, int contextOrganizationId, int? organizationId = null);
	}
}