using System.Collections.Generic;

namespace RP.Model
{
	public class PublicPigeonFilter : BaseEntityFilter
	{
		public string Search { get; set; }
		public ICollection<int> SexIds { get; set; }
		public int? UserId { get; set; }
		public int? UserRoleId { get; set; }
	}
}