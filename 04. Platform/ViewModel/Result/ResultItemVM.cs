using System.Collections.Generic;

namespace RP.Platform.ViewModel
{
	public class ResultItemViewModel
	{
		public ICollection<ResultItemViewModel> Items { get; set; }
		public int Level { get; set; }
		public string Name { get; set; }
		public int? RaceId { get; set; }
	}
}