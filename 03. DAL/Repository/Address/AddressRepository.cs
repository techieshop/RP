using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.DAL.Repository
{
	public class AddressRepository : BaseRepository<EntityDbContext>, IAddressRepository
	{
		public AddressRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public ICollection<Address> GetAddresses(List<int> addressIds)
		{
			return GetQuery<Address>().Where(a => addressIds.Contains(a.Id)).ToList();
		}
	}
}