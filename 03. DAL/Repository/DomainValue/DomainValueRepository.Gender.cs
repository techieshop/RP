using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.DAL.Repository
{
	public partial class DomainValueRepository
	{
		public IDictionary<int, DomainValue> GetGenders()
		{
			return GetQuery<DomainValue>()
				.AsEnumerable()
				.ToDictionary(t => t.Id);
		}

		public DomainValue GetGender(int genderId)
		{
			return GetQuery<DomainValue>().SingleOrDefault(t => t.Id == genderId);
		}
	}
}