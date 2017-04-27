using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public partial class UserRepository : BaseRepository<EntityDbContext>, IUserRepository
	{
		public UserRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public UserDetails GetUserDetails(int userId, int contextOrganizationId, int id)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				UId = id
			};

			return ExecuteMultiResultSetSp<UserDetails>("[acc].[spGetUserDetails]", parameters);
		}

		public UserItems GetUserItems(int userId, int contextOrganizationId, UserFilter filter)
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

			return ExecuteMultiResultSetSp<UserItems>("[acc].[spGetUserItems]", parameters);
		}

		public ICollection<SelectListItemCount> GetUsers(int userId, int contextOrganizationId)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId
			};

			return ExecuteSp<SelectListItemCount>("[acc].[spGetUsers]", parameters);
		}
	}
}