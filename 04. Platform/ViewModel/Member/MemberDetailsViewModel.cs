using System.Collections.Generic;

namespace RP.Platform.ViewModel
{
	public class MemberDetailsViewModel : BaseDetailsViewModel<MemberDetailViewModel>
	{
		public AddressViewModel Address { get; set; }
		public WebsiteViewModel Website { get; set; }
		public ICollection<OrganizationMemberTypeViewModel> OrganizationMemberTypes { get; set; }
	}
}