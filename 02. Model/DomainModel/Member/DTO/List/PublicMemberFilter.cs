namespace RP.Model
{
	public class PublicMemberFilter : BaseEntityFilter
	{
		public string Search { get; set; }
		public int? UserId { get; set; }
		public int? UserRoleId { get; set; }
	}
}