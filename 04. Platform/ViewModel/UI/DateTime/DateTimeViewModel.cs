using RP.Model;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class DateTimeViewModel : BaseInputViewModel
	{
		public DateTimeViewModel(ModelMetadata metadata, DataTypeUI dataType, DateTimeFormatUI dateTimeFormat)
			: base(metadata)
		{
			if (string.IsNullOrEmpty(PlaceholderMessage))
			{
				switch (dateTimeFormat)
				{
					case DateTimeFormatUI.Date:
						PlaceholderMessage = StyleContext.GetTranslation(Dom.Translation.Common.DateFormat);
						break;

					case DateTimeFormatUI.DateTime:
						PlaceholderMessage = StyleContext.GetTranslation(Dom.Translation.Common.DateTimeFormat);
						break;
				}
			}

			DataType = dataType;
			DateTimeFormat = dateTimeFormat;
		}

		public DataTypeUI DataType { get; private set; }
		public DateTimeFormatUI DateTimeFormat { get; private set; }
	}
}