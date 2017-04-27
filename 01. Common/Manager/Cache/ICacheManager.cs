using System.Collections.Generic;

namespace RP.Common.Manager
{
	public interface ICacheManager
	{
		void FlushAll();

		void FlushCategories(IEnumerable<string> categories);

		void FlushCategory(string category);

		object Get(string category, string key);

		TEntity Get<TEntity>(string category, string key) where TEntity : class;

		void Remove(string category, string key);

		void Set(string category, string key, object value, int durationInSec);
	}
}