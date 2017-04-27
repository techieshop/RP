namespace RP.Model
{
	public class PigeonDetail : BaseDetail
	{
		public string Code { get; set; }
		public string Number { get; set; }
		public string OrganizationName { get; set; }
		public string OwnerFirstName { get; set; }
		public string OwnerLastName { get; set; }
		public string OwnerMiddleName { get; set; }
		public int PrizeCount { get; set; }
		public int? SexId { get; set; }
		public int Year { get; set; }
	}
}