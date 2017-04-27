using RP.Model;
using RP.Platform.DataAnnotations;

namespace RP.Platform.ViewModel
{
	public class RegistrationViewModel : BaseViewModel
	{
		[RPDisplay(LabelCode = Dom.Translation.Registration.CandidateInfo, PlaceholderCode = Dom.Translation.Registration.CandidateInfo)]
		[RPMaxLength(2048)]
		[RPRequired]
		public string CandidateInfo { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Registration.HeadInfo, PlaceholderCode = Dom.Translation.Registration.HeadInfo)]
		[RPMaxLength(2048)]
		[RPRequired]
		public string HeadInfo { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Organization.Singular, PlaceholderCode = Dom.Translation.Organization.Singular)]
		[RPMaxLength(256)]
		[RPRequired]
		public string OrganizationName { get; set; }
	}
}