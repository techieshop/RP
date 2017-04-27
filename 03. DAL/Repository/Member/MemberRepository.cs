using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public class MemberRepository : BaseRepository<EntityDbContext>, IMemberRepository
	{
		public MemberRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public MemberDetails GetMemberDetails(int userId, int contextOrganizationId, int id)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				memberId = id
			};

			return ExecuteMultiResultSetSp<MemberDetails>("[acc].[spGetMemberDetails]", parameters);
		}

		public MemberItems GetMemberItems(int userId, int contextOrganizationId, MemberFilter filter)
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

			return ExecuteMultiResultSetSp<MemberItems>("[acc].[spGetMemberItems]", parameters);
		}

		public ICollection<SelectListItemCount> GetMembers(int userId, int contextOrganizationId)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId
			};

			return ExecuteSp<SelectListItemCount>("[acc].[spGetMembers]", parameters);
		}
	}
}