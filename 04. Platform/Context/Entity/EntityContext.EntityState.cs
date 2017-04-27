using System;
using RP.Model;
using RP.Platform.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace RP.Platform.Context
{
	public partial class EntityContext
	{
		public EntityStateTransition ChangeEntityState(int entityInfoId, int entityStateTransitionId)
		{
			EntityInfo entityInfo = _entityRepository.GetEntityInfo(entityInfoId, t => t.EntityOrganizations, t => t.EntityProgress);
			return ChangeEntityState(entityInfo, entityStateTransitionId);
		}

		public EntityStateTransition ChangeEntityState(EntityInfo entityInfo, int entityStateTransitionId)
		{
			return ChangeEntityState(entityInfo, entityStateTransitionId, null, null);
		}

		public EntityStateTransition ChangeEntityState(EntityInfo entityInfo, int entityStateTransitionId, int? organizationId)
		{
			return ChangeEntityState(entityInfo, entityStateTransitionId, organizationId, null);
		}

		public IDictionary<int, string> GetAccesibleEntityTransitionsFrom(int entityStateId, int entityTypeId, int organizationId)
		{
			EntityState entityState = _entityRepository.GetEntityState(entityStateId);
			return entityState.TransitionsFrom
				.Where(t => _userContext.HasOrganizationStateTransitionAccess(organizationId, entityTypeId, t.Id))
				.OrderBy(t => t.Order)
				.ToDictionary(t => t.Id, t => _styleContext.GetTranslation(t.ActionBeforeCode));
		}

		public bool TryChangeEntityState(int entityInfoId, int organizationId, int entityTransitionId, int? userId = null)
		{
			EntityOrganization entityOrganization = _entityRepository.GetEntityOrganization(entityInfoId, organizationId);
			return TryChangeEntityState(entityOrganization, entityTransitionId, userId);
		}

		public bool TryChangeEntityState(EntityOrganization entityOrganization, int entityTransitionId, int? userId = null)
		{
			EntityStateTransition entityTransition = _entityRepository.GetEntityStateTransition(entityTransitionId);

			if (entityOrganization == null
				|| entityTransition == null
				|| entityTransition.EntityStateFromId != entityOrganization.EntityStateId
				|| !_userContext.HasOrganizationStateTransitionAccess(entityOrganization.OrganizationId, entityOrganization.EntityTypeId, entityTransition.Id))
			{
				return false;
			}

			entityOrganization.EntityStateId = entityTransition.EntityStateToId;

			if (userId == null && _userContext.IsAuthenticated)
				userId = _userContext.User.Id;

			_entityRepository.Add(new EntityProgress
			{
				EntityInfoId = entityOrganization.EntityInfoId,
				OrganizationId = entityOrganization.OrganizationId,
				UserId = userId,
				DateTime = DateTime.UtcNow,
				EntityStateBeforeId = entityTransition.EntityStateFromId,
				EntityStateAfterId = entityTransition.EntityStateToId
			});

			return true;
		}

		public bool TryChangeEntityState(int entityInfoId, int entityOrganizationId, int entityTransitionId, out EntityStateChangeViewModel entityStateChangeViewModel)
		{
			if (!TryChangeEntityState(entityInfoId, entityOrganizationId, entityTransitionId))
			{
				entityStateChangeViewModel = null;
				return false;
			}

			EntityStateTransition entityStateTransition = _entityRepository.GetEntityStateTransition(entityTransitionId);
			EntityOrganization entityOrganization = _entityRepository.GetEntityOrganization(entityInfoId, entityOrganizationId);

			entityStateChangeViewModel = new EntityStateChangeViewModel
			{
				EntityInfoId = entityOrganization.EntityInfoId,
				OrganizationId = entityOrganization.OrganizationId,
				EntityTransitions = GetAccesibleEntityTransitionsFrom(entityStateTransition.EntityStateToId, entityOrganization.EntityTypeId, entityOrganization.OrganizationId),
				EntityStateChanged = new EntityStateChangedViewModel
				{
					EntityStateId = entityStateTransition.EntityStateToId,
					EntityStateName = _styleContext.GetTranslation(entityStateTransition.EntityStateTo.NameCode)
				}
			};

			_entityRepository.UnitOfWork.SaveChanges();
			return true;
		}

		private EntityStateTransition ChangeEntityState(EntityInfo entityInfo, int entityStateTransitionId, int? organizationId, int? userId)
		{
			EntityStateTransition actualTransition = null;
			EntityOrganization entityOrganization = GetEntityOrganization(entityInfo, organizationId);
			if (entityOrganization != null)
			{
				EntityState entityState = _entityRepository.GetEntityState(entityOrganization.EntityStateId);
				EntityStateTransition transition = entityState.TransitionsFrom.SingleOrDefault(t => t.EntityStateToId == entityStateTransitionId);
				if (userId == null && _userContext.IsAuthenticated)
					userId = _userContext.User.Id;
				if (transition != null)
				{
					AddEntityProgress(
						entityInfo,
						new EntityProgress
						{
							UserId = userId,
							DateTime = DateTime.UtcNow,
							OrganizationId = entityOrganization.OrganizationId,
							EntityStateBeforeId = transition.EntityStateFromId,
							EntityStateAfterId = transition.EntityStateToId
						}
					);
					entityOrganization.EntityStateId = transition.EntityStateToId;

					actualTransition = transition;
				}
			}
			return actualTransition;
		}
	}
}