using RP.DAL.DBContext;
using RP.Model;

namespace RP.DAL.Repository
{
	public interface IPigeonRepository : IRepository<EntityDbContext>
	{
		PigeonDetails GetPigeonDetails(int userId, int contextOrganizationId, int id);

		PigeonItems GetPigeonItems(int userId, int contextOrganizationId, PigeonFilter filter);
	}
}