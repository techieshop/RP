using RP.Model;

namespace RP.DAL.Mapping
{
	public class RaceResultMap : BaseModelMap<RaceResult>
	{
		public RaceResultMap()
		{
			ToTable("RaceResult", "race");

			Property(x => x.RaceId).IsRequired();
			Property(x => x.PigeonId).IsRequired();
			Property(x => x.Distance).IsRequired();
			Property(x => x.FlyTime).IsRequired();
			Property(x => x.Speed).IsRequired();
			Property(x => x.Position).IsRequired();
			Property(x => x.Ac).IsRequired();

			HasRequired(t => t.Race).WithMany().HasForeignKey(t => t.RaceId);
			HasRequired(t => t.Pigeon).WithMany().HasForeignKey(t => t.PigeonId);

			HasMany(t => t.RaceResultCategories).WithRequired().HasForeignKey(t => t.RaceResultId);
		}
	}
}