using RP.Common.Extension;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RP.DAL.DBContext
{
	public class BaseDbContext<TContext> : DbContext where TContext : DbContext
	{
		private ObjectContext _objectContext;

		static BaseDbContext()
		{
			Database.SetInitializer<TContext>(null);
		}

		public BaseDbContext(DbConnectionEnum connection)
			: base(connection.GetMetadata().Value)
		{
			if (connection == DbConnectionEnum.Undefined)
				throw new ArgumentNullException(nameof(connection));

			Configuration.LazyLoadingEnabled = true;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}

		public ObjectContext ObjectContext => _objectContext ?? (_objectContext = ((IObjectContextAdapter)this).ObjectContext);
	}
}