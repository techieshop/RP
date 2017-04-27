using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.DAL.Repository
{
	public partial class DomainValueRepository
	{
		public IDictionary<int, DomainValue> GetSexs()
		{
			return GetQuery<DomainValue>()
				.AsEnumerable()
				.ToDictionary(t => t.Id);
		}

		public DomainValue GetSex(int sexId)
		{
			return GetQuery<DomainValue>().SingleOrDefault(t => t.Id == sexId);
		}
	}
}