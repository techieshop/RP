using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RP.Common.Extension
{
	public static class QueryableExtensions
	{
		public static IQueryable<TEntity> IncludeMany<TEntity>(this IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
		{
			if (!includes.IsNullOrEmpty())
				query = includes.Aggregate(query, (q, i) => q.Include(i));
			return query;
		}
	}
}