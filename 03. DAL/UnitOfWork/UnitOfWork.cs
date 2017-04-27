using RP.DAL.DBContext;
using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace RP.DAL.UnitOfWork
{
	public class UnitOfWork<TContext> : IUnitOfWork<TContext>
		where TContext : BaseDbContext<TContext>
	{
		private bool _disposed;

		public UnitOfWork(TContext dbContext)
		{
			DbContext = dbContext;
		}

		public void SaveChanges()
		{
			try
			{
				DbContext.SaveChanges();
			}
			catch (DbEntityValidationException ex)
			{
				var errorMessages = ex.EntityValidationErrors
					.SelectMany(t => t.ValidationErrors)
					.Select(t => t.ErrorMessage);
				var fullErrorMessage = string.Join("; ", errorMessages);
				var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
				throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
			}
		}

		public TContext DbContext { get; }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					DbContext?.Dispose();
				}
			}
			_disposed = true;
		}
	}
}