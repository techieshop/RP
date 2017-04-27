namespace RP.Model
{
	public class OrganizationRelationRef
	{
		public int OrganizationId { get; set; }
		public int RelatedOrganizationId { get; set; }
		public int EntityTypeId { get; set; }
	}
}