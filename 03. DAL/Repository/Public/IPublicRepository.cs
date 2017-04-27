using RP.DAL.DBContext;
using RP.Model;

namespace RP.DAL.Repository
{
	public interface IPublicRepository : IRepository<EntityDbContext>
	{
		PublicMemberDetails GetMemberDetails(int id, PublicMemberFilter filter);

		PublicMemberItems GetMemberItems(PublicMemberFilter filter);

		PublicOrganizationDetails GetOrganizationDetails(int id, PublicOrganizationFilter filter);

		PublicOrganizationItems GetOrganizationItems(PublicOrganizationFilter filter);

		PublicPigeonDetails GetPigeonDetails(int id, PublicPigeonFilter filter);

		PublicPigeonItems GetPigeonItems(PublicPigeonFilter filter);
	}
}