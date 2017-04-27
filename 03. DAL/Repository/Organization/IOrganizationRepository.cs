using RP.DAL.DBContext;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public interface IOrganizationRepository : IRepository<EntityDbContext>
	{
		OrganizationDetails GetOrganizationDetails(int userId, int contextOrganizationId, int id);

		OrganizationItems GetOrganizationItems(int userId, int contextOrganizationId, OrganizationFilter filter);

		ICollection<OrganizationRelation> GetOrganizationRelation(int organizationId);

		IList<OrganizationRelationRef> GetOrganizationRelationRef(int organizationId);

		ICollection<SelectListItemCount> GetOrganizations(int userId, int contextOrganizationId, ICollection<int> organizationTypeIds = null);
	}
}