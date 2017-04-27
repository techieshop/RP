using System.Data.Entity;
using System.Linq;
using EntityState = RP.Model.EntityState;

namespace RP.DAL.Repository
{
	public partial class EntityRepository
	{
		public EntityState GetEntityState(int id)
		{
			EntityState entityState;
			GetQuery<EntityState>()
			.Include(t => t.TransitionsFrom)
			.Include(t => t.TransitionsTo)
			.AsEnumerable()
			.ToDictionary(t => t.Id, t => t)
			.TryGetValue(id, out entityState);
			return entityState;
		}
	}
}