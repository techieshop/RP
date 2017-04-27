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
		public static MvcHtmlString RPRadioButtonFor<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			ICollection<SelectListItem> items,
			string labelMessage = null,
			string placeholderMessage = null)
		{
			var metadata = htmlHelper.GetModelMetadata(expression, labelMessage, placeholderMessage);
			var viewModel = new RadioButtonItemsViewModel(metadata)
			{
				Items = items,
				Name = ExpressionHelper.GetExpressionText(expression)
			};
			if (viewModel.Model != null && viewModel.Items != null && viewModel.Items.Count > 0)
			{
				foreach (var item in viewModel.Items)
				{
					object value = viewModel.Model;
					if (viewModel.ModelMetadata.ModelType.IsEnum)
						value = Convert.ChangeType(viewModel.Model, item.Value.GetType());
					if (item.Value.Equals(value.ToString()))
						item.Selected = true;
				}
			}
			return htmlHelper.Partial(Mvc.View.UI.RadioButton, viewModel);
		}
	}
}