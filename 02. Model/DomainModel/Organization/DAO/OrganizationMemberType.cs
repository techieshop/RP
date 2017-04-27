namespace RP.Model
{
	public class OrganizationMemberType : BaseModel
	{
		public virtual int OrganizationId { get; set; }
		public virtual int MemberId { get; set; }
		public virtual int MemberTypeId { get; set; }

		public virtual Organization Organization { get; set; }
		public virtual Member Member { get; set; }
		public virtual DomainValue MemberType { get; set; }
	}
}