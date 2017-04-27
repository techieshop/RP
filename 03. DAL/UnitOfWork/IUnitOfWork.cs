using RP.DAL.DBContext;
using System;

namespace RP.DAL.UnitOfWork
{
	public interface IUnitOfWork<out TContext> : IDisposable
		where TContext : BaseDbContext<TContext>
	{
		void SaveChanges();

		TContext DbContext { get; }
	}
}