namespace RP.Model
{
	public class PublicPigeonDetail : BaseDetail
	{
		public int Year { get; set; }
		public string Code { get; set; }
		public int MemberId { get; set; }
		public string OrganizationName { get; set; }
		public int SexId { get; set; }
		public int PrizeCount { get; set; }
	}
}