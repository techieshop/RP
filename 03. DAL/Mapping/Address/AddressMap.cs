using RP.Model;

namespace RP.DAL.Mapping
{
	public class AddressMap : BaseModelMap<Address>
	{
		public AddressMap()
		{
			ToTable("Address", "acc");

			Property(t => t.CountryId).IsRequired();
			Property(t => t.City).HasMaxLength(128).IsOptional();
			Property(t => t.PostalCode).HasMaxLength(16).IsOptional();
			Property(t => t.Street).HasMaxLength(128).IsOptional();
			Property(t => t.Number).HasMaxLength(16).IsOptional();
			Property(t => t.Latitude).IsOptional();
			Property(t => t.Longitude).IsOptional();
			Property(t => t.FormattedAddress).IsOptional();

			HasRequired(t => t.Country).WithMany().HasForeignKey(t => t.CountryId);
		}
	}
}