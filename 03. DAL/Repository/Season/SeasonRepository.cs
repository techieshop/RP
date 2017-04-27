using System.Collections.Generic;
using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;

namespace RP.DAL.Repository
{
	public class SeasonRepository : BaseRepository<EntityDbContext>, ISeasonRepository
	{
		public SeasonRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public SeasonDetails GetSeasonDetails(int userId, int contextOrganizationId, int id)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				SeasonId = id
			};

			return ExecuteMultiResultSetSp<SeasonDetails>("[race].[spGetSeasonDetails]", parameters);
		}

		public SeasonItems GetSeasonItems(int userId, int contextOrganizationId, SeasonFilter filter)
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

			return ExecuteMultiResultSetSp<SeasonItems>("[race].[spGetSeasonItems]", parameters);
		}

		public ICollection<SelectListItemCount> GetSeasons(int userId, int contextOrganizationId, int? organizationId = null)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				OrganizationId = organizationId
			};

			return ExecuteSp<SelectListItemCount>("[race].[spGetSeasons]", parameters);
		}
	}
}