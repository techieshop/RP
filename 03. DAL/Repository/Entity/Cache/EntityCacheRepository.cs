using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RP.DAL.Repository
{
	public class EntityCacheRepository : BaseRepository<EntityDbContext>, IEntityCacheRepository
	{
		public EntityCacheRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public IDictionary<int, EntityType> GetEntityTypes()
		{
			return GetQuery<EntityType>()
				.AsEnumerable()
				.ToDictionary(t => t.Id);
		}

		public int? GetEntityStateNameCode(int entityStateId)
		{
			var entityState = GetQuery<Model.EntityState>()
				.FirstOrDefault(t => t.Id == entityStateId);
			return entityState?.NameCode;
		}

		public IDictionary<int, Model.EntityState> GetEntityStates()
		{
			return GetQuery<Model.EntityState>()
				.Include(t => t.TransitionsFrom)
				.Include(t => t.TransitionsTo)
				.AsEnumerable()
				.ToDictionary(t => t.Id, t => t);
		}

		public IDictionary<int, ICollection<Model.EntityState>> GetEntityTypeStates()
		{
			return GetQuery<EntityType>()
				.Include(t => t.EntityStates)
				.AsEnumerable()
				.ToDictionary(t => t.Id, t => t.EntityStates);
		}

		public IDictionary<int, EntityStateTransition> GetEntityStateTransitions()
		{
			return GetQuery<EntityStateTransition>()
				.Include(t => t.EntityStateFrom)
				.Include(t => t.EntityStateTo)
				.AsEnumerable()
				.ToDictionary(t => t.Id, t => t);
		}

		public IDictionary<int, ICollection<EntityStateTransition>> GetEntityTypeStateTransitions()
		{
			var items = GetQuery<EntityStateTransition>().Include(t => t.EntityStateTo);
			var result = new Dictionary<int, ICollection<EntityStateTransition>>();
			foreach (var item in items.Select(t => t.EntityStateTo.EntityTypeId).Distinct())
			{
				result.Add(item, items.Where(t => t.EntityStateTo.EntityTypeId == item).ToList());
			}
			return result;
		}
	}
}