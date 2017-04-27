using RP.Model;

namespace RP.DAL.Mapping
{
	public class SeasonMap : BaseEntityMap<Season>
	{
		public SeasonMap()
		{
			ToTable("Season", "race");

			Property(x => x.SeasonTypeId).IsRequired();
			Property(x => x.Year).IsRequired();

			HasRequired(x => x.SeasonType).WithMany().HasForeignKey(x => x.SeasonTypeId);

			HasMany(t => t.Races).WithRequired().HasForeignKey(t => t.SeasonId);
			HasMany(t => t.Statements).WithRequired().HasForeignKey(t => t.SeasonId);
		}
	}
}