using RP.Model;
using System.Linq;

namespace RP.DAL.Repository
{
	public partial class EntityRepository
	{
		public EntityType GetEntityType(int id)
		{
			return GetQuery<EntityType>().SingleOrDefault(t => t.Id == id);
		}
	}
}