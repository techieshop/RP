using System;

namespace RP.Model
{
	public class Registration : BaseModel
	{
		public DateTime DateTimeRequest { get; set; }
		public bool IsRead { get; set; }
		public string OrganizationName { get; set; }
		public string CandidateInfo { get; set; }
		public string HeadInfo { get; set; }
	}
}