using RP.Platform.Manager;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace RP.Platform.Extension
{
	public static partial class HtmlExtension
	{
		public static ModelMetadata GetModelMetadata<TModel, TProperty>(
					this HtmlHelper<TModel> htmlHelper,
					Expression<Func<TModel, TProperty>> expression,
					string labelMessage = null,
					string placeholderMessage = null,
					string helpTitleMessage = null,
					string helpDescriptionMessage = null)
		{
			var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

			if (!string.IsNullOrEmpty(labelMessage))
				metadata.AdditionalValues[Mvc.ModelMetadata.LabelMessage] = labelMessage;

			if (!string.IsNullOrEmpty(placeholderMessage))
				metadata.AdditionalValues[Mvc.ModelMetadata.PlaceholderMessage] = placeholderMessage;

			if (!string.IsNullOrEmpty(helpTitleMessage))
				metadata.AdditionalValues[Mvc.ModelMetadata.HelpTitleMessage] = helpTitleMessage;

			if (!string.IsNullOrEmpty(helpDescriptionMessage))
				metadata.AdditionalValues[Mvc.ModelMetadata.HelpDescriptionMessage] = helpDescriptionMessage;

			metadata.AdditionalValues[Mvc.ModelMetadata.ValidationMessage] = htmlHelper.ValidationMessageFor(expression);

			return metadata;
		}
	}
}