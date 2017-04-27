using RP.Model;

namespace RP.DAL.Mapping
{
	public class OrganizationRelationMap : BaseModelMap<OrganizationRelation>
	{
		public OrganizationRelationMap()
		{
			ToTable("OrganizationRelation", "acc");

			Property(t => t.OrganizationId).IsRequired();
			Property(t => t.RelatedOrganizationId).IsRequired();
			Property(t => t.Order).IsRequired();
			Property(t => t.Level).IsRequired();

			HasRequired(t => t.Organization).WithMany().HasForeignKey(t => t.OrganizationId);
			HasRequired(t => t.RelatedOrganization).WithMany().HasForeignKey(t => t.RelatedOrganizationId);
		}
	}
}