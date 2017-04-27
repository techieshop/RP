using RP.Platform.Context;

namespace RP.Platform.DataAnnotations
{
	public class RPDisplayAttribute : BaseDataAnnotationAttribute
	{
		public int LabelCode { get; set; }
		public int PlaceholderCode { get; set; }
		public int HelpTitleCode { get; set; }
		public int HelpDescriptionCode { get; set; }
		public bool ShowValidationMessage { get; set; }

		public RPDisplayAttribute()
		{
			ShowValidationMessage = true;
		}

		public string LabelMessage => StyleContext.Current.GetTranslation(LabelCode);

		public string PlaceholderMessage => StyleContext.Current.GetTranslation(PlaceholderCode);

		public string HelpTitleMessage => StyleContext.Current.GetTranslation(HelpTitleCode);

		public string HelpDescriptionMessage => StyleContext.Current.GetTranslation(HelpDescriptionCode);
	}
}