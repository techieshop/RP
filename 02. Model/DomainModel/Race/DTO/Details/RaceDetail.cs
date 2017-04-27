﻿using System;

namespace RP.Model
{
	public class RaceDetail : BaseDetail
	{
		public DateTime? DarknessBeginTime { get; set; }
		public DateTime? DarknessEndTime { get; set; }
		public DateTime? EndСompetitionTime { get; set; }
		public bool HasCalculatedDistances { get; set; }
		public bool HasResults { get; set; }
		public int MemberCount { get; set; }
		public string Name { get; set; }
		public string OrganizationName { get; set; }
		public int PigeonCount { get; set; }
		public int PointId { get; set; }
		public string PointName { get; set; }
		public int RaceTypeId { get; set; }
		public int ReturnPigeonCount { get; set; }
		public int SeasonId { get; set; }
		public int SeasonTypeId { get; set; }
		public DateTime? StartRaceTime { get; set; }
		public DateTime? StartСompetitionTime { get; set; }
		public string Weather { get; set; }
		public int Year { get; set; }
	}
}