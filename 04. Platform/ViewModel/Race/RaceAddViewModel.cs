using RP.Model;
using RP.Platform.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class RaceAddViewModel
	{
		[RPDisplay(LabelCode = Dom.Translation.Race.DarknessBeginTime)]
		public DateTime? DarknessBeginTime { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Race.DarknessEndTime)]
		public DateTime? DarknessEndTime { get; set; }

		public DateTime? EndСompetitionTime { get; set; }

		[RPRequired]
		[RPMaxLength(128)]
		[RPDisplay(LabelCode = Dom.Translation.Common.Name)]
		public string Name { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Organization.Singular)]
		[RPRequired]
		public int OrganizationId { get; set; }

		public ICollection<SelectListItem> OrganizationItems { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Point.Singular)]
		[RPRequired]
		public int PointId { get; set; }

		public ICollection<SelectListItem> PointItems { get; set; }
		public string PointName { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Race.RaceType)]
		[RPRequired]
		public int RaceTypeId { get; set; }

		public ICollection<SelectListItem> RaceTypeItems { get; set; }
		public string RaceTypeName { get; set; }

		[RPRequired]
		[RPDisplay(LabelCode = Dom.Translation.Season.Singular)]
		public int SeasonId { get; set; }

		public ICollection<SelectListItem> SeasonItems { get; set; }
		public string SeasonName { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Race.StartRaceTime)]
		public DateTime? StartRaceTime { get; set; }

		public DateTime? StartСompetitionTime { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Race.Weather)]
		[RPMaxLength(256)]
		public string Weather { get; set; }
	}
}