using RP.Common.Extension;
using RP.Platform.Manager;
using RP.Platform.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace RP.Platform.Extension
{
	public static partial class HtmlExtension
	{
		public static MvcHtmlString RPTextBoxFor<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			DataTypeUI dataType = DataTypeUI.Undefined,
			string labelMessage = null,
			string placeholderMessage = null,
			string cssClass = null)
		{
			var metadata = htmlHelper.GetModelMetadata(expression, labelMessage, placeholderMessage);
			var viewModel = new TextBoxViewModel(metadata, dataType);
			var htmlAttributes = new Dictionary<string, object>();

			if (!string.IsNullOrEmpty(viewModel.PlaceholderMessage))
				htmlAttributes.Add("placeholder", viewModel.PlaceholderMessage);

			htmlAttributes.Add("type", viewModel.DataType.GetMetadata().Value);
			htmlAttributes.Add("class", $"form-control input-val {cssClass}");
			viewModel.Input = htmlHelper.TextBoxFor(expression, htmlAttributes);

			return htmlHelper.Partial(Mvc.View.UI.TextBox, viewModel);
		}
	}
}