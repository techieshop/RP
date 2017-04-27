using System;

namespace RP.Platform.ViewModel
{
	public class ResultDetailViewModel : BaseViewModel
	{
		public int AdultsCount { get; set; }
		public double AverageDistance { get; set; }
		public int CockCount { get; set; }
		public double DurationOfCompetition { get; set; }
		public TimeSpan DurationOfCompetitionTimeSpan { get; set; }
		public DateTime EndСompetitionTime { get; set; }
		public int HenCount { get; set; }
		public double InFactAbidedPercent { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public int MemberCount { get; set; }
		public int MemberTwentyPercentAFact { get; set; }
		public string Name { get; set; }
		public int OrganizationId { get; set; }
		public string OrganizationName { get; set; }
		public int PigeonCount { get; set; }
		public int PigeonTwentyPercentAFact { get; set; }
		public string PointName { get; set; }
		public int RaceTypeId { get; set; }
		public string SeasonName { get; set; }
		public int SeasonTypeId { get; set; }
		public double SpeedOfFirst { get; set; }
		public double SpeedOfLast { get; set; }
		public DateTime StartСompetitionTime { get; set; }
		public DateTime StartRaceTime { get; set; }
		public DateTime TimeOfFirst { get; set; }
		public DateTime TimeOfLast { get; set; }
		public int TwentyPercent { get; set; }
		public string Weather { get; set; }
		public int Year { get; set; }
		public int YearlyCount { get; set; }
		public int YoungCount { get; set; }
	}
}