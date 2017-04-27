using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;

namespace RP.DAL.Repository
{
	public partial class DomainValueRepository : BaseRepository<EntityDbContext>, IDomainValueRepository
	{
		public DomainValueRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}
	}
}