namespace RP.Platform.ViewModel
{
	public class PigeonItemsViewModel : BaseItemsViewModel<PigeonFilterViewModel, PigeonItemViewModel>
	{
		public int OrganizationCount { get; set; }
		public int MemberCount { get; set; }
	}
}