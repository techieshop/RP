using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;

namespace RP.DAL.Repository
{
	public class PublicRepository : BaseRepository<EntityDbContext>, IPublicRepository
	{
		public PublicRepository(IUnitOfWork<EntityDbContext> unitOfWork)
			: base(unitOfWork)
		{
		}

		public PublicMemberDetails GetMemberDetails(int id, PublicMemberFilter filter)
		{
			var parameters = new
			{
				id
			};

			return ExecuteMultiResultSetSp<PublicMemberDetails>("[acc].[spGetPublicMemberDetails]", parameters);
		}

		public PublicMemberItems GetMemberItems(PublicMemberFilter filter)
		{
			var parameters = new
			{
				filter.Search,
				filter.Skip,
				filter.Take
			};

			return ExecuteMultiResultSetSp<PublicMemberItems>("[acc].[spGetPublicMemberItems]", parameters);
		}

		public PublicOrganizationDetails GetOrganizationDetails(int id, PublicOrganizationFilter filter)
		{
			var parameters = new
			{
				id
			};

			return ExecuteMultiResultSetSp<PublicOrganizationDetails>("[acc].[spGetPublicOrganizationDetails]", parameters);
		}

		public PublicOrganizationItems GetOrganizationItems(PublicOrganizationFilter filter)
		{
			var parameters = new
			{
				filter.Search,
				filter.Skip,
				filter.Take
			};

			return ExecuteMultiResultSetSp<PublicOrganizationItems>("[acc].[spGetPublicOrganizationItems]", parameters);
		}

		public PublicPigeonDetails GetPigeonDetails(int id, PublicPigeonFilter filter)
		{
			var parameters = new
			{
				id
			};

			return ExecuteMultiResultSetSp<PublicPigeonDetails>("[acc].[spGetPublicPigeonDetails]", parameters);
		}

		public PublicPigeonItems GetPigeonItems(PublicPigeonFilter filter)
		{
			var parameters = new
			{
				filter.Search,
				filter.Skip,
				filter.Take
			};

			return ExecuteMultiResultSetSp<PublicPigeonItems>("[acc].[spGetPublicPigeonItems]", parameters);
		}
	}
}