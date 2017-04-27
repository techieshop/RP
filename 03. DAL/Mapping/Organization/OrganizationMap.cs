using RP.Model;

namespace RP.DAL.Mapping
{
	public class OrganizationMap : BaseEntityMap<Organization>
	{
		public OrganizationMap()
		{
			ToTable("Organization", "acc");

			Property(x => x.Name).HasMaxLength(256).IsRequired();
			Property(x => x.Description).IsOptional();

			Property(x => x.CreateDate).IsOptional();
			Property(x => x.AddressId).IsOptional();
			Property(x => x.OrganizationTypeId).IsOptional();
			Property(x => x.WebsiteId).IsOptional();

			HasOptional(t => t.Address).WithMany().HasForeignKey(t => t.AddressId);
			HasOptional(t => t.Website).WithMany().HasForeignKey(t => t.WebsiteId);

			HasMany(t => t.OrganizationMemberTypes).WithOptional().HasForeignKey(t => t.OrganizationId);
			HasMany(t => t.OrganizationRelations).WithRequired().HasForeignKey(t => t.OrganizationId);
		}
	}
}