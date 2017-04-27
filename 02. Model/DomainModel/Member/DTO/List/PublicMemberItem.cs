namespace RP.Model
{
	public class PublicMemberItem : BaseItem
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string Phone { get; set; }
		public int? AddressId { get; set; }
		public int PigeonCount { get; set; }
	}
}