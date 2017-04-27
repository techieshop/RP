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
		public static MvcHtmlString RPTextAreaFor<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			int rows = 10,
			string labelMessage = null,
			string placeholderMessage = null)
		{
			var metadata = htmlHelper.GetModelMetadata(expression, labelMessage, placeholderMessage);
			var viewModel = new TextBoxViewModel(metadata);
			var htmlAttributes = new Dictionary<string, object>();

			if (!string.IsNullOrEmpty(viewModel.PlaceholderMessage))
				htmlAttributes.Add("placeholder", viewModel.PlaceholderMessage);

			htmlAttributes.Add("type", viewModel.DataType.GetMetadata().Value);
			htmlAttributes.Add("class", "form-control input-val");
			htmlAttributes.Add("style", "resize:none;");
			htmlAttributes.Add("rows", rows);
			viewModel.Input = htmlHelper.TextAreaFor(expression, htmlAttributes);

			return htmlHelper.Partial(Mvc.View.UI.TextArea, viewModel);
		}
	}
}