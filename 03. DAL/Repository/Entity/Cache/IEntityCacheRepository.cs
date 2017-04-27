using RP.Common.Manager;
using RP.DAL.DBContext;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	[Cache(DurationInSec = CacheManager.InfiniteDuration)]
	public interface IEntityCacheRepository : IRepository<EntityDbContext>
	{
		int? GetEntityStateNameCode(int entityStateId);

		IDictionary<int, EntityState> GetEntityStates();

		IDictionary<int, EntityStateTransition> GetEntityStateTransitions();

		IDictionary<int, EntityType> GetEntityTypes();

		IDictionary<int, ICollection<Model.EntityState>> GetEntityTypeStates();

		IDictionary<int, ICollection<EntityStateTransition>> GetEntityTypeStateTransitions();
	}
}