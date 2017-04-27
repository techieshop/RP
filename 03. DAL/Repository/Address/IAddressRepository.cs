using RP.DAL.DBContext;
using RP.Model;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public interface IAddressRepository : IRepository<EntityDbContext>
	{
		ICollection<Address> GetAddresses(List<int> addressIds);
	}
}