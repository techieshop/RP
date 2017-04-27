using RP.Model;

namespace RP.DAL.Mapping
{
	public class CountryMap : BaseModelMap<Country>
	{
		public CountryMap()
		{
			ToTable("Country", "dom");

			Property(t => t.Code).HasMaxLength(2).IsFixedLength().IsRequired();
			Property(t => t.NameCode).IsRequired();
			Property(t => t.PhoneCode).HasMaxLength(6).IsRequired();
			Property(t => t.Icon).HasMaxLength(256).IsOptional();
		}
	}
}