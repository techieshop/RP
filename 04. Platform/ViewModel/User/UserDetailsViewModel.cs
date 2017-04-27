using RP.Model;
using System.Collections.Generic;

namespace RP.Platform.ViewModel
{
	public class UserDetailsViewModel : BaseDetailsViewModel<UserDetailViewModel>
	{
		public AddressViewModel Address { get; set; }
		public ICollection<UserRoleRef> UserRoles { get; set; }
	}
}