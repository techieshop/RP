using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;
using System;
using System.Linq.Expressions;

namespace RP.DAL.Repository
{
	public interface IRepository<TContext>
		where TContext : BaseDbContext<TContext>
	{
		TEntity Get<TEntity>(int id) where TEntity : BaseModel;

		TEntity GetEntity<TEntity>(int id, params Expression<Func<TEntity, object>>[] includes) where TEntity : BaseEntity;

		TEntity GetEntityByEntityInfoId<TEntity>(int entityInfoId, params Expression<Func<TEntity, object>>[] includes) where TEntity : BaseEntity;

		void Add<TEntity>(TEntity entity) where TEntity : class;

		void AddOrUpdate<TEntity>(TEntity entity) where TEntity : BaseModel;

		void Delete<TEntity>(TEntity entity) where TEntity : class;

		IUnitOfWork<TContext> UnitOfWork { get; }
	}
}