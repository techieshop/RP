using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public class RoleRepository : BaseRepository<EntityDbContext>, IRoleRepository
	{
		public RoleRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public RoleDetails GetRoleDetails(int userId, int contextOrganizationId, int id)

		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				RoleId = id
			};

			return ExecuteMultiResultSetSp<RoleDetails>("[acc].[spGetRoleDetails]", parameters);
		}

		public RoleItems GetRoleItems(int userId, int contextOrganizationId, RoleFilter filter)
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

			return ExecuteMultiResultSetSp<RoleItems>("[acc].[spGetRoleItems]", parameters);
		}

		public ICollection<SelectListItemCount> GetRoles(int userId, int contextOrganizationId)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId
			};

			return ExecuteSp<SelectListItemCount>("[acc].[spGetRoles]", parameters);
		}
	}
}