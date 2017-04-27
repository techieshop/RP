using RP.Model;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class RadioButtonItemsViewModel : BaseInputViewModel
	{
		public ICollection<SelectListItem> Items { get; set; }
		public string Name { get; set; }

		public RadioButtonItemsViewModel(ModelMetadata metadata)
			: base(metadata)
		{
			if (string.IsNullOrEmpty(_placeholderMessage))
				_placeholderMessage = StyleContext.GetTranslation(Dom.Translation.Common.Select);

			if (string.IsNullOrEmpty(_helpTitleMessage))
				_helpTitleMessage = StyleContext.GetTranslation(Dom.Translation.Common.Enter);
		}
	}
}