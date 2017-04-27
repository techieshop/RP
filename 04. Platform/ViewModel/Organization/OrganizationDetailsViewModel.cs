using RP.Model;
using RP.Platform.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class OrganizationDetailsViewModel : BaseDetailsViewModel<OrganizationDetailViewModel>
	{
		public AddressViewModel Address { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.MemberType.Cashier)]
		public ICollection<int> CashierIds { get; set; }

		public ICollection<SelectListItem> CashierItems { get; set; }
		public Dictionary<int, string> Cashiers { get; set; }
		public Dictionary<int, string> Deputies { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.MemberType.Deputy)]
		public ICollection<int> DeputyIds { get; set; }

		public ICollection<SelectListItem> DeputyItems { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.MemberType.Head)]
		public ICollection<int> HeadIds { get; set; }

		public ICollection<SelectListItem> HeadItems { get; set; }
		public Dictionary<int, string> Heads { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.MemberType.Judge)]
		public ICollection<int> JudgeIds { get; set; }

		public ICollection<SelectListItem> JudgeItems { get; set; }
		public Dictionary<int, string> Judges { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.MemberType.MainJudge)]
		public ICollection<int> MainJudgeIds { get; set; }

		public ICollection<SelectListItem> MainJudgeItems { get; set; }
		public Dictionary<int, string> MainJudges { get; set; }
		public Dictionary<int, string> Secretaries { get; set; }

		[RPDisplay(LabelCode = Dom.Translation.MemberType.Secretary)]
		public ICollection<int> SecretaryIds { get; set; }

		public ICollection<SelectListItem> SecretaryItems { get; set; }
		public WebsiteViewModel Website { get; set; }
	}
}