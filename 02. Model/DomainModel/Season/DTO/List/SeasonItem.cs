namespace RP.Model
{
	public class SeasonItem : BaseItem
	{
		public int MemberCount { get; set; }
		public string OrganizationName { get; set; }
		public int PigeonCount { get; set; }
		public int SeasonTypeId { get; set; }
		public int Year { get; set; }
	}
}