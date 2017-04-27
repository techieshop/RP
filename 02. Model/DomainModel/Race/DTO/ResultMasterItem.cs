namespace RP.Model
{
	public class ResultMasterItem : BaseModel
	{
		public double Coefficient { get; set; }
		public double CoefficientTotal { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public double Mark { get; set; }
		public string Member { get; set; }
		public string MiddleName { get; set; }
		public string OrganizationName { get; set; }
		public int Position { get; set; }
		public int PrizeCount { get; set; }
		public int StatementCount { get; set; }
		public double Success { get; set; }
	}
}