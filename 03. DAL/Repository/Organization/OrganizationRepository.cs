using RP.Common.Extension;
using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.DAL.Repository
{
	public class OrganizationRepository : BaseRepository<EntityDbContext>, IOrganizationRepository
	{
		public OrganizationRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public OrganizationDetails GetOrganizationDetails(int userId, int contextOrganizationId, int id)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				organizationId = id
			};

			return ExecuteMultiResultSetSp<OrganizationDetails>("[acc].[spGetOrganizationDetails]", parameters);
		}

		public OrganizationItems GetOrganizationItems(int userId, int contextOrganizationId, OrganizationFilter filter)
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

			return ExecuteMultiResultSetSp<OrganizationItems>("[acc].[spGetOrganizationItems]", parameters);
		}

		public ICollection<OrganizationRelation> GetOrganizationRelation(int organizationId)
		{
			return GetQuery<OrganizationRelation>()
				.Where(t => t.OrganizationId == organizationId)
				.OrderBy(t => t.Order)
				.ToList();
		}

		public IList<OrganizationRelationRef> GetOrganizationRelationRef(int organizationId)
		{
			var parameters = new
			{
				organizationId
			};
			return ExecuteSp<OrganizationRelationRef>("[acc].[spGetOrganizationRelationRef]", parameters);
		}

		public ICollection<SelectListItemCount> GetOrganizations(int userId, int contextOrganizationId, ICollection<int> organizationTypeIds = null)
		{
			var parameters = new
			{
				userId,
				contextOrganizationId,
				OrganizationTypeIds = organizationTypeIds.ToKeyList()
			};

			return ExecuteSp<SelectListItemCount>("[acc].[spGetOrganizations]", parameters);
		}
	}
}