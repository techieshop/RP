using RP.Model;

namespace RP.DAL.Mapping
{
	public class TranslationCodeMap : BaseModelMap<TranslationCode>
	{
		public TranslationCodeMap()
		{
			ToTable("TranslationCode", "stl");

			Property(t => t.Name).HasMaxLength(128).IsOptional();

			HasMany(t => t.Translations).WithRequired().HasForeignKey(t => t.TranslationCodeId);
		}
	}
}