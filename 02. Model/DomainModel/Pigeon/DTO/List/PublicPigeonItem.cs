namespace RP.Model
{
	public class PublicPigeonItem : BaseItem
	{
		public int Year { get; set; }
		public string Code { get; set; }
		public string OwnerFirstName { get; set; }
		public string OwnerLastName { get; set; }
		public string OwnerMiddleName { get; set; }
		public int SexId { get; set; }
		public int PrizeCount { get; set; }
	}
}