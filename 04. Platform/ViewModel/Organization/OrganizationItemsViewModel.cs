namespace RP.Platform.ViewModel
{
	public class OrganizationItemsViewModel : BaseItemsViewModel<OrganizationFilterViewModel, OrganizationItemViewModel>
	{
		public int MemberCount { get; set; }
		public int PigeonCount { get; set; }
	}
}