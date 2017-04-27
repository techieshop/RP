namespace RP.Platform.ViewModel
{
	public class MemberItemsViewModel : BaseItemsViewModel<MemberFilterViewModel, MemberItemViewModel>
	{
		public int OrganizationCount { get; set; }
		public int PigeonCount { get; set; }
	}
}