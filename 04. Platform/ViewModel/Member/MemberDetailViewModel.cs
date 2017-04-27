using RP.Model;
using RP.Platform.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class MemberDetailViewModel : BaseDetailViewModel
	{
		[RPDisplay(LabelCode = Dom.Translation.Common.DateOfBirth)]
		public DateTime? DateOfBirth { get; set; }

		[RPEmail]
		[RPMaxLength(128)]
		[RPDisplay(LabelCode = Dom.Translation.Common.Email)]
		public string Email { get; set; }

		[RPRequired]
		[RPMaxLength(32)]
		[RPDisplay(LabelCode = Dom.Translation.User.FirstName)]
		public string FirstName { get; set; }

		public string Gender { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.Gender.Name)]
		public int? GenderId { get; set; }

		public ICollection<SelectListItem> GenderItems { get; set; }

		[RPRequired]
		[RPMaxLength(32)]
		[RPDisplay(LabelCode = Dom.Translation.User.LastName)]
		public string LastName { get; set; }

		public string MemberType { get; set; }

		[RPMaxLength(32)]
		[RPDisplay(LabelCode = Dom.Translation.User.MiddleName)]
		public string MiddleName { get; set; }

		[RPMaxLength(16)]
		[RPDisplay(LabelCode = Dom.Translation.Common.Mobile)]
		public string Mobile { get; set; }

		public string OrganizationName { get; set; }

		[RPMaxLength(16)]
		[RPDisplay(LabelCode = Dom.Translation.Common.Telephone)]
		public string Phone { get; set; }

		public int PigeonCount { get; set; }
	}
}