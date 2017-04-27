using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.DAL.Repository
{
	public class RegistrationRepository : BaseRepository<EntityDbContext>, IRegistrationRepository
	{
		public RegistrationRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public ICollection<Registration> GetRegistrations(bool onlyNew = false)
		{
			if (onlyNew)
			{
				return GetQuery<Registration>()
					.Where(r => !r.IsRead)
					.OrderByDescending(m => m.DateTimeRequest)
					.ToList();
			}
			else
			{
				return GetQuery<Registration>()
					.OrderByDescending(m => m.DateTimeRequest)
					.ToList();
			}
		}
	}
}