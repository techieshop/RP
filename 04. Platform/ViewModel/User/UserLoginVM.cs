using RP.Model;
using RP.Platform.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace RP.Platform.ViewModel
{
	public class UserLoginViewModel
	{
		[RPRequired]
		[RPDisplay(LabelCode = Dom.Translation.Common.Email, PlaceholderCode = Dom.Translation.Common.Email)]
		public string Email { get; set; }

		[Required]
		[RPDisplay(LabelCode = Dom.Translation.Common.Password, PlaceholderCode = Dom.Translation.Common.Password)]
		public string Password { get; set; }

		public string RedirectUrl { get; set; }
	}
}