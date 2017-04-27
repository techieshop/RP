using RP.Platform.Manager;
using RP.Platform.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace RP.Platform.Extension
{
	public static partial class HtmlExtension
	{
		public static MvcHtmlString RPTabsWithDateTimeFor<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			IDictionary<string, ICollection<DateTimeSelectListItem>> items,
			string labelMessage = null,
			string placeholderMessage = null)
		{
			var metadata = htmlHelper.GetModelMetadata(expression, labelMessage, placeholderMessage);
			var viewModel = new TabsWithDateTimeViewModel(metadata)
			{
				Items = new Dictionary<string, ICollection<DateTimeSelectListItem>>(),
				Name = ExpressionHelper.GetExpressionText(expression)
			};

			if (items != null)
				viewModel.Items = items;

			viewModel.Input = htmlHelper.RPHiddenFor(expression);

			if (viewModel.Model != null && viewModel.Items != null && viewModel.Items.Count > 0)
			{
				foreach (var item in viewModel.Items)
				{
					if (item.Value != null && item.Value.Count > 0 && ((IEnumerable)viewModel.Model).Cast<object>().Any())
						foreach (var val in item.Value)
						{
							if (((IEnumerable)viewModel.Model).Cast<object>().Contains(int.Parse(val.Value)))
							{
								val.Selected = true;
							}
						}
				}
			}
			return htmlHelper.Partial(Mvc.View.UI.TabsWithDateTime, viewModel);
		}

		public static MvcHtmlString RPTabsWithDragFor<TModel, TProperty>(
					this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			IDictionary<string, ICollection<SelectListItem>> items,
			string labelMessage = null,
			string placeholderMessage = null)
		{
			var metadata = htmlHelper.GetModelMetadata(expression, labelMessage, placeholderMessage);
			var viewModel = new TabsWithDragViewModel(metadata)
			{
				Items = new Dictionary<string, ICollection<SelectListItem>>(),
				Name = ExpressionHelper.GetExpressionText(expression)
			};

			if (items != null)
				viewModel.Items = items;

			viewModel.Input = htmlHelper.RPHiddenFor(expression);

			if (viewModel.Model != null && viewModel.Items != null && viewModel.Items.Count > 0)
			{
				foreach (var item in viewModel.Items)
				{
					if (item.Value != null && item.Value.Count > 0 && ((IEnumerable)viewModel.Model).Cast<object>().Any())
						foreach (var val in item.Value)
						{
							if (((IEnumerable)viewModel.Model).Cast<object>().Contains(int.Parse(val.Value)))
							{
								val.Selected = true;
							}
						}
				}
			}
			return htmlHelper.Partial(Mvc.View.UI.TabsWithDrag, viewModel);
		}
	}
}