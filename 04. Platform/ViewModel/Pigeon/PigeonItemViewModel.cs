namespace RP.Platform.ViewModel
{
	public class PigeonItemViewModel : BaseItemViewModel
	{
		public string Code { get; set; }
		public string Number { get; set; }
		public string OrganizationName { get; set; }
		public string OwnerFormattedName { get; set; }
		public int PrizeCount { get; set; }
		public string Ring { get; set; }
		public string Sex { get; set; }
		public int SexId { get; set; }
		public int Year { get; set; }
	}
}