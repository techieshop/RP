using RP.DAL.DBContext;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public interface IRegistrationRepository : IRepository<EntityDbContext>
	{
		ICollection<Registration> GetRegistrations(bool onlyNew = false);
	}
}