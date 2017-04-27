using RP.Model;

namespace RP.DAL.Mapping
{
	public class WebsiteMap : BaseModelMap<Website>
	{
		public WebsiteMap()
		{
			ToTable("Website", "acc");

			Property(t => t.Url).HasMaxLength(256).IsRequired();
			Property(t => t.Description).IsOptional();
		}
	}
}