using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.DAL.Repository
{
	public partial class EntityRepository
	{
		public ICollection<EntityOrganization> GetEntityOrganizationByOrganizationId(int organizationId)
		{
			return GetQuery<EntityOrganization>()
				.Where(t => t.OrganizationId == organizationId)
				.ToList();
		}

		public EntityOrganization GetEntityOrganization(int entityInfoId, int organizationId)
		{
			return GetQuery<EntityOrganization>()
				.FirstOrDefault(t => t.EntityInfoId == entityInfoId && t.OrganizationId == organizationId);
		}
	}
}