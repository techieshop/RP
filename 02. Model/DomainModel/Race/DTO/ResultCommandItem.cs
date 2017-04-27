namespace RP.Model
{
	public class ResultCommandItem : BaseModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public double Mark { get; set; }
		public double MarkTotal { get; set; }
		public string Member { get; set; }
		public string MiddleName { get; set; }
		public string OrganizationName { get; set; }
		public int Position { get; set; }
		public int PrizeCount { get; set; }
		public int StatementCount { get; set; }
	}
}