using RP.DAL.Mapping;
using System.Data.Entity;

namespace RP.DAL.DBContext
{
	public class EntityDbContext : BaseDbContext<EntityDbContext>
	{
		public EntityDbContext(DbConnectionEnum connection) : base(connection)
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Configurations.Add(new AddressMap());
			modelBuilder.Configurations.Add(new CommonRaceMap());
			modelBuilder.Configurations.Add(new CountryMap());
			modelBuilder.Configurations.Add(new DomainValueMap());
			modelBuilder.Configurations.Add(new DomainValueTypeMap());
			modelBuilder.Configurations.Add(new EntityInfoMap());
			modelBuilder.Configurations.Add(new EntityOrganizationMap());
			modelBuilder.Configurations.Add(new EntityProgressMap());
			modelBuilder.Configurations.Add(new EntityStateMap());
			modelBuilder.Configurations.Add(new EntityStateTransitionMap());
			modelBuilder.Configurations.Add(new EntityTypeMap());
			modelBuilder.Configurations.Add(new MemberMap());
			modelBuilder.Configurations.Add(new PigeonMap());
			modelBuilder.Configurations.Add(new PigeonReturnTimeMap());
			modelBuilder.Configurations.Add(new PointMap());
			modelBuilder.Configurations.Add(new RaceMap());
			modelBuilder.Configurations.Add(new RaceDistanceMap());
			modelBuilder.Configurations.Add(new RacePigeonMap());
			modelBuilder.Configurations.Add(new RaceResultMap());
			modelBuilder.Configurations.Add(new RaceResultCategoryMap());
			modelBuilder.Configurations.Add(new RaceStatisticMap());
			modelBuilder.Configurations.Add(new RoleEntityStateAccessMap());
			modelBuilder.Configurations.Add(new RoleEntityStateTransitionAccessMap());
			modelBuilder.Configurations.Add(new RoleEntityTypeAccessMap());
			modelBuilder.Configurations.Add(new RoleMap());
			modelBuilder.Configurations.Add(new RegistrationMap());
			modelBuilder.Configurations.Add(new SeasonMap());
			modelBuilder.Configurations.Add(new StatementMap());
			modelBuilder.Configurations.Add(new OrganizationMap());
			modelBuilder.Configurations.Add(new OrganizationMemberTypeMap());
			modelBuilder.Configurations.Add(new OrganizationRelationMap());
			modelBuilder.Configurations.Add(new TemplateMap());
			modelBuilder.Configurations.Add(new TranslationCodeMap());
			modelBuilder.Configurations.Add(new TranslationMap());
			modelBuilder.Configurations.Add(new UserMap());
			modelBuilder.Configurations.Add(new UserRoleMap());
			modelBuilder.Configurations.Add(new WebsiteMap());
		}
	}
}