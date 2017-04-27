using RP.Model;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class DropDownViewModel : BaseInputViewModel
	{
		public DropDownViewModel(ModelMetadata metadata)
			: base(metadata)
		{
			if (string.IsNullOrEmpty(_placeholderMessage))
				_placeholderMessage = StyleContext.GetTranslation(Dom.Translation.Common.Select);

			if (string.IsNullOrEmpty(_helpTitleMessage))
				_helpTitleMessage = StyleContext.GetTranslation(Dom.Translation.Common.Enter);
		}

		public string DataDepends { get; set; }
		public string DataUrl { get; set; }
		public bool HasFind { get; set; }
		public string DataId { get; set; }
		public bool IsMultiSelect { get; set; }
		public ICollection<SelectListItem> Items { get; set; }
		public string Name { get; set; }
		public string SelectedValue { get; set; }
	}
}