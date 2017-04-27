using RP.DAL.DBContext;
using RP.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RP.DAL.Repository
{
	public interface IEntityRepository : IRepository<EntityDbContext>
	{
		EntityInfo GetEntityInfo(int id, params Expression<Func<EntityInfo, object>>[] includes);

		EntityOrganization GetEntityOrganization(int entityInfoId, int organizationId);

		ICollection<EntityOrganization> GetEntityOrganizationByOrganizationId(int organizationId);

		EntityState GetEntityState(int id);

		EntityStateTransition GetEntityStateTransition(int id);

		EntityType GetEntityType(int id);

		ICollection<EntityStateTransition> GetInitialEntityStateTransitions(int entityTypeId);
	}
}