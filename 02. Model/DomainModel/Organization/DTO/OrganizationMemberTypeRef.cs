namespace RP.Model
{
	public class OrganizationMemberTypeRef : BaseModel
	{
		public int OrganizationId { get; set; }
		public int MemberId { get; set; }
		public int? MemberTypeId { get; set; }

		public string OrganizationName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
	}
}