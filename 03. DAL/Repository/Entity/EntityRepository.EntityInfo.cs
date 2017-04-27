using RP.Common.Extension;
using RP.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace RP.DAL.Repository
{
	public partial class EntityRepository
	{
		public EntityInfo GetEntityInfo(int id, params Expression<Func<EntityInfo, object>>[] includes)
		{
			return GetQuery<EntityInfo>()
				.Where(t => t.Id == id)
				.IncludeMany(includes)
				.SingleOrDefault();
		}
	}
}