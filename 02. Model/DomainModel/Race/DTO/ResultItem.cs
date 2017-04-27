namespace RP.Model
{
	public class ResultItem : BaseModel
	{
		public string Name { get; set; }
		public string OrganizationName { get; set; }
		public int RaceTypeId { get; set; }
		public int SeasonId { get; set; }
		public int SeasonTypeId { get; set; }
		public int Year { get; set; }
	}
}