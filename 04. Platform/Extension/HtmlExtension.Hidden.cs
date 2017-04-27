using RP.Common.Extension;
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
		public static MvcHtmlString RPHiddenFor<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			IDictionary<string, object> htmlAttributes = null)
		{
			if (htmlAttributes == null)
				htmlAttributes = new Dictionary<string, object>();

			var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			if (metadata.ModelType.IsCollection())
			{
				string value = string.Empty;
				if (metadata.Model != null)
				{
					var items = ((IEnumerable)metadata.Model).Cast<object>();
					value = string.Join(",", items);
				}
				htmlAttributes.Add("Value", value);
			}
			else if (metadata.Model != null && metadata.ModelType.IsEnum)
			{
				object value = Convert.ChangeType(metadata.Model, metadata.ModelType.GetEnumUnderlyingType());
				htmlAttributes.Add("Value", value);
			}

			return htmlHelper.HiddenFor(expression, htmlAttributes);
		}
	}
}