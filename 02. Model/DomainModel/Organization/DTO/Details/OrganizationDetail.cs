using System;

namespace RP.Model
{
	public class OrganizationDetail : BaseDetail
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime? CreateDate { get; set; }
		public int MemberCount { get; set; }
	}
}