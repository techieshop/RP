using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.DAL.Repository
{
	public partial class EntityRepository
	{
		public ICollection<EntityStateTransition> GetInitialEntityStateTransitions(int entityTypeId)
		{
			ICollection<EntityStateTransition> entityStateTransitions;
			_entityCacheRepository.GetEntityTypeStateTransitions().TryGetValue(entityTypeId, out entityStateTransitions);
			return entityStateTransitions.Where(t => t.EntityStateFromId == null).ToList();
		}

		public EntityStateTransition GetEntityStateTransition(int id)
		{
			return GetQuery<EntityStateTransition>().SingleOrDefault(t => t.Id == id);
		}

		public EntityStateTransition GetEntityStateTransition(int entityTypeId, int? entityStateFromId, int entityStateToId)
		{
			ICollection<EntityStateTransition> entityStateTransitions;
			_entityCacheRepository.GetEntityTypeStateTransitions().TryGetValue(entityTypeId, out entityStateTransitions);

			return entityStateTransitions.SingleOrDefault(x => x.EntityStateFromId == entityStateFromId && x.EntityStateToId == entityStateToId);
		}
	}
}