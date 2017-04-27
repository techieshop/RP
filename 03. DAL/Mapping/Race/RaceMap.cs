using RP.Model;

namespace RP.DAL.Mapping
{
	public class RaceMap : BaseEntityMap<Race>
	{
		public RaceMap()
		{
			ToTable("Race", "race");

			Property(x => x.Name).HasMaxLength(128).IsRequired();
			Property(x => x.StartRaceTime).IsOptional();
			Property(x => x.DarknessBeginTime).IsOptional();
			Property(x => x.DarknessEndTime).IsOptional();
			Property(x => x.StartСompetitionTime).IsOptional();
			Property(x => x.EndСompetitionTime).IsOptional();
			Property(x => x.Weather).HasMaxLength(256).IsOptional();
			Property(x => x.RaceTypeId).IsRequired();
			Property(x => x.PointId).IsRequired();
			Property(x => x.SeasonId).IsRequired();

			Property(x => x.MemberCount).IsOptional();
			Property(x => x.PigeonCount).IsOptional();
			Property(x => x.AverageDistance).IsOptional();
			Property(x => x.TimeOfFirst).IsOptional();
			Property(x => x.TimeOfLast).IsOptional();
			Property(x => x.SpeedOfFirst).IsOptional();
			Property(x => x.SpeedOfLast).IsOptional();
			Property(x => x.DurationOfCompetition).IsOptional();
			Property(x => x.TwentyPercent).IsOptional();
			Property(x => x.CockCount).IsOptional();
			Property(x => x.HenCount).IsOptional();
			Property(x => x.YoungCount).IsOptional();
			Property(x => x.YearlyCount).IsOptional();
			Property(x => x.AdultsCount).IsOptional();
			Property(x => x.MemberTwentyPercentAFact).IsOptional();
			Property(x => x.PigeonTwentyPercentAFact).IsOptional();
			Property(x => x.InFactAbidedPercent).IsOptional();

			HasRequired(x => x.RaceType).WithMany().HasForeignKey(x => x.RaceTypeId);
			HasRequired(x => x.Point).WithMany().HasForeignKey(x => x.PointId);
			HasRequired(x => x.Season).WithMany().HasForeignKey(x => x.SeasonId);

			HasMany(t => t.CommonRaces).WithRequired().HasForeignKey(t => t.RaceId);
			HasMany(t => t.PigeonReturnTimes).WithRequired().HasForeignKey(t => t.RaceId);
			HasMany(t => t.RaceDistances).WithRequired().HasForeignKey(t => t.RaceId);
			HasMany(t => t.RacePigeons).WithRequired().HasForeignKey(t => t.RaceId);
			HasMany(t => t.RaceResults).WithRequired().HasForeignKey(t => t.RaceId);
			HasMany(t => t.RaceStatistics).WithRequired().HasForeignKey(t => t.RaceId);
		}
	}
}