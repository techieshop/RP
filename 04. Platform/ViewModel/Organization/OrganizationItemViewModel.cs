using System.Collections.Generic;

namespace RP.Platform.ViewModel
{
	public class OrganizationItemViewModel : BaseItemViewModel
	{
		public string Name { get; set; }
		public int MemberCount { get; set; }

		public Dictionary<int, string> Heads { get; set; }
		public Dictionary<int, string> Judges { get; set; }
	}
}