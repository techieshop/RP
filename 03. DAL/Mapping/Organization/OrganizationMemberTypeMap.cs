using RP.Model;

namespace RP.DAL.Mapping
{
	public class OrganizationMemberTypeMap : BaseModelMap<OrganizationMemberType>
	{
		public OrganizationMemberTypeMap()
		{
			ToTable("OrganizationMemberType", "acc");

			Property(x => x.OrganizationId).IsRequired();
			Property(x => x.MemberId).IsRequired();
			Property(x => x.MemberTypeId).IsRequired();

			HasRequired(t => t.Organization).WithMany().HasForeignKey(t => t.OrganizationId);
			HasRequired(t => t.Member).WithMany().HasForeignKey(t => t.MemberId);
			HasRequired(t => t.MemberType).WithMany().HasForeignKey(t => t.MemberTypeId);
		}
	}
}