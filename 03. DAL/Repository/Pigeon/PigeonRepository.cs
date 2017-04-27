using RP.Common.Extension;
using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;

namespace RP.DAL.Repository
{
	public class PigeonRepository : BaseRepository<EntityDbContext>, IPigeonRepository
	{
		public PigeonRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public PigeonDetails GetPigeonDetails(int userId, int contextOrganizationId, int id)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				pigeonId = id
			};

			return ExecuteMultiResultSetSp<PigeonDetails>("[acc].[spGetPigeonDetails]", parameters);
		}

		public PigeonItems GetPigeonItems(int userId, int contextOrganizationId, PigeonFilter filter)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				filter.EntityStateIds,
				filter.Search,
				SexIds = filter.SexIds.ToKeyList(),
				filter.Skip,
				filter.Take
			};

			return ExecuteMultiResultSetSp<PigeonItems>("[acc].[spGetPigeonItems]", parameters);
		}
	}
}