namespace RP.Model
{
	public class PigeonRef : BaseModel
	{
		public string Code { get; set; }
		public bool InStatement { get; set; }
		public int MemberId { get; set; }
		public string Number { get; set; }
		public string Ring { get; set; }
		public int Year { get; set; }
	}
}