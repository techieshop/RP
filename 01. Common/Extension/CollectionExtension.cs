using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;

namespace RP.Common.Extension
{
	public static class CollectionExtensions
	{
		public static bool Any(this IEnumerable source)
		{
			bool any = false;
			if (source != null)
			{
				IEnumerator enumerator = source.GetEnumerator();
				if (enumerator.MoveNext())
					any = true;
			}
			return any;
		}

		public static object First(this IEnumerable source)
		{
			object result = null;
			if (source != null)
			{
				IEnumerator enumerator = source.GetEnumerator();
				if (enumerator.MoveNext())
					result = enumerator.Current;
			}
			return result;
		}

		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			if (collection != null)
			{
				foreach (var item in collection)
				{
					action(item);
				}
			}
		}

		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			TValue value;
			if (dictionary != null)
				dictionary.TryGetValue(key, out value);
			else
				value = default(TValue);

			return value;
		}

		public static ICollection<TValue> GetValues<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
		{
			ICollection<TValue> values = null;
			if (dictionary != null)
				values = dictionary.Values;

			return values;
		}

		public static bool IsCollection(this Type type)
		{
			return !type.IsAssignableFrom(typeof(String))
				&& type.GetInterfaces().Contains(typeof(IEnumerable));
		}

		public static bool IsCollection(this PropertyInfo property)
		{
			return property.PropertyType.IsCollection();
		}

		public static bool IsFirst<T>(this IEnumerable<T> items, T item) where T : class
		{
			var first = items.FirstOrDefault();
			if (first == null)
				return false;

			return item.Equals(first);
		}

		public static bool IsLast<T>(this IEnumerable<T> items, T item) where T : class
		{
			var last = items.LastOrDefault();
			if (last == null)
				return false;

			return item.Equals(last);
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
		{
			return collection == null || !collection.Any();
		}

		public static string Join(this IEnumerable<string> source, string separator)
		{
			return Format.Join(separator, source);
		}

		public static string Join<TItem>(this IEnumerable<TItem> source, string separator)
		{
			string result = null;
			var enumerable = source as List<TItem> ?? source.ToList();
			if (!enumerable.IsNullOrEmpty())
			{
				result = string.Join(separator, enumerable);
			}
			return result;
		}

		public static string ToQueryString(this NameValueCollection source)
		{
			return String.Join("&", (from string name in source select String.Concat(name, "=", HttpUtility.UrlEncode(source[name]))).ToArray());
		}
	}
}