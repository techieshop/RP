using RP.Model;
using RP.Platform.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class UserProfileViewModel : BaseViewModel
	{
		public AddressViewModel Address { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Common.DateOfBirth)]
		public DateTime? DateOfBirth { get; set; }

		[RPRequired]
		[RPMaxLength(128)]
		[RPEmail]
		[RPDisplay(LabelCode = Dom.Translation.Common.Email)]
		public string Email { get; set; }

		[RPRequired]
		[RPMaxLength(32)]
		[RPDisplay(LabelCode = Dom.Translation.User.FirstName)]
		public string FirstName { get; set; }

		public string FormattedName { get; set; }
		public string Gender { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Gender.Name)]
		public int? GenderId { get; set; }

		public ICollection<SelectListItem> GenderItems { get; set; }
		public string Language { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Language.Name)]
		public int LanguageId { get; set; }

		public ICollection<SelectListItem> LanguageItems { get; set; }

		[RPRequired]
		[RPMaxLength(32)]
		[RPDisplay(LabelCode = Dom.Translation.User.LastName)]
		public string LastName { get; set; }

		[RPMaxLength(32)]
		[RPDisplay(LabelCode = Dom.Translation.User.MiddleName)]
		public string MiddleName { get; set; }

		[RPMaxLength(32)]
		[RPDisplay(LabelCode = Dom.Translation.Common.Mobile)]
		public string Mobile { get; set; }

		[RPRequired]
		[RPMaxLength(64)]
		[RPDisplay(LabelCode = Dom.Translation.Common.Password)]
		public string Password { get; set; }

		[RPRequired]
		[RPMaxLength(64)]
		[RPDisplay(LabelCode = Dom.Translation.Common.ConfirmPassword)]
		public string ConfirmPassword { get; set; }

		[RPMaxLength(32)]
		[RPDisplay(LabelCode = Dom.Translation.Common.Telephone)]
		public string Phone { get; set; }

		[RPMaxLength(32)]
		[RPDisplay(LabelCode = Dom.Translation.User.Salutation)]
		public string Salutation { get; set; }
	}
}