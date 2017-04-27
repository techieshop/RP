using RP.Common.Manager;
using RP.DAL.DBContext;
using System.Collections.Generic;

namespace RP.DAL.Repository
{
	public interface ITranslationRepository : IRepository<EntityDbContext>
	{
		[Cache(Category = CacheManager.CategoryName.Translation, DurationInSec = CacheManager.InfiniteDuration)]
		IDictionary<int, string> GetTranslations(int languageId);
	}
}