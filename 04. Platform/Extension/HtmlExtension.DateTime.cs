using RP.Common.Extension;
using RP.Platform.Manager;
using RP.Platform.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using RP.Common.Manager;

namespace RP.Platform.Extension
{
	public static partial class HtmlExtension
	{
		public static MvcHtmlString RPDateTimeFor<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			DataTypeUI dataType = DataTypeUI.Undefined,
			DateTimeFormatUI dateTimeFormat = DateTimeFormatUI.Date,
			string labelMessage = null,
			string placeholderMessage = null)
		{
			var metadata = htmlHelper.GetModelMetadata(expression, labelMessage, placeholderMessage);
			var viewModel = new DateTimeViewModel(metadata, dataType, dateTimeFormat);
			var htmlAttributes = new Dictionary<string, object>();

			if (!string.IsNullOrEmpty(viewModel.PlaceholderMessage))
				htmlAttributes.Add("placeholder", viewModel.PlaceholderMessage);

			htmlAttributes.Add("type", viewModel.DataType.GetMetadata().Value);
			switch (dateTimeFormat)
			{
				case DateTimeFormatUI.Date:
					htmlAttributes.Add("class", "form-control datepicker");
					break;

				case DateTimeFormatUI.DateTime:
					htmlAttributes.Add("class", "form-control datetimepicker");
					break;
			}
			viewModel.Input = htmlHelper.TextBoxFor(expression, ((DateTime?)viewModel.Model).ToShortDateString(CultureManager.DateTimeFormat), htmlAttributes);

			return htmlHelper.Partial(Mvc.View.UI.DateTime, viewModel);
		}
	}
}