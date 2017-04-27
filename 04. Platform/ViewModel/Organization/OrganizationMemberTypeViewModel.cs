namespace RP.Platform.ViewModel
{
	public class OrganizationMemberTypeViewModel : BaseViewModel
	{
		public int OrganizationId { get; set; }
		public int MemberId { get; set; }

		public int MemberTypeId { get; set; }
		public string OrganizationName { get; set; }
	}
}