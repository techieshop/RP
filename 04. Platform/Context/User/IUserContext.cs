using RP.Model;
using System.Collections.Generic;

namespace RP.Platform.Context
{
	public interface IUserContext
	{
		//Statement
		ICollection<Registration> GetRegistrations(bool onlyNew = false);

		UserData User { get; }
		bool IsAuthenticated { get; }
		IDictionary<int, EntityTypeAccessExt> EntityTypeAccess { get; }
		ICollection<EntityStateAccess> EntityStateAccess { get; }
		ICollection<EntityStateTransitionAccess> EntityStateTransitionAccess { get; }

		bool HasAccess(Access access);

		bool HasAccess(int entityTypeId);

		bool HasAccess(int entityTypeId, int accessTypeId);

		bool HasStateAccess(int entityTypeId, int entityStateId);

		bool HasStateAccess(int entityTypeId, int entityStateId, int accessTypeId);

		bool HasStateTransitionAccess(int entityTypeId, int entityStateTransitionId);

		bool HasOrganizationAccess(int organizationId);

		bool HasOrganizationAccess(int organizationId, int entityTypeId);

		bool HasOrganizationAccess(int organizationId, int entityTypeId, int accessTypeId);

		bool HasOrganizationStateAccess(int organizationId, int entityTypeId, int entityStateId);

		bool HasOrganizationStateAccess(int organizationId, int entityTypeId, int entityStateId, int accessTypeId);

		bool HasOrganizationStateTransitionAccess(int organizationId, int entityTypeId, int entityStateTransitionId);
	}
}