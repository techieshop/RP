using RP.Model;

namespace RP.DAL.Mapping
{
	public class UserMap : BaseEntityMap<User>
	{
		public UserMap()
		{
			ToTable("User", "acc");

			Property(t => t.Email).HasMaxLength(128).IsRequired();
			Property(t => t.Password).HasMaxLength(64).IsRequired();
			Property(t => t.LastName).HasMaxLength(32).IsRequired();
			Property(t => t.FirstName).HasMaxLength(32).IsRequired();
			Property(t => t.MiddleName).HasMaxLength(32).IsOptional();
			Property(t => t.DateOfBirth).IsOptional();
			Property(t => t.Phone).HasMaxLength(32).IsOptional();
			Property(t => t.Mobile).HasMaxLength(32).IsOptional();
			Property(t => t.Salutation).HasMaxLength(32).IsOptional();
			Property(t => t.HasPhoto).IsRequired();
			Property(t => t.AddressId).IsOptional();
			Property(t => t.GenderId).IsOptional();
			Property(t => t.LanguageId).IsRequired();

			HasOptional(t => t.Address).WithMany().HasForeignKey(t => t.AddressId);
			HasOptional(t => t.Gender).WithMany().HasForeignKey(t => t.GenderId);
			HasRequired(t => t.Language).WithMany().HasForeignKey(t => t.LanguageId);

			HasMany(t => t.UserRoles).WithRequired().HasForeignKey(t => t.UserId);
		}
	}
}