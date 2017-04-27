using System;
using System.Collections.Generic;

namespace RP.Model
{
	public class Race : BaseEntity
	{
		public virtual int? AdultsCount { get; set; }
		public virtual double? AverageDistance { get; set; }
		public virtual int? CockCount { get; set; }
		public virtual ICollection<CommonRace> CommonRaces { get; set; }
		public virtual DateTime? DarknessBeginTime { get; set; }
		public virtual DateTime? DarknessEndTime { get; set; }
		public virtual double? DurationOfCompetition { get; set; }
		public virtual DateTime? EndСompetitionTime { get; set; }
		public virtual int? HenCount { get; set; }
		public virtual double? InFactAbidedPercent { get; set; }
		public virtual int? MemberCount { get; set; }
		public virtual int? MemberTwentyPercentAFact { get; set; }
		public virtual string Name { get; set; }
		public virtual int? PigeonCount { get; set; }
		public virtual ICollection<PigeonReturnTime> PigeonReturnTimes { get; set; }
		public virtual int? PigeonTwentyPercentAFact { get; set; }
		public virtual Point Point { get; set; }
		public virtual int PointId { get; set; }
		public virtual ICollection<RaceDistance> RaceDistances { get; set; }
		public virtual ICollection<RacePigeon> RacePigeons { get; set; }
		public virtual ICollection<RaceResult> RaceResults { get; set; }
		public virtual ICollection<RaceStatistic> RaceStatistics { get; set; }
		public virtual DomainValue RaceType { get; set; }
		public virtual int RaceTypeId { get; set; }
		public virtual Season Season { get; set; }
		public virtual int SeasonId { get; set; }
		public virtual double? SpeedOfFirst { get; set; }
		public virtual double? SpeedOfLast { get; set; }
		public virtual DateTime? StartRaceTime { get; set; }
		public virtual DateTime? StartСompetitionTime { get; set; }
		public virtual DateTime? TimeOfFirst { get; set; }
		public virtual DateTime? TimeOfLast { get; set; }
		public virtual int? TwentyPercent { get; set; }
		public virtual string Weather { get; set; }
		public virtual int? YearlyCount { get; set; }
		public virtual int? YoungCount { get; set; }

		public static Race Empty()
		{
			return new Race
			{
				EntityInfo = EntityInfo.Empty(Dom.EntityType.Race.Id)
			};
		}
	}
}