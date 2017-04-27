using RP.Model;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class TextBoxViewModel : BaseInputViewModel
	{
		public TextBoxViewModel(ModelMetadata metadata)
			: this(metadata, DataTypeUI.Undefined) { }

		public TextBoxViewModel(ModelMetadata metadata, DataTypeUI dataType)
			: base(metadata)
		{
			if (string.IsNullOrEmpty(PlaceholderMessage))
			{
				PlaceholderMessage = StyleContext.GetTranslation(Dom.Translation.Common.Enter);
			}

			DataType = dataType;
		}

		public DataTypeUI DataType { get; private set; }
	}
}