using RP.Model;
using RP.Platform.ViewModel;
using System.Collections.Generic;

namespace RP.Platform.Context
{
	public interface IEntityContext
	{
		void AddEntityOrganization(EntityInfo entityInfo, int organizationId, int entityStateId);

		void AddEntityOrganization(EntityInfo entityInfo, Organization organization, int entityStateId);

		void AddEntityProgress(EntityInfo entityInfo, EntityProgress entityProgress);

		EntityStateTransition ChangeEntityState(int entityInfoId, int entityStateTransitionId);

		EntityStateTransition ChangeEntityState(EntityInfo entityInfo, int entityStateTransitionId);

		EntityStateTransition ChangeEntityState(EntityInfo entityInfo, int entityStateTransitionId, int? organizationId);

		IDictionary<int, string> GetAccesibleEntityTransitionsFrom(int entityStateId, int entityTypeId, int organizationId);

		EntityOrganization GetEntityOrganization(EntityInfo entityInfo, int? organizationId = null);

		bool TryChangeEntityState(int entityInfoId, int entityOrganizationId, int entityTransitionId, out EntityStateChangeViewModel entityStateChangeViewModel);
	}
}