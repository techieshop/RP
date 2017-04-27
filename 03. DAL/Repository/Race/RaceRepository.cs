using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.DAL.Repository
{
	public class RaceRepository : BaseRepository<EntityDbContext>, IRaceRepository
	{
		public RaceRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public RaceDetails GetRaceDetails(int userId, int contextOrganizationId, int id)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				RaceId = id
			};

			return ExecuteMultiResultSetSp<RaceDetails>("[race].[spGetRaceDetails]", parameters);
		}

		public RaceItems GetRaceItems(int userId, int contextOrganizationId, RaceFilter filter)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				filter.EntityStateIds,
				filter.Search,
				filter.Skip,
				filter.Take
			};

			return ExecuteMultiResultSetSp<RaceItems>("[race].[spGetRaceItems]", parameters);
		}

		public RaceResultReturnTimes GetRaceResultReturnTimes(int userId, int contextOrganizationId, int id)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				RaceId = id
			};

			return ExecuteMultiResultSetSp<RaceResultReturnTimes>("[race].[spGetRaceResultReturnTimes]", parameters);
		}

		public ResultCommandItems GetResultCommand(int userId, int contextOrganizationId, int id)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				RaceId = id
			};

			return ExecuteMultiResultSetSp<ResultCommandItems>("[race].[spGetResultCommand]", parameters);
		}

		public ResultDetail GetResultDetails(int userId, int contextOrganizationId, int id)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				RaceId = id
			};

			return ExecuteSp<ResultDetail>("[race].[spGetResultDetails]", parameters).FirstOrDefault();
		}

		public ICollection<ResultItem> GetResultItems(int userId, int contextOrganizationId)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId
			};

			return ExecuteSp<ResultItem>("[race].[spGetResultItems]", parameters);
		}

		public ResultMasterItems GetResultMaster(int userId, int contextOrganizationId, int id)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				RaceId = id
			};

			return ExecuteMultiResultSetSp<ResultMasterItems>("[race].[spGetResultMaster]", parameters);
		}

		public ResultTimeItems GetResultTime(int userId, int contextOrganizationId, int id)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				RaceId = id
			};

			return ExecuteMultiResultSetSp<ResultTimeItems>("[race].[spGetResultTime]", parameters);
		}
	}
}