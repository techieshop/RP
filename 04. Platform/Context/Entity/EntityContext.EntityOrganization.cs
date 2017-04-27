using RP.Model;
using System.Linq;

namespace RP.Platform.Context
{
	public partial class EntityContext
	{
		public void AddEntityOrganization(EntityInfo entityInfo, int organizationId, int entityStateId)
		{
			entityInfo.EntityOrganizations.Add(new EntityOrganization
			{
				EntityTypeId = entityInfo.EntityTypeId,
				OrganizationId = organizationId,
				EntityStateId = entityStateId
			});
		}

		public void AddEntityOrganization(EntityInfo entityInfo, Organization organization, int entityStateId)
		{
			entityInfo.EntityOrganizations.Add(new EntityOrganization
			{
				EntityTypeId = entityInfo.EntityTypeId,
				Organization = organization,
				EntityStateId = entityStateId
			});
		}

		public EntityOrganization GetEntityOrganization(EntityInfo entityInfo, int? organizationId = null)
		{
			return
				entityInfo.EntityOrganizations.SingleOrDefault(t => organizationId == null || t.OrganizationId == organizationId);
		}
	}
}