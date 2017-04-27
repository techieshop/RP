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
		public static MvcHtmlString RPDropDownFor<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			ICollection<SelectListItem> items,
			string labelMessage = null,
			string placeholderMessage = null,
			bool hasFind = true,
			string findMessage = null,
			bool isMultiSelect = true,
			string dataId = null,
			string dataDepends = null,
			string dataUrl = null)
		{
			var metadata = htmlHelper.GetModelMetadata(expression, labelMessage, placeholderMessage, findMessage);
			var viewModel = new DropDownViewModel(metadata)
			{
				Items = new List<SelectListItem>(),
				Name = ExpressionHelper.GetExpressionText(expression),
				HasFind = hasFind,
				DataId = dataId,
				DataDepends = dataDepends,
				DataUrl = dataUrl
			};

			if (items != null)
				viewModel.Items = items.ToList();

			viewModel.IsMultiSelect = isMultiSelect;
			viewModel.Input = htmlHelper.RPHiddenFor(expression);

			if (viewModel.Model != null && viewModel.Items != null && viewModel.Items.Count > 0)
			{
				foreach (var item in viewModel.Items)
				{
					if (viewModel.IsMultiSelect)
					{
						if (((IEnumerable)viewModel.Model).Cast<object>().Any()
							&& ((IEnumerable)viewModel.Model).Cast<object>().Contains(int.Parse(item.Value)))
						{
							viewModel.SelectedValue = string.IsNullOrWhiteSpace(viewModel.SelectedValue) ? item.Text : string.Join("|", viewModel.SelectedValue, item.Text);
							item.Selected = true;
						}
					}
					else
					{
						object value = viewModel.Model;
						if (viewModel.ModelMetadata.ModelType.IsEnum)
							value = Convert.ChangeType(viewModel.Model, item.Value.GetType());
						if (item.Value.Equals(value.ToString()))
						{
							viewModel.SelectedValue = item.Text;
							item.Selected = true;
						}
					}
				}
			}
			return htmlHelper.Partial(Mvc.View.UI.DropDown, viewModel);
		}
	}
}