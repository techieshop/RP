using System;

namespace RP.Model
{
	public class PublicOrganizationDetail : BaseDetail
	{
		public string Name { get; set; }
		public DateTime? CreateDate { get; set; }
		public int MemberCount { get; set; }
	}
}