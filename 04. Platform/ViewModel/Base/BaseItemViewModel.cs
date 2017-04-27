namespace RP.Platform.ViewModel
{
	public class BaseItemViewModel : BaseViewModel
	{
		public int EntityInfoId { get; set; }
		public int EntityStateId { get; set; }
		public int? EntityStateNameCode { get; set; }
		public int OrganizationId { get; set; }
	}
}