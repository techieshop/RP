using RP.DAL.DBContext;
using RP.Model;
using System.Collections.Generic;
using RP.Common.Manager;

namespace RP.DAL.Repository
{
	[Cache(Category = CacheManager.CategoryName.DomainValue, DurationInSec = CacheManager.InfiniteDuration)]
	public interface IDomainValueRepository : IRepository<EntityDbContext>
	{
		IDictionary<int, Country> GetCountries();

		Country GetCountry(int contryId);

		Country GetCountry(string contryCode);

		DomainValue GetGender(int genderId);

		IDictionary<int, DomainValue> GetGenders();

		DomainValue GetLanguage(int languageId);

		IDictionary<int, DomainValue> GetLanguages();

		DomainValue GetSex(int sexId);

		IDictionary<int, DomainValue> GetSexs();
	}
}