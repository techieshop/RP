using RP.Model;
using System.Collections.Generic;
using System.Linq;

namespace RP.DAL.Repository
{
	public partial class DomainValueRepository
	{
		public IDictionary<int, Country> GetCountries()
		{
			return GetQuery<Country>()
				.AsEnumerable()
				.ToDictionary(t => t.Id);
		}

		public Country GetCountry(int countrtyId)
		{
			return GetQuery<Country>().SingleOrDefault(t => t.Id == countrtyId);
		}

		public Country GetCountry(string contryCode)
		{
			return GetQuery<Country>().SingleOrDefault(t => t.Code == contryCode.ToUpper());
		}
	}
}