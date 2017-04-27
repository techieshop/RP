using RP.Model;

namespace RP.DAL.Mapping
{
	public class TemplateMap : BaseEntityMap<Template>
	{
		public TemplateMap()
		{
			ToTable("Template", "stl");

			Property(x => x.Name).HasMaxLength(128).IsRequired();
			Property(x => x.Description).HasMaxLength(512).IsOptional();
			Property(x => x.TitleCode).IsRequired();
			Property(x => x.ContentCode).IsRequired();
		}
	}
}