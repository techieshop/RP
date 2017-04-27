namespace RP.Model
{
	public class OrganizationRelation : BaseModel
	{
		public virtual int OrganizationId { get; set; }
		public virtual int RelatedOrganizationId { get; set; }
		public virtual byte Order { get; set; }
		public virtual byte Level { get; set; }

		public virtual Organization Organization { get; set; }
		public virtual Organization RelatedOrganization { get; set; }
	}
}