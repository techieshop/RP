using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace RP.Common.Manager
{
	public class CacheManager : ICacheManager, IDisposable
	{
		public const int InfiniteDuration = -1;

		private readonly ConcurrentDictionary<string, MemoryCache> _categoryCaches;

		public CacheManager()
		{
			_categoryCaches = new ConcurrentDictionary<string, MemoryCache>();
		}

		public void Dispose()
		{
			FlushAll();
		}

		public void FlushAll()
		{
			ICollection<string> categories = _categoryCaches.Keys;
			foreach (var category in categories)
				FlushCategory(category);
		}

		public void FlushCategories(IEnumerable<string> categories)
		{
			if (categories != null)
			{
				foreach (string category in categories)
					FlushCategory(category);
			}
		}

		public void FlushCategory(string category)
		{
			MemoryCache categoryCache;
			if (_categoryCaches.TryGetValue(category, out categoryCache))
			{
				categoryCache.Dispose();
				_categoryCaches.TryRemove(category, out categoryCache);
			}
		}

		public object Get(string category, string key)
		{
			return GetCacheByCategory(category).Get(key);
		}

		public TEntity Get<TEntity>(string category, string key) where TEntity : class
		{
			return GetCacheByCategory(category).Get(key) as TEntity;
		}

		public void Remove(string category, string key)
		{
			GetCacheByCategory(category).Remove(key);
		}

		public void Set(string category, string key, object value, int durationInSec)
		{
			DateTimeOffset absoluteExpiration;
			if (durationInSec == InfiniteDuration)
			{
				absoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
			}
			else
			{
				TimeSpan duration = TimeSpan.FromSeconds(durationInSec);
				absoluteExpiration = new DateTimeOffset(DateTime.Now.Add(duration));
			}
			GetCacheByCategory(category).Set(key, value, absoluteExpiration);
		}

		private MemoryCache GetCacheByCategory(string category)
		{
			MemoryCache categoryCache;
			if (!_categoryCaches.TryGetValue(category, out categoryCache))
			{
				categoryCache = new MemoryCache(category);
				_categoryCaches.TryAdd(category, categoryCache);
			}
			return categoryCache;
		}

		public static class CategoryName
		{
			public const string Default = "DEFAULT_CACHE";
			public const string DomainValue = "DOMAIN_VALUE_CACHE";
			public const string Translation = "TRANSLATION_CACHE";
		}
	}
}