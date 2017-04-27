namespace RP.Model
{
	public class MemberItem : BaseItem
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string OrganizationName { get; set; }
		public string Phone { get; set; }
		public string Mobile { get; set; }
		public string FormattedAddress { get; set; }
		public int PigeonCount { get; set; }
	}
}