using System;

namespace RP.Model
{
	public class ResultDetailCommon : BaseModel
	{
		public string Name { get; set; }
		public string SeasonName { get; set; }
		public int SeasonTypeId { get; set; }
		public DateTime StartRaceTime { get; set; }
		public int Year { get; set; }
	}
}