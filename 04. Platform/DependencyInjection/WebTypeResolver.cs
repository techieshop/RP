using Microsoft.Practices.Unity;
using RP.DAL.DBContext;
using RP.DAL.Repository;
using RP.DAL.UnitOfWork;
using RP.Platform.Context;
using RP.Platform.View;

namespace RP.Platform.DependencyInjection
{
	public class WebTypeResolver : ITypeResolver
	{
		public void RegisterType(IUnityContainer container)
		{
			container.RegisterType<EntityDbContext>(new InjectionConstructor(DbConnectionEnum.Platform));

			container.RegisterType<IUnitOfWork<EntityDbContext>, UnitOfWork<EntityDbContext>>(new HierarchicalLifetimeManager());

			container.RegisterType<IEntityContext, EntityContext>(new HierarchicalLifetimeManager());
			container.RegisterType<IStyleContext, StyleContext>(new HierarchicalLifetimeManager());
			container.RegisterType<IUserContext, UserContext>(new HierarchicalLifetimeManager());

			container.RegisterType<IAddressRepository, AddressRepository>();
			container.RegisterType<IDomainValueRepository, DomainValueRepository>();
			container.RegisterType<IEntityCacheRepository, EntityCacheRepository>();
			container.RegisterType<IEntityRepository, EntityRepository>();
			container.RegisterType<IMemberRepository, MemberRepository>();
			container.RegisterType<IOrganizationRepository, OrganizationRepository>();
			container.RegisterType<IPigeonRepository, PigeonRepository>();
			container.RegisterType<IPointRepository, PointRepository>();
			container.RegisterType<IPublicRepository, PublicRepository>();
			container.RegisterType<IRaceRepository, RaceRepository>();
			container.RegisterType<IRegistrationRepository, RegistrationRepository>();
			container.RegisterType<IRoleRepository, RoleRepository>();
			container.RegisterType<ISeasonRepository, SeasonRepository>();
			container.RegisterType<ITranslationRepository, TranslationRepository>();
			container.RegisterType<IUserRepository, UserRepository>();

			container.RegisterType(typeof(BaseViewPage<>));
		}
	}
}