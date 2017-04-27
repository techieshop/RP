using RP.Common.Extension;
using RP.DAL.Repository;
using RP.Model;
using RP.Platform.Manager;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RP.Platform.Context
{
	public class UserContext : IUserContext
	{
		private readonly IRegistrationRepository _registrationRepository;
		private readonly IEntityRepository _entityRepository;
		private readonly IOrganizationRepository _organizationRepository;
		private readonly IUserRepository _userRepository;

		private IDictionary<int, IDictionary<int, OrganizationRelationRef>> _organizationRelations;

		public UserContext(
			IEntityRepository entityRepository,
			IOrganizationRepository organizationRepository,
			IRegistrationRepository registrationRepository,
			IUserRepository userRepository)
		{
			_entityRepository = entityRepository;
			_organizationRepository = organizationRepository;
			_registrationRepository = registrationRepository;
			_userRepository = userRepository;

			int? userId = AuthManager.GetUserId();
			UserAuth userAuth = _userRepository.GetUserAuth(userId);

			if (userId == null || userAuth.UserData == null || userAuth.EntityStateAccess.IsNullOrEmpty())
			{
				AuthManager.SignOut();
			}
			else
			{
				User = userAuth.UserData;
				EntityStateAccess = userAuth.EntityStateAccess;
				EntityStateTransitionAccess = userAuth.EntityStateTransitionAccess;
				AuthManager.RenewAuthCookie();
			}

			InitUserAuth(userAuth);
		}

		public static IUserContext Current => DependencyResolver.Current.GetService<IUserContext>();

		//Statement
		public ICollection<Registration> GetRegistrations(bool onlyNew = false)
		{
			return _registrationRepository.GetRegistrations(onlyNew);
		}

		public UserData User { get; }

		public bool IsAuthenticated => User != null && !EntityStateAccess.IsNullOrEmpty();

		public IDictionary<int, EntityTypeAccessExt> EntityTypeAccess { get; private set; }

		public ICollection<EntityStateAccess> EntityStateAccess { get; }

		public ICollection<EntityStateTransitionAccess> EntityStateTransitionAccess { get; }

		public bool HasAccess(int entityTypeId)
		{
			return HasAccess(new Access
			{
				EntityTypeId = entityTypeId
			});
		}

		public bool HasAccess(int entityTypeId, int accessTypeId)
		{
			return HasAccess(new Access
			{
				EntityTypeId = entityTypeId,
				AccessTypeId = accessTypeId
			});
		}

		public bool HasStateAccess(int entityTypeId, int entityStateId)
		{
			return HasAccess(new Access
			{
				EntityTypeId = entityTypeId,
				EntityStateId = entityStateId
			});
		}

		public bool HasStateAccess(int entityTypeId, int entityStateId, int accessTypeId)
		{
			return HasAccess(new Access
			{
				EntityTypeId = entityTypeId,
				EntityStateId = entityStateId,
				AccessTypeId = accessTypeId
			});
		}

		public bool HasStateTransitionAccess(int entityTypeId, int entityStateTransitionId)
		{
			return HasAccess(new Access
			{
				EntityTypeId = entityTypeId,
				EntityStateTransitionId = entityStateTransitionId
			});
		}

		public bool HasOrganizationAccess(int organizationId)
		{
			return HasAccess(new Access
			{
				OrganizationId = organizationId
			});
		}

		public bool HasOrganizationAccess(int organizationId, int entityTypeId)
		{
			return HasAccess(new Access
			{
				OrganizationId = organizationId,
				EntityTypeId = entityTypeId
			});
		}

		public bool HasOrganizationAccess(int organizationId, int entityTypeId, int accessTypeId)
		{
			return HasAccess(new Access
			{
				OrganizationId = organizationId,
				EntityTypeId = entityTypeId,
				AccessTypeId = accessTypeId
			});
		}

		public bool HasOrganizationStateAccess(int organizationId, int entityTypeId, int entityStateId)
		{
			return HasAccess(new Access
			{
				OrganizationId = organizationId,
				EntityTypeId = entityTypeId,
				EntityStateId = entityStateId
			});
		}

		public bool HasOrganizationStateAccess(int organizationId, int entityTypeId, int entityStateId, int accessTypeId)
		{
			return HasAccess(new Access
			{
				OrganizationId = organizationId,
				EntityTypeId = entityTypeId,
				EntityStateId = entityStateId,
				AccessTypeId = accessTypeId
			});
		}

		public bool HasOrganizationStateTransitionAccess(int organizationId, int entityTypeId, int entityStateTransitionId)
		{
			return HasAccess(new Access
			{
				OrganizationId = organizationId,
				EntityTypeId = entityTypeId,
				EntityStateTransitionId = entityStateTransitionId
			});
		}

		public bool HasAccess(Access access)
		{
			if (access.EntityTypeId == null && access.OrganizationId != null && IsAuthenticated)
			{
				InitOrganizationRelations(access.OrganizationId.Value);
				if (_organizationRelations[access.OrganizationId.Value] != null)
					access.EntityTypeId = _organizationRelations[access.OrganizationId.Value][access.OrganizationId.Value].EntityTypeId;
			}
			if (access.EntityTypeId != null)
			{
				access.AccessTypeId = access.AccessTypeId ?? Dom.AccessType.Read;
				if (IsAuthenticated)
				{
					if (HasAccess(access, EntityTypeAccess))
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool HasAccess(Access access, IDictionary<int, EntityTypeAccessExt> entityTypeAccess)
		{
			if (access.EntityTypeId == null)
				return false;

			if (!entityTypeAccess.ContainsKey(access.EntityTypeId.Value))
				return false;

			EntityTypeAccessExt entityTypeAccessExt = entityTypeAccess[access.EntityTypeId.Value];
			if (access.EntityStateId != null)
			{
				return entityTypeAccessExt.EntityStateAccess.ContainsKey(access.EntityStateId.Value)
					&& entityTypeAccessExt.EntityStateAccess[access.EntityStateId.Value] >= access.AccessTypeId;
			}
			if (access.EntityStateTransitionId != null)
			{
				EntityStateTransition transition = _entityRepository.GetEntityStateTransition(access.EntityStateTransitionId.Value);
				if (transition == null)
					return false;

				int entityStateId = transition.EntityStateFromId ?? transition.EntityStateToId;
				return entityTypeAccessExt.EntityStateTransitionAccess.Contains(transition.Id)
					&& entityTypeAccessExt.EntityStateAccess.ContainsKey(entityStateId)
					&& entityTypeAccessExt.EntityStateAccess[entityStateId] == Dom.AccessType.ReadWrite;
			}
			if (access.AccessTypeId == Dom.AccessType.Create)
			{
				if (entityTypeAccessExt.AccessTypeId >= Dom.AccessType.ReadWrite)
				{
					IEnumerable<EntityStateTransition> entityStateTransitions = _entityRepository.GetInitialEntityStateTransitions(access.EntityTypeId.Value);
					foreach (EntityStateTransition entityStateTransition in entityStateTransitions)
					{
						if (entityTypeAccessExt.EntityStateTransitionAccess.Contains(entityStateTransition.Id)
							&& entityTypeAccessExt.EntityStateAccess.ContainsKey(entityStateTransition.EntityStateToId)
							&& entityTypeAccessExt.EntityStateAccess[entityStateTransition.EntityStateToId] == Dom.AccessType.ReadWrite)

							return true;
					}
				}
			}
			else if (entityTypeAccessExt.AccessTypeId >= access.AccessTypeId)
			{
				return true;
			}
			return false;
		}

		private void InitUserAuth(UserAuth userAuth)
		{
			if (!userAuth.EntityStateAccess.IsNullOrEmpty())
			{
				EntityTypeAccess = new Dictionary<int, EntityTypeAccessExt>();

				foreach (var userAccess in userAuth.EntityStateAccess)
				{
					if (!EntityTypeAccess.ContainsKey(userAccess.EntityTypeId))
					{
						var accessTypeId = userAuth.EntityTypeAccess.FirstOrDefault(t => t.EntityTypeId == userAccess.EntityTypeId)?.AccessTypeId;
						EntityTypeAccess.Add(userAccess.EntityTypeId, new EntityTypeAccessExt
						{
							AccessTypeId = accessTypeId ?? Dom.AccessType.Undefined,
							EntityStateAccess = new Dictionary<int, int>(),
							EntityStateTransitionAccess = new HashSet<int>()
						});
					}

					if (!EntityTypeAccess[userAccess.EntityTypeId].EntityStateAccess.ContainsKey(userAccess.EntityStateId))
						EntityTypeAccess[userAccess.EntityTypeId].EntityStateAccess.Add(userAccess.EntityStateId, Dom.AccessType.Undefined);
					if (EntityTypeAccess[userAccess.EntityTypeId].EntityStateAccess[userAccess.EntityStateId] < userAccess.AccessTypeId)
						EntityTypeAccess[userAccess.EntityTypeId].EntityStateAccess[userAccess.EntityStateId] = userAccess.AccessTypeId;
				}

				foreach (var userAccess in userAuth.EntityStateTransitionAccess)
				{
					EntityTypeAccess[userAccess.EntityTypeId].EntityStateTransitionAccess.Add(userAccess.EntityStateTransitionId);
				}

				_organizationRelations = new Dictionary<int, IDictionary<int, OrganizationRelationRef>>();
			}
		}

		private void InitOrganizationRelations(int organizationId)
		{
			if (!_organizationRelations.ContainsKey(organizationId))
			{
				IList<OrganizationRelationRef> relations = _organizationRepository.GetOrganizationRelationRef(organizationId);
				if (relations.Count > 0)
				{
					InitOrganizationRelations(relations);
				}
				else
				{
					_organizationRelations.Add(organizationId, null);
				}
			}
		}

		private void InitOrganizationRelations(IList<OrganizationRelationRef> relations)
		{
			for (int i = 0; i < relations.Count; i++)
			{
				if (_organizationRelations.ContainsKey(relations[i].RelatedOrganizationId))
					break;

				IDictionary<int, OrganizationRelationRef> organizationRelations = new Dictionary<int, OrganizationRelationRef>();
				for (int j = i; j < relations.Count; j++)
				{
					OrganizationRelationRef relation;
					if (i == 0)
					{
						relation = relations[j];
					}
					else
					{
						relation = new OrganizationRelationRef
						{
							OrganizationId = relations[i].RelatedOrganizationId,
							RelatedOrganizationId = relations[j].RelatedOrganizationId,
							EntityTypeId = relations[j].EntityTypeId
						};
					}
					organizationRelations.Add(relations[j].RelatedOrganizationId, relation);
				}
				_organizationRelations.Add(relations[i].RelatedOrganizationId, organizationRelations);
			}
		}
	}
}