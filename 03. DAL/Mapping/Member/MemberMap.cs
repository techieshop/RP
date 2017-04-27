using RP.Model;

namespace RP.DAL.Mapping
{
	public class MemberMap : BaseEntityMap<Member>
	{
		public MemberMap()
		{
			ToTable("Member", "acc");

			Property(x => x.Email).HasMaxLength(128).IsOptional();
			Property(x => x.LastName).HasMaxLength(32).IsRequired();
			Property(x => x.FirstName).HasMaxLength(32).IsRequired();
			Property(x => x.MiddleName).HasMaxLength(32).IsOptional();
			Property(x => x.Phone).HasMaxLength(32).IsOptional();
			Property(x => x.Mobile).HasMaxLength(32).IsOptional();
			Property(x => x.DateOfBirth).IsOptional();
			Property(x => x.AddressId).IsOptional();
			Property(x => x.GenderId).IsOptional();
			Property(x => x.WebsiteId).IsOptional();

			HasOptional(t => t.Address).WithMany().HasForeignKey(t => t.AddressId);
			HasOptional(t => t.Gender).WithMany().HasForeignKey(t => t.GenderId);
			HasOptional(t => t.Website).WithMany().HasForeignKey(t => t.WebsiteId);

			HasMany(t => t.OrganizationMemberTypes).WithRequired().HasForeignKey(t => t.MemberId);
			HasMany(t => t.Pigeons).WithRequired().HasForeignKey(t => t.MemberId);
			HasMany(t => t.RaceDistances).WithRequired().HasForeignKey(t => t.MemberId);
			HasMany(t => t.RaceStatistics).WithRequired().HasForeignKey(t => t.MemberId);
		}
	}
}