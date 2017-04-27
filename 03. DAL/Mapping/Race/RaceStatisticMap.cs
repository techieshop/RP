using RP.Model;

namespace RP.DAL.Mapping
{
	public class RaceStatisticMap : BaseModelMap<RaceStatistic>
	{
		public RaceStatisticMap()
		{
			ToTable("RaceStatistic", "race");

			Property(x => x.RaceId).IsRequired();
			Property(x => x.MemberId).IsRequired();
			Property(x => x.StatedPigeonCount).IsRequired();
			Property(x => x.PrizePigeonCount).IsRequired();
			Property(x => x.Mark).IsRequired();
			Property(x => x.Success).IsRequired();

			HasRequired(t => t.Race).WithMany().HasForeignKey(t => t.RaceId);
			HasRequired(t => t.Member).WithMany().HasForeignKey(t => t.MemberId);
		}
	}
}