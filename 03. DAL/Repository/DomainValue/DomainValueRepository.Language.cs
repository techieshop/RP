using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.DAL.Repository
{
	public partial class DomainValueRepository
	{
		public IDictionary<int, DomainValue> GetLanguages()
		{
			return GetQuery<DomainValue>()
				.AsEnumerable()
				.ToDictionary(t => t.Id);
		}

		public DomainValue GetLanguage(int languageId)
		{
			return GetQuery<DomainValue>().SingleOrDefault(t => t.Id == languageId);
		}
	}
}