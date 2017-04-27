using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public class PointRepository : BaseRepository<EntityDbContext>, IPointRepository
	{
		public PointRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public PointDetails GetPointDetails(int userId, int contextOrganizationId, int id)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				pointId = id
			};

			return ExecuteMultiResultSetSp<PointDetails>("[race].[spGetPointDetails]", parameters);
		}

		public PointItems GetPointItems(int userId, int contextOrganizationId, PointFilter filter)
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

			return ExecuteMultiResultSetSp<PointItems>("[race].[spGetPointItems]", parameters);
		}

		public ICollection<SelectListItemCount> GetPoints(int userId, int contextOrganizationId, int? organizationId = null)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				OrganizationId = organizationId
			};

			return ExecuteSp<SelectListItemCount>("[race].[spGetPoints]", parameters);
		}
	}
}