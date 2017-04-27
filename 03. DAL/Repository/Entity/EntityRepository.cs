using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;

namespace RP.DAL.Repository
{
	public partial class EntityRepository : BaseRepository<EntityDbContext>, IEntityRepository
	{
		private readonly IEntityCacheRepository _entityCacheRepository;

		public EntityRepository(
			IEntityCacheRepository entityCacheRepository,
			IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
			_entityCacheRepository = entityCacheRepository;
		}
	}
}