using RP.DAL.DBContext;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public interface IRaceRepository : IRepository<EntityDbContext>
	{
		RaceDetails GetRaceDetails(int userId, int contextOrganizationId, int id);

		RaceItems GetRaceItems(int userId, int contextOrganizationId, RaceFilter filter);

		RaceResultReturnTimes GetRaceResultReturnTimes(int userId, int contextOrganizationId, int id);

		ResultCommandItems GetResultCommand(int userId, int contextOrganizationId, int id);

		ResultDetail GetResultDetails(int userId, int contextOrganizationId, int id);

		ICollection<ResultItem> GetResultItems(int userId, int contextOrganizationId);

		ResultMasterItems GetResultMaster(int userId, int contextOrganizationId, int id);

		ResultTimeItems GetResultTime(int userId, int contextOrganizationId, int id);
	}
}