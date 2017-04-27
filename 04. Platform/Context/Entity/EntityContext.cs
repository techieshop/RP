using RP.DAL.Repository;
using System.Web.Mvc;

namespace RP.Platform.Context
{
	public partial class EntityContext : IEntityContext
	{
		private readonly IEntityRepository _entityRepository;
		private readonly IStyleContext _styleContext;
		private readonly IUserContext _userContext;

		public EntityContext(
			IEntityRepository entityRepository,
			IStyleContext styleContext,
			IUserContext userContext)
		{
			_entityRepository = entityRepository;
			_styleContext = styleContext;
			_userContext = userContext;
		}

		public static IEntityContext Current => DependencyResolver.Current.GetService<IEntityContext>();
	}
}