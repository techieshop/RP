using RP.Model;

namespace RP.DAL.Mapping
{
	public class TranslationMap : BaseModelMap<Translation>
	{
		public TranslationMap()
		{
			ToTable("Translation", "stl");

			Property(t => t.LanguageId).IsRequired();
			Property(t => t.TranslationCodeId).IsRequired();
			Property(t => t.Value).IsRequired();

			HasRequired(t => t.Language).WithMany().HasForeignKey(t => t.LanguageId);
			HasRequired(t => t.TranslationCode).WithMany().HasForeignKey(t => t.TranslationCodeId);
		}
	}
}